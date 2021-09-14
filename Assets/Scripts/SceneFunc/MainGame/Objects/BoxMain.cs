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
    [SerializeField] private Sprite unboxingSprite;

    [Header("Objects linking")]
    [SerializeField] private GameObject object_Tape;
    [SerializeField] private GameObject object_BoxCol;
    [SerializeField] private GameObject object_Sticker;
    [SerializeField] private GameObject objcet_Invoice;

    private SpriteRenderer m_sprR;

    //엔딩2
    private bool isUntaped;
    private readonly float timeForSecondEnd = 60.0f;
    private float wastedTime;
    //
    

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
                EventManager.instance.isEnding_02 = true;
                DataRWManager.instance.InputDataValue("end02", 1, DataRWManager.instance.mySaveData_event);
                EventManager.instance.OnEvent_EndingOpen();
                Debug.Log("2번째 엔딩");
            }
        }
    }

    private void OnResetEvent()
    {
        isUntaped = false;
        wastedTime = 0.0f;
        object_Tape.SetActive(true);
        object_BoxCol.SetActive(false);
        m_sprR.sprite = boxingSprite;
    }

    private void OnResetEvent_depth_1()
    {
        object_Sticker.SetActive(true);
        objcet_Invoice.SetActive(false);
    }

    private void OnUnsetEvent_depth_1()
    {
        object_Sticker.SetActive(false);
        objcet_Invoice.SetActive(false);
    }
}

public partial class BoxMain : MonoBehaviour
{
    public void DoUntaping()
    {
        isUntaped = true;

        object_Tape.SetActive(false);
        object_BoxCol.SetActive(true);
    }

    public void DoUnBoxing(MyEndings.UnboxingType _type)
    {
        m_sprR.sprite = unboxingSprite;
        if (object_Tape.activeSelf) object_Tape.SetActive(false);
        if (object_Sticker.activeSelf) object_Sticker.SetActive(false);
        if (objcet_Invoice.activeSelf) objcet_Invoice.SetActive(false);
        
        switch (_type)
        {
            case MyEndings.UnboxingType.first:
                EventManager.instance.isEnding_01 = true;
                DataRWManager.instance.InputDataValue("end01", 1, DataRWManager.instance.mySaveData_event);
                EventManager.instance.OnEvent_EndingOpen();
                Debug.Log("1번째 엔딩");
                break;
            case MyEndings.UnboxingType.third:
                EventManager.instance.isEnding_03 = true;
                DataRWManager.instance.InputDataValue("end03", 1, DataRWManager.instance.mySaveData_event);
                EventManager.instance.OnEvent_EndingOpen();
                Debug.Log("3번재 엔딩");
                break;
        }
    }

    public void RemoveSticker()
    {
        object_Sticker.SetActive(false);
        objcet_Invoice.SetActive(true);
    }
}
