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

    [SerializeField] private Vector2 initPos_xy; // 작업 편의용도. 작업 후 비활성화해두면 버튼을 처음에 두번 눌러야 활성화됨.
    private bool m_isSubPuzzleOn = false;
    public bool isSubPuzzleOn
    {
        get { return m_isSubPuzzleOn; }
        private set { m_isSubPuzzleOn = value; }
    }

    [Header("Screen Border Setting")]
    [SerializeField] private Transform pos_LeftTop;
    private Vector2 m_pos_LeftTop;
    [SerializeField] private Transform pos_RightBottom;
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
                UnActiveConcentration();
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
                UnActiveSlidingPuzzle();
        }
    }
    [SerializeField] private GameObject wayRotatePuzzleObjects;
    private bool m_isWayRotatePuzzleClear = false;
    public bool isWayPuzzleClear
    {
        get { return m_isWayRotatePuzzleClear; }
        set
        {
            m_isWayRotatePuzzleClear = value;
            if (m_isWayRotatePuzzleClear)
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
            EventManager.instance.ev_Reset -= ResetEvent;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = initPos_xy;

        cam_main = Camera.main;
        m_pos_LeftTop = cam_main.WorldToScreenPoint(pos_LeftTop.position);
        m_pos_RightBottom = cam_main.WorldToScreenPoint(pos_RightBottom.position);

        EventManager.instance.ev_Reset += ResetEvent;
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
        if(Input.GetMouseButtonDown(0))
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
        wayRotatePuzzleObjects.SetActive(false);
        gameObject.SetActive(false);
        
        m_isDialClear = false;
        m_isConcentrationClear = false;
        m_isSlidingPuzzleClear = false;
        m_isWayRotatePuzzleClear = false;
        m_isSubPuzzleOn = false;
    }

    private void OnWindow()
    {
        gameObject.SetActive(true);
    }

    private void OffWindow()
    {
        gameObject.SetActive(false);
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
            wayRotatePuzzleObjects.SetActive(false);
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
            wayRotatePuzzleObjects.SetActive(false);
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
            wayRotatePuzzleObjects.SetActive(false);
        }
    }

    private void UnActiveSlidingPuzzle()
    {
        slidingPuzzleObjects.SetActive(false);
        OffWindow();
    }

    public void ActiveWayRotatePuzzle()
    {
        if(!isWayPuzzleClear)
        {
            OnWindow();
            wayRotatePuzzleObjects.SetActive(true);
            dialLockObjects.SetActive(false);
            concentrationObjects.SetActive(false); 
            slidingPuzzleObjects.SetActive(false);
        }
    }

    private void UnActiveWayRotatePuzzle()
    {
        wayRotatePuzzleObjects.SetActive(false);
        OffWindow();
    }
}
