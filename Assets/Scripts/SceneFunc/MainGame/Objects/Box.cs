using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : DetectOnMouseClick
{
    [Header("For Box Setting_defualt")]
    [SerializeField] private GameObject boxObject;
    [SerializeField] private GameObject tapeObject;
    [SerializeField] private Transform tape_LeftUp;
    [SerializeField] private Transform tape_RightDown;
    [SerializeField] private Sprite boxingSprite;
    [SerializeField] private Sprite unboxingSprite;

    [Header("InvoiceSetting")]
    [SerializeField] private GameObject stickerObject;
    [SerializeField] private Transform sticker_LeftUp;
    [SerializeField] private Transform sticker_RightDown;
    [SerializeField] private GameObject invoiceObject;
    [SerializeField] private Transform invoice_LeftUp;
    [SerializeField] private Transform invoice_RightDown;
    

    private SpriteRenderer m_sprR;

    //엔딩1
    private bool isUntaped;
    //엔딩 2
    private readonly float timeForSecondEnd = 60.0f;
    private float startTime;
    //엔딩3
    private readonly int stickerClickMax = 5;
    private int stickerClickCnt;
    private bool isDowned_2;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_sprR = boxObject.GetComponent<SpriteRenderer>();
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

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            mousePosition = myCam.ScreenToWorldPoint(mousePosition);

            Debug.Log($"Down, v_leftUp: {(Vector2)leftUp.position}, v_rightDown: {(Vector2)rightDown.position}, mousePosition: {mousePosition}");

            if(isUntaped)
            {
                if (mousePosition.x > leftUp.position.x
                && mousePosition.x < rightDown.position.x
                && mousePosition.y > rightDown.position.y
                && mousePosition.y < leftUp.position.y)
                    isDowned = true;
                else
                    isDowned = false;
            }
            else
            {
                if (mousePosition.x > tape_LeftUp.position.x
                && mousePosition.x < tape_RightDown.position.x
                && mousePosition.y > tape_RightDown.position.y
                && mousePosition.y < tape_LeftUp.position.y)
                    isDowned = true;
                else
                    isDowned = false;
            }

            if (EventManager.instance.hadKnife)
            {
                if (stickerObject.activeSelf)
                {
                    if (mousePosition.x > sticker_LeftUp.position.x
                    && mousePosition.x < sticker_RightDown.position.x
                    && mousePosition.y > sticker_RightDown.position.y
                    && mousePosition.y < sticker_LeftUp.position.y)
                        isDowned_2 = true;
                    else
                        isDowned_2 = false;
                }
                else if (invoiceObject.activeSelf)
                {
                    if (mousePosition.x > invoice_LeftUp.position.x
                    && mousePosition.x < invoice_RightDown.position.x
                    && mousePosition.y > invoice_RightDown.position.y
                    && mousePosition.y < invoice_LeftUp.position.y)
                        isDowned_2 = true;
                    else
                        isDowned_2 = false;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDowned)
            {
                isDowned = false;
                mousePosition = Input.mousePosition;
                mousePosition = myCam.ScreenToWorldPoint(mousePosition);

                Debug.Log($"Up, v_leftUp: {(Vector2)leftUp.position}, v_rightDown: {(Vector2)rightDown.position}, mousePosition: {mousePosition}");

                if (isUntaped)
                {
                    if (mousePosition.x > leftUp.position.x
                    && mousePosition.x < rightDown.position.x
                    && mousePosition.y > rightDown.position.y
                    && mousePosition.y < leftUp.position.y)
                        Execute();
                }
                else
                {
                    if (mousePosition.x > tape_LeftUp.position.x
                    && mousePosition.x < tape_RightDown.position.x
                    && mousePosition.y > tape_RightDown.position.y
                    && mousePosition.y < tape_LeftUp.position.y)
                    {
                        isUntaped = true;
                        tapeObject.SetActive(false);
                    }
                }
            }
            else if (isDowned_2)
            {
                isDowned_2 = false;
                mousePosition = Input.mousePosition;
                mousePosition = myCam.ScreenToWorldPoint(mousePosition);

                Debug.Log($"Up, v_leftUp: {(Vector2)leftUp.position}, v_rightDown: {(Vector2)rightDown.position}, mousePosition: {mousePosition}");

                if (stickerObject.activeSelf)
                {
                    if (mousePosition.x > sticker_LeftUp.position.x
                    && mousePosition.x < sticker_RightDown.position.x
                    && mousePosition.y > sticker_RightDown.position.y
                    && mousePosition.y < sticker_LeftUp.position.y)
                    {
                        StickerClicked();
                    }
                }
                else if (invoiceObject.activeSelf)
                {
                    if (mousePosition.x > invoice_LeftUp.position.x
                    && mousePosition.x < invoice_RightDown.position.x
                    && mousePosition.y > invoice_RightDown.position.y
                    && mousePosition.y < invoice_LeftUp.position.y)
                    {
                        m_sprR.sprite = unboxingSprite;
                        tapeObject.SetActive(false);
                        invoiceObject.SetActive(false);
                        EventManager.instance.isEnding_03 = true;
                        DataRWManager.instance.InputDataValue("end03", 1, DataRWManager.instance.mySaveData_event);
                        EventManager.instance.OnEvent_EndingOpen();
                        Debug.Log("3번재 엔딩");
                    }
                }
            }
        }

        if(!isUntaped)
        {
            if (startTime < timeForSecondEnd)
                startTime += Time.deltaTime;
            else
            {
                EventManager.instance.isEnding_02 = true;
                DataRWManager.instance.InputDataValue("end02", 1, DataRWManager.instance.mySaveData_event);
                EventManager.instance.OnEvent_EndingOpen();
                Debug.Log("2번째 엔딩");
            }
        }
    }

    protected override void Execute()
    {
        m_sprR.sprite = unboxingSprite;
        if (stickerObject.activeSelf) stickerObject.SetActive(false);
        if (invoiceObject.activeSelf) invoiceObject.SetActive(false);
        EventManager.instance.isEnding_01 = true;
        DataRWManager.instance.InputDataValue("end01", 1, DataRWManager.instance.mySaveData_event);
        EventManager.instance.OnEvent_EndingOpen();
        Debug.Log("1번째 엔딩");
    }

    private void OnResetEvent()
    {
        Debug.Log("박스 리셋 이벤트");
        isDowned = false;
        isDowned_2 = false;
        isUntaped = false;
        tapeObject.SetActive(true);
        m_sprR.sprite = boxingSprite;
        startTime = 0.0f;
    }

    private void OnResetEvent_depth_1()
    {
        stickerObject.SetActive(true);
        invoiceObject.SetActive(false);
        stickerClickCnt = 0;
    }

    private void OnUnsetEvent_depth_1()
    {
        stickerObject.SetActive(false);
        invoiceObject.SetActive(false);
    }

    private void StickerClicked()
    {
        if (stickerClickMax <= ++stickerClickCnt)
        {
            stickerObject.SetActive(false);
            invoiceObject.SetActive(true);
        }
    }
}
