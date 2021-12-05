using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class BoxMain : MonoBehaviour
{
    private static BoxMain m_instance;
    public static BoxMain instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<BoxMain>();
            return m_instance;
        }
    }

    [Header("Resource Setting")]
    [SerializeField] private Sprite boxingSprite;
    [SerializeField] private Sprite untapingSprite;
    [SerializeField] private Sprite unboxingSprite;

    [Header("Box Objects linking")]
    [SerializeField] private GameObject object_ClosedBoxShadow;
    [SerializeField] private GameObject object_OpendBoxShadow;
    [SerializeField] private GameObject object_Collider_Tape;
    [SerializeField] private GameObject object_Collider_underInvoiceTape;
    [SerializeField] private GameObject object_BoxCol;
    [SerializeField] private GameObject object_InBoxObject;
    private InBoxImageManager inBoxObjectImageManager;
    [SerializeField] private GameObject object_Invoice_Cover;
    [SerializeField] private GameObject object_Invoice;
    [SerializeField] private GameObject object_Cube;
    [SerializeField] private CubeCol cubeManager;
    [SerializeField] private GameObject object_Portal_Blue;
    [SerializeField] private GameObject object_Portal_Orange;
    [SerializeField] private GameObject object_CubeButton;
    [SerializeField] private CubeButtonCol cubeButtonManager;

    [Header("Other")]
    [SerializeField] private int sOrder_default;
    [SerializeField] private int sOrder_open;

    private SpriteRenderer m_sprR;

    //엔딩2
    private bool m_isUntaped;
    public bool isUntaped { get { return m_isUntaped; } set { m_isUntaped = value; } }
    [Header("Ending Conditions")]
    [SerializeField] private float timeForSecondEnd = 60.0f;
    private float wastedTime;
    //엔딩4
    private Vector3 firstPortalPos;
    private Vector3 secondPortalPos;
    private bool m_isCubeIlluminated = false;
    public bool isCubeIlluminated { get { return m_isCubeIlluminated; } }
    private bool m_isCubeSecondIlluminated = false;
    public bool isCubeSecondIlluminated { get { return m_isCubeSecondIlluminated; } }
    private bool isFirstOrenge = false;
    //엔딩5
    private bool m_isCubeOut = false;
    public bool isCubeOut { get { return m_isCubeOut; } }
    private bool m_isEnd05Route = false;
    public bool isEnd05Route { get { return m_isEnd05Route; } }

    private Queue<Action> ev_queue = new Queue<Action>(); //언박싱 처리 중인데 리셋 불려서 '박스는 열린', '송장은 다시 붙어있는' 오류가 발생하는 것 방지용도.
    public event Action ev_BoxOpend;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
    }

    private void OnEnable()
    {
        MainEventManager.instance.ev_Reset += OnResetEvent;
        MainEventManager.instance.ev_Endleast_Set_1 += OnResetEvent_depth_1;
        MainEventManager.instance.ev_Endleast_UnSet_1 += OnUnsetEvent_depth_1;
    }

    private void OnDisable()
    {
        if (MainEventManager.instance != null)
        {
            MainEventManager.instance.ev_Reset -= OnResetEvent;
            MainEventManager.instance.ev_Endleast_Set_1 -= OnResetEvent_depth_1;
            MainEventManager.instance.ev_Endleast_UnSet_1 -= OnUnsetEvent_depth_1;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_sprR = GetComponent<SpriteRenderer>();
        inBoxObjectImageManager = object_InBoxObject.GetComponent<InBoxImageManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ev_queue.Count > 0)
            ev_queue.Dequeue().Invoke();
        if (!isUntaped && !SubPuzzleManager.instance.isSubPuzzleOn && !EndCutSceneManager.instance.isEndingOn)
        {
            if (wastedTime < timeForSecondEnd)
                wastedTime += Time.deltaTime;
            else
            {
                isUntaped = true;
                DataRWManager.instance.InputDataValue("end02", 1, DataRWManager.instance.mySaveData_event);
                MainEventManager.instance.OnEvent_EndingOpen();
                EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.second);
                SubPuzzleManager.instance.OffWindow();
                Debug.Log("2번째 엔딩");
            }
        }
    }

    private void OnResetEvent()
    {
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            StopAllCoroutines();
            isUntaped = false;
            m_isCubeIlluminated = false;
            m_isCubeSecondIlluminated = false;
            isFirstOrenge = false;
            m_isCubeOut = false;
            m_isEnd05Route = false;
            wastedTime = 0.0f;
            object_Collider_Tape?.SetActive(true);
            object_BoxCol?.SetActive(false);
            object_InBoxObject.SetActive(false);
            m_sprR.sprite = boxingSprite;
            m_sprR.sortingOrder = sOrder_default;
            object_ClosedBoxShadow.SetActive(true);
            object_OpendBoxShadow.SetActive(false);
        }
    }

    private void OnResetEvent_depth_1()
    {
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            object_Invoice_Cover.SetActive(true);
            object_Collider_underInvoiceTape?.SetActive(false);
            object_Invoice.SetActive(false);
            object_Cube.SetActive(false);
            object_Portal_Blue.SetActive(false);
            object_Portal_Orange.SetActive(false);
            object_CubeButton.SetActive(true);
        }
    }

    private void OnUnsetEvent_depth_1()
    {
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            object_Invoice_Cover.SetActive(false);
            object_Collider_underInvoiceTape?.SetActive(true);
            object_Invoice.SetActive(false);
            object_Cube.SetActive(false);
            object_Portal_Blue.SetActive(false);
            object_Portal_Orange.SetActive(false);
            object_CubeButton.SetActive(false);
        }
    }
}

