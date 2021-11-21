using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyEndings
{
    public enum UnboxingType
    {
        first,
        third,
        fourth
    }

    public enum EndingIndex
    {
        first = 0,
        second = 1,
        third = 2,
        fourth = 3,
        fifth = 4,
        sixth = 5,
        seventh = 6,
        eighth = 7
    }
}

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
    [SerializeField] private GameObject object_ShadowLight;
    [SerializeField] private GameObject object_Collider_Tape;
    [SerializeField] private GameObject object_Collider_underInvoiceTape;
    [SerializeField] private GameObject object_BoxCol;
    [SerializeField] private GameObject object_InBoxObject;
    private InBoxImageManager inBoxObjectImageManager;
    [SerializeField] private GameObject object_Invoice_Cover;
    [SerializeField] private GameObject object_Invoice;
    [SerializeField] private GameObject object_Cube;
    [SerializeField] private GameObject object_Portal;
    [SerializeField] private GameObject object_CubeSticker;

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
    private bool m_isCubeMoved;
    public bool isCubeMoved { get { return m_isCubeMoved; } set { m_isCubeMoved = value; } }
    private bool m_isCubeTape_Untaped;
    public bool isCubeTape_Untaped { get { return m_isCubeTape_Untaped; } set { m_isCubeTape_Untaped = value; } }

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
        EventManager.instance.ev_Reset += OnResetEvent;
        SubEventManager.instance.ev_Endleast_Set_1 += OnResetEvent_depth_1;
        SubEventManager.instance.ev_Endleast_UnSet_1 += OnUnsetEvent_depth_1;
    }

    private void OnDisable()
    {
        EventManager.instance.ev_Reset -= OnResetEvent;
        SubEventManager.instance.ev_Endleast_Set_1 -= OnResetEvent_depth_1;
        SubEventManager.instance.ev_Endleast_UnSet_1 -= OnUnsetEvent_depth_1;
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
                EventManager.instance.OnEvent_EndingOpen();
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
            isUntaped = false;
            wastedTime = 0.0f;
            object_Collider_Tape?.SetActive(true);
            object_BoxCol?.SetActive(false);
            object_InBoxObject.SetActive(false);
            m_sprR.sprite = boxingSprite;
            m_sprR.sortingOrder = sOrder_default;
            object_ShadowLight.SetActive(true);

            isCubeMoved = false;
            isCubeTape_Untaped = false;
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
            object_Portal.SetActive(false);
            object_CubeSticker.SetActive(false);
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
            object_Portal.SetActive(false);
            object_CubeSticker.SetActive(false);
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
            if (isCubeMoved)
            {
                object_Collider_Tape?.SetActive(false);
                m_sprR.sprite = untapingSprite;
                object_Portal.SetActive(true);
            }
            else
            {
                object_Collider_Tape?.SetActive(false);
                m_sprR.sprite = untapingSprite;
                object_BoxCol?.SetActive(true);
            }
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
            object_Cube.SetActive(false);
            object_Portal.SetActive(false);
            object_ShadowLight.SetActive(false);
            SubPuzzleManager.instance.OffWindow();

            object_InBoxObject.SetActive(true);
            inBoxObjectImageManager.SetEnding(_type);
            ev_BoxOpend?.Invoke();

            switch (_type)
            {
                case MyEndings.UnboxingType.first:
                    DataRWManager.instance.InputDataValue("end01", 1, DataRWManager.instance.mySaveData_event);
                    EventManager.instance.OnEvent_EndingOpen();
                    EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.first);
                    Debug.Log("1번째 엔딩");
                    break;
                case MyEndings.UnboxingType.third:
                    DataRWManager.instance.InputDataValue("end03", 1, DataRWManager.instance.mySaveData_event);
                    EventManager.instance.OnEvent_EndingOpen();
                    EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.third);
                    Debug.Log("3번째 엔딩");
                    break;
                case MyEndings.UnboxingType.fourth:
                    //object_CubeSticker.SetActive(false);
                    DataRWManager.instance.InputDataValue("end04", 1, DataRWManager.instance.mySaveData_event);
                    EventManager.instance.OnEvent_EndingOpen();
                    EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.fourth);
                    Debug.Log("4번째 엔딩");
                    break;
            }
        }
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

    public void DoCubePassPortal()
    {
        ev_queue.Enqueue(Execute);
        void Execute()
        {
            object_Cube.SetActive(false);
            object_Portal.SetActive(false);
            object_CubeSticker.SetActive(true);
        }
    }
}
