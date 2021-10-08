using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyEndings
{
    public enum UnboxingType
    {
        first,
        third
    }

    public enum EndingIndex
    {
        first,
        second,
        third,
        fourth
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
    [SerializeField] protected Sprite boxingSprite;
    [SerializeField] protected Sprite untapingSprite;
    [SerializeField] protected Sprite unboxingSprite;
    [SerializeField] protected Sprite end03_ObjectSprite;

    [Header("Box Objects linking")]
    [SerializeField] private GameObject object_Collider_Tape;
    [SerializeField] private GameObject object_Collider_underInvoiceTape;
    [SerializeField] private GameObject object_BoxCol;
    [SerializeField] protected GameObject object_InBoxObject;
    protected SpriteRenderer inBoxObjectSpriteR;
    [SerializeField] protected GameObject object_Invoice_Cover;
    [SerializeField] protected GameObject object_Invoice;
    [SerializeField] protected GameObject object_Cube;
    [SerializeField] protected GameObject object_Portal;
    [SerializeField] protected GameObject object_CubeSticker;

    protected SpriteRenderer m_sprR;

    //엔딩2
    private bool m_isUntaped;
    public bool isUntaped { get { return m_isUntaped; } set { m_isUntaped = value; } }
    [Header("Ending Conditions")]
    [SerializeField] private float timeForSecondEnd = 60.0f;
    private float wastedTime;
    //엔딩4
    private bool m_isCubeMoved;
    public bool isCubeMoved { get { return m_isCubeMoved; } set { m_isCubeMoved = value; } }
    

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

    protected virtual void OnEnable()
    {
        EventManager.instance.ev_Reset += OnResetEvent;
        SubEventManager.instance.ev_Endleast_Set_1 += OnResetEvent_depth_1;
        SubEventManager.instance.ev_Endleast_UnSet_1 += OnUnsetEvent_depth_1;
    }

    protected virtual void OnDisable()
    {
        EventManager.instance.ev_Reset -= OnResetEvent;
        SubEventManager.instance.ev_Endleast_Set_1 -= OnResetEvent_depth_1;
        SubEventManager.instance.ev_Endleast_UnSet_1 -= OnUnsetEvent_depth_1;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_sprR = GetComponent<SpriteRenderer>();
        inBoxObjectSpriteR = object_InBoxObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isUntaped)
        {
            if (wastedTime < timeForSecondEnd)
                wastedTime += Time.deltaTime;
            else
            {
                isUntaped = true;
                DataRWManager.instance.InputDataValue("end02", 1, DataRWManager.instance.mySaveData_event);
                EventManager.instance.OnEvent_EndingOpen();
                EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.second);
                Debug.Log("2번째 엔딩");
            }
        }
    }

    private void OnResetEvent()
    {
        isUntaped = false;
        wastedTime = 0.0f;
        object_Collider_Tape?.SetActive(true);
        object_BoxCol?.SetActive(false);
        object_InBoxObject.SetActive(false);
        m_sprR.sprite = boxingSprite;

        isCubeMoved = false;
    }

    private void OnResetEvent_depth_1()
    {
        object_Invoice_Cover.SetActive(true);
        object_Collider_underInvoiceTape?.SetActive(false);
        object_Invoice.SetActive(false);
        object_Cube.SetActive(false);
        object_Portal.SetActive(false);
        object_CubeSticker.SetActive(false);
    }

    private void OnUnsetEvent_depth_1()
    {
        object_Invoice_Cover.SetActive(false);
        object_Collider_underInvoiceTape?.SetActive(true);
        object_Invoice.SetActive(false);
        object_Cube.SetActive(false);
        object_Portal.SetActive(false);
        object_CubeSticker.SetActive(false);
    }
}

public partial class BoxMain : MonoBehaviour
{
    public void DoUntaping()
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

    public void DoUnBoxing(MyEndings.UnboxingType _type)
    {
        m_sprR.sprite = unboxingSprite;
        object_Collider_Tape?.SetActive(false);  // 기존엔 if(object_Tape.activeSelf)를 체크해서 했었으나 뻘짓임을 깨닫고 수정
        object_Invoice_Cover.SetActive(false);
        object_Invoice.SetActive(false);
        object_Cube.SetActive(false);
        object_Portal.SetActive(false);

        object_InBoxObject.SetActive(true);

        switch (_type)
        {
            case MyEndings.UnboxingType.first:
                DataRWManager.instance.InputDataValue("end01", 1, DataRWManager.instance.mySaveData_event);
                EventManager.instance.OnEvent_EndingOpen();
                EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.first);
                inBoxObjectSpriteR.sprite = null;
                Debug.Log("1번째 엔딩");
                break;
            case MyEndings.UnboxingType.third:
                DataRWManager.instance.InputDataValue("end03", 1, DataRWManager.instance.mySaveData_event);
                EventManager.instance.OnEvent_EndingOpen();
                EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.third);
                inBoxObjectSpriteR.sprite = end03_ObjectSprite;
                Debug.Log("3번째 엔딩");
                break;
        }
    }

    public void RemoveSticker()
    {
        object_Invoice_Cover.SetActive(false);
        object_Invoice.SetActive(true);
    }

    public void CubeSetActive()
    {
        object_Cube.SetActive(true);
    }

    public void DoCubePassPortal()
    {
        object_Cube.SetActive(false);
        object_Portal.SetActive(false);
        object_CubeSticker.SetActive(true);
    }

    public void RemoveCubeSticker()
    {
        object_CubeSticker.SetActive(false);
        DataRWManager.instance.InputDataValue("end04", 1, DataRWManager.instance.mySaveData_event);
        EventManager.instance.OnEvent_EndingOpen();
        EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.fourth);
        Debug.Log("4번째 엔딩");
    }
}
