using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SubPuzzleManager : MonoBehaviour
{
    private static SubPuzzleManager m_instance;
    public static SubPuzzleManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<SubPuzzleManager>();
            return m_instance;
        }
    }

    private bool m_isSubPuzzleOn = false;
    public bool isSubPuzzleOn
    {
        get { return m_isSubPuzzleOn; }
        private set { m_isSubPuzzleOn = value; }
    }

    [Header("Screen Border Setting")]
    [SerializeField] private RectTransform pos_LeftTop;
    [SerializeField] private RectTransform pos_RightBottom;
    [SerializeField] private RectTransform pos_LeftTop_Sliding;
    [SerializeField] private RectTransform pos_RightBottom_Sliding;
    private Vector2 m_pos_LeftTop;
    private Vector2 m_pos_RightBottom;
    private Camera cam_main;

    [Header("Puzzle Registration")]
    [SerializeField] private GameObject dialLockObjects;
    private bool m_isDialClear = false;
    public bool isDialClear
    {
        get { return m_isDialClear; }
        set {
            m_isDialClear = value;
            if (m_isDialClear)
                UnActiveDialLock();
            } 
    }
    [SerializeField] private GameObject concentrationObjects;
    private bool m_isConcentrationClear = false;
    public bool isConcentrationClear
    {
        get { return m_isConcentrationClear; }
        set
        {
            m_isConcentrationClear = value;
            if (m_isConcentrationClear)
            {
                UnActiveConcentration();
                GlitchScreenManager.instance.DelayedGlitchOn(1.0f, 0.5f);
                BoxMain.instance.DoUnBoxing(MyEndings.UnboxingType.third_2); // 내부에서 1.5초 딜레이 후 연기 애니메이션 실행부분 존재.
            }
        }
    }
    [SerializeField] private GameObject slidingPuzzleObjects;
    private bool m_isSlidingPuzzleClear = false;
    public bool isSlidingPuzzleClear
    {
        get { return m_isSlidingPuzzleClear; }
        set
        {
            m_isSlidingPuzzleClear = value;
            if (m_isSlidingPuzzleClear)
            {
                UnActiveSlidingPuzzle();
                BoxMain.instance.PortalSetActive(false);
            }
        }
    }
    [SerializeField] private GameObject pipelinePuzzleObjects;
    private bool m_isPipelinePuzzleClear = false;
    public bool isPipelinePuzzleClear
    {
        get { return m_isPipelinePuzzleClear; }
        set
        {
            m_isPipelinePuzzleClear = value;
            if (m_isPipelinePuzzleClear)
                UnActiveWayRotatePuzzle();
        }
    }


    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);

        slidingPuzzleObjects.SetActive(true);
        SlidingPuzzleEventManager.instance.InitSetting();
        ResetEvent();
    }

    private void OnDestroy()
    {
        if (m_instance == this)
        {
            m_instance = null;
            if(MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= ResetEvent;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam_main = Camera.main;
        //m_pos_LeftTop = cam_main.WorldToScreenPoint(pos_LeftTop.position);
        //m_pos_RightBottom = cam_main.WorldToScreenPoint(pos_RightBottom.position);
        m_pos_LeftTop = pos_LeftTop.position;
        m_pos_RightBottom = pos_RightBottom.position;

        MainEventManager.instance.ev_Reset += ResetEvent;
    }

    private void OnEnable()
    {
        isSubPuzzleOn = true;
    }

    private void OnDisable()
    {
        isSubPuzzleOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            //마우스 포지션이 스크린 범위 밖이라면
            if(mousePos.x < m_pos_LeftTop.x ||
               mousePos.x > m_pos_RightBottom.x ||
               mousePos.y < m_pos_RightBottom.y ||
               mousePos.y > m_pos_LeftTop.y)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void ResetEvent()
    {
        dialLockObjects.SetActive(false);
        concentrationObjects.SetActive(false);
        slidingPuzzleObjects.SetActive(false);
        pipelinePuzzleObjects.SetActive(false);
        gameObject.SetActive(false);
        
        m_isDialClear = false;
        m_isConcentrationClear = false;
        m_isSlidingPuzzleClear = false;
        m_isPipelinePuzzleClear = false;
        m_isSubPuzzleOn = false;
    }

    private void OnWindow()
    {
        gameObject.SetActive(true);
    }

    public void OffWindow()
    {
        gameObject.SetActive(false);
        dialLockObjects.SetActive(false);
        concentrationObjects.SetActive(false);
        slidingPuzzleObjects.SetActive(false);
        pipelinePuzzleObjects.SetActive(false);
    }
}


public partial class SubPuzzleManager : MonoBehaviour
{
    public void ActiveDialLock()
    {
        if (!isDialClear)
        {
            OnWindow();
            dialLockObjects.SetActive(true);
            concentrationObjects.SetActive(false);
            slidingPuzzleObjects.SetActive(false);
            pipelinePuzzleObjects.SetActive(false);
        }
    }

    private void UnActiveDialLock()
    {
        dialLockObjects.SetActive(false);
        OffWindow();
    }

    public void ActiveConcentration()
    {
        if (!isConcentrationClear)
        {
            OnWindow();
            concentrationObjects.SetActive(true);
            dialLockObjects.SetActive(false);
            slidingPuzzleObjects.SetActive(false);
            pipelinePuzzleObjects.SetActive(false);
        }
    }

    private void UnActiveConcentration()
    {
        concentrationObjects.SetActive(false);
        OffWindow();
    }

    public void ActiveSlidingPuzzle()
    {
        if (!isSlidingPuzzleClear)
        {
            OnWindow();
            slidingPuzzleObjects.SetActive(true);
            dialLockObjects.SetActive(false);
            concentrationObjects.SetActive(false);
            pipelinePuzzleObjects.SetActive(false);
        }
    }

    private void UnActiveSlidingPuzzle()
    {
        slidingPuzzleObjects.SetActive(false);
        OffWindow();
    }

    public void ActiveWayRotatePuzzle()
    {
        if(!isPipelinePuzzleClear)
        {
            OnWindow();
            pipelinePuzzleObjects.SetActive(true);
            dialLockObjects.SetActive(false);
            concentrationObjects.SetActive(false); 
            slidingPuzzleObjects.SetActive(false);
        }
    }

    private void UnActiveWayRotatePuzzle()
    {
        pipelinePuzzleObjects.SetActive(false);
        OffWindow();
    }

    public void ChangeBorder(bool _isSlidingPz)
    {
        if(_isSlidingPz)
        {
            m_pos_LeftTop = pos_LeftTop_Sliding.position;
            m_pos_RightBottom = pos_RightBottom_Sliding.position;
        }
        else
        {
            m_pos_LeftTop = pos_LeftTop.position;
            m_pos_RightBottom = pos_RightBottom.position;
        }
    }
}
