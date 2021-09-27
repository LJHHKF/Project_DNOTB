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
    [SerializeField] private Sprite boxingSprite;
    [SerializeField] private Sprite untapingSprite;
    [SerializeField] private Sprite unboxingSprite;
    [SerializeField] private Sprite end01_ObjectSprite;

    [Header("Objects linking")]
    [SerializeField] private GameObject object_Collider_Tape;
    [SerializeField] private GameObject object_BoxCol;
    [SerializeField] private GameObject object_InBoxObject;
    private SpriteRenderer inBoxObjectSpriteR;
    [SerializeField] private GameObject object_Sticker;
    [SerializeField] private GameObject objcet_Invoice;
    [SerializeField] private GameObject object_Cube;
    [SerializeField] private GameObject object_Portal;
    [SerializeField] private GameObject object_CubeSticker;

    private SpriteRenderer m_sprR;

    //����2
    private bool m_isUntaped;
    public bool isUntaped { get { return m_isUntaped; } set { m_isUntaped = value; } }
    [Header("Ending Conditions")]
    [SerializeField] private float timeForSecondEnd = 60.0f;
    private float wastedTime;
    //����4
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
    void Start()
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
                Debug.Log("2��° ����");
            }
        }
    }

    private void OnResetEvent()
    {
        isUntaped = false;
        wastedTime = 0.0f;
        object_Collider_Tape.SetActive(true);
        object_BoxCol.SetActive(false);
        object_InBoxObject.SetActive(false);
        m_sprR.sprite = boxingSprite;

        isCubeMoved = false;
    }

    private void OnResetEvent_depth_1()
    {
        object_Sticker.SetActive(true);
        objcet_Invoice.SetActive(false);
        object_Cube.SetActive(false);
        object_Portal.SetActive(false);
        object_CubeSticker.SetActive(false);
    }

    private void OnUnsetEvent_depth_1()
    {
        object_Sticker.SetActive(false);
        objcet_Invoice.SetActive(false);
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
            object_Collider_Tape.SetActive(false);
            m_sprR.sprite = untapingSprite;
            object_Portal.SetActive(true);
        }
        else
        {
            object_Collider_Tape.SetActive(false);
            m_sprR.sprite = untapingSprite;
            object_BoxCol.SetActive(true);
        }
    }

    public void DoUnBoxing(MyEndings.UnboxingType _type)
    {
        m_sprR.sprite = unboxingSprite;
        object_Collider_Tape.SetActive(false);  // ������ if(object_Tape.activeSelf)�� üũ�ؼ� �߾����� �������� ���ݰ� ����
        object_Sticker.SetActive(false);
        objcet_Invoice.SetActive(false);
        object_Cube.SetActive(false);
        object_Portal.SetActive(false);

        object_InBoxObject.SetActive(true);

        switch (_type)
        {
            case MyEndings.UnboxingType.first:
                DataRWManager.instance.InputDataValue("end01", 1, DataRWManager.instance.mySaveData_event);
                EventManager.instance.OnEvent_EndingOpen();
                EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.first);
                inBoxObjectSpriteR.sprite = end01_ObjectSprite;
                Debug.Log("1��° ����");
                break;
            case MyEndings.UnboxingType.third:
                DataRWManager.instance.InputDataValue("end03", 1, DataRWManager.instance.mySaveData_event);
                EventManager.instance.OnEvent_EndingOpen();
                EndCutSceneManager.instance.OnCutScene(MyEndings.EndingIndex.third);
                inBoxObjectSpriteR.sprite = null;
                Debug.Log("3���� ����");
                break;
        }
    }

    public void RemoveSticker()
    {
        object_Sticker.SetActive(false);
        objcet_Invoice.SetActive(true);
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
        Debug.Log("4��° ����");
    }
}