public partial class BoxMain : MonoBehaviour
{
    public void DoUntaping()
    {
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            isUntaped = true;
            object_Collider_Tape?.SetActive(false);
            m_sprR.sprite = untapingSprite;
            object_BoxCol?.SetActive(true);
        }
    }

    public void DoUnBoxing(MyEndings.UnboxingType _type)
    {
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            m_sprR.sprite = unboxingSprite;
            m_sprR.sortingOrder = sOrder_open;
            object_Collider_Tape?.SetActive(false);  // 기존엔 if(object_Tape.activeSelf)를 체크해서 했었으나 뻘짓임을 깨닫고 수정
            object_Invoice_Cover.SetActive(false);
            object_Invoice.SetActive(false);
            object_ClosedBoxShadow.SetActive(false);
            object_OpendBoxShadow.SetActive(true);
            SubPuzzleManager.instance.OffWindow();

            object_InBoxObject.SetActive(true);
            inBoxObjectImageManager.SetEnding(_type);
            ev_BoxOpend?.Invoke();

            if (_type != MyEndings.UnboxingType.fourth)
            {
                object_Cube.SetActive(false);
                object_Portal_Blue.SetActive(false);
                object_Portal_Orange.SetActive(false);
            }
            else
                StartCoroutine(CubeObjectsOut());

            switch (_type)
            {
                case MyEndings.UnboxingType.first:
                    DataRWManager.instance.InputDataValue("end01", 1, DataRWManager.instance.mySaveData_event);
                    MainEventManager.instance.OnEvent_EndingOpen();
                    EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.first);
                    Debug.Log("1번째 엔딩");
                    break;
                case MyEndings.UnboxingType.third_1:
                    DataRWManager.instance.InputDataValue("end03_1", 1, DataRWManager.instance.mySaveData_event);
                    MainEventManager.instance.OnEvent_EndingOpen();
                    EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.third_1);
                    Debug.Log("3_1번째 엔딩");
                    break;
                case MyEndings.UnboxingType.third_2:
                    inBoxObjectImageManager.SmokeAnimOn(1.5f);
                    DataRWManager.instance.InputDataValue("end03_2", 1, DataRWManager.instance.mySaveData_event);
                    MainEventManager.instance.OnEvent_EndingOpen();
                    StartCoroutine(DelayedOnAct(2.5f,Execute_3_1));
                    void Execute_3_1()
                    {
                        EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.third_2);
                    }
                    Debug.Log("3_2번째 엔딩");
                    break;
                case MyEndings.UnboxingType.fourth:
                    //object_CubeSticker.SetActive(false);
                    DataRWManager.instance.InputDataValue("end04", 1, DataRWManager.instance.mySaveData_event);
                    MainEventManager.instance.OnEvent_EndingOpen();
                    StartCoroutine(DelayedOnAct(1.0f, Execute_4));
                    void Execute_4()
                    {
                        EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.fourth);
                    }
                    Debug.Log("4번째 엔딩");
                    break;
                case MyEndings.UnboxingType.fifth:
                    DataRWManager.instance.InputDataValue("end05", 1, DataRWManager.instance.mySaveData_event);
                    MainEventManager.instance.OnEvent_EndingOpen();
                    EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.fifth);
                    Debug.Log("5번째 엔딩");
                    break;
            }
        }
    }

    private IEnumerator CubeObjectsOut()
    {
        SpriteRenderer _sprR_Cube, _sprR_P_Orange, _sprR_P_Blue;
        _sprR_Cube = object_Cube.GetComponent<SpriteRenderer>();
        _sprR_P_Orange = object_Portal_Orange.GetComponent<SpriteRenderer>();
        _sprR_P_Blue = object_Portal_Blue.GetComponent<SpriteRenderer>();
        float _curAlpha = 1.0f;
        Color _color;
        while (_curAlpha <= 0.001f)
        {
            _curAlpha -= Time.deltaTime / 1.0f; // * 1.0f. 1초마다 1.0f만큼 빼라.
            _color = new Color(1.0f, 1.0f, 1.0f, _curAlpha);
            _sprR_Cube.color = _color;
            _sprR_P_Blue.color = _color;
            _sprR_P_Orange.color = _color;
        }
        object_Cube.SetActive(false);
        object_Portal_Orange.SetActive(false);
        object_Portal_Blue.SetActive(false);
        yield break;
    }

    public void RemoveSticker()
    {
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            object_Invoice_Cover.SetActive(false);
            object_Invoice.SetActive(true);
        }
    }

    public void CubeSetActive()
    {   
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            object_Cube.SetActive(true);
        }
    }

    public void PortalSetActive(bool _isOrenge)
    {
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            if (_isOrenge)
            {
                object_Portal_Orange.SetActive(true);
                firstPortalPos = object_Portal_Orange.transform.position;
                isFirstOrenge = true;
            }
            else
            {
                object_Portal_Blue.SetActive(true);
                firstPortalPos = object_Portal_Blue.transform.position;
                isFirstOrenge = false;
            }
        }
    }

    public void SetCubeIllumination(bool _isEnd05)
    {
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            if(_isEnd05)
            {
                //m_isCubeIlluminated = true;
                m_isEnd05Route = true;
                cubeManager.SetCubeIllumination();
            }                
            else if (!m_isCubeIlluminated)
            {
                m_isCubeIlluminated = true;
                cubeManager.SetCubeIllumination();
                //Another Portal Set
                //if (MagnifierManager.instance.isCubeInvest)
                if (object_Portal_Orange.activeSelf)
                {
                    object_Portal_Blue.SetActive(true);
                    secondPortalPos = object_Portal_Blue.transform.position;
                }
                else if (object_Portal_Blue.activeSelf)
                {
                    object_Portal_Orange.SetActive(true);
                    secondPortalPos = object_Portal_Orange.transform.position;
                }
            }
            else
            {
                m_isCubeSecondIlluminated = true;
                cubeManager.SetCubeIllumination();
            }
        }
    }

    public void CubeMoveToPortal()
    {
        cubeManager.CubeMovePortal(firstPortalPos, secondPortalPos, isFirstOrenge);
    }

    public void CubeMoveToPortal_end05()
    {
        cubeManager.CubeMovePortal_end05(object_Portal_Blue.transform.position);
    }

    public void OnCubeOut_end05()
    {
        StartCoroutine(CubeOut_end05());
        m_isCubeOut = true;
    }

    private IEnumerator CubeOut_end05()
    {
        SpriteRenderer _sprR_Cube, _sprR_P_Blue;
        _sprR_Cube = object_Cube.GetComponent<SpriteRenderer>();
        _sprR_P_Blue = object_Portal_Blue.GetComponent<SpriteRenderer>();
        float _curAlpha = 1.0f;
        Color _color;
        while (_curAlpha <= 0.001f)
        {
            _curAlpha -= Time.deltaTime / 1.0f; // * 1.0f. 1초마다 1.0f만큼 빼라.
            _color = new Color(1.0f, 1.0f, 1.0f, _curAlpha);
            _sprR_Cube.color = _color;
            _sprR_P_Blue.color = _color;
        }
        object_Cube.SetActive(false);
        object_Portal_Blue.SetActive(false);
        yield break;
    }

    public int GetButtonClickedCount()
    {
        return cubeButtonManager.GetCntClicked();
    }

    private IEnumerator DelayedOnAct(float _delay, Action _act)
    {
        yield return new WaitForSeconds(_delay);
        _act.Invoke();
        yield break;
    }
}
