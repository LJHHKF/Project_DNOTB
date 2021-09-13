using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : DetectOnMouseClick
{
    [Header("For Box Setting")]
    [SerializeField] private GameObject boxObject;
    [SerializeField] private GameObject tapeObject;
    [SerializeField] private Transform tape_LeftUp;
    [SerializeField] private Transform tape_RightDown;
    [SerializeField] private Sprite boxingSprite;
    [SerializeField] private Sprite unboxingSprite;

    private SpriteRenderer m_sprR;

    private bool isUntaped;
    private float timeForSecondEnd = 60.0f;
    private float startTime;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_sprR = boxObject.GetComponent<SpriteRenderer>();
        OnResetEvent();
    }

    private void OnEnable()
    {
        EventManager.instance.ev_Reset += OnResetEvent;
    }

    private void OnDisable()
    {
        EventManager.instance.ev_Reset -= OnResetEvent;
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
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDowned)
            {
                isDowned = false;
                mousePosition = Input.mousePosition;
                mousePosition = myCam.ScreenToWorldPoint(mousePosition);

                Debug.Log($"Up, v_leftUp: {(Vector2)leftUp.position}, v_rightDown: {(Vector2)rightDown.position}, mousePosition: {mousePosition}");

                if(isUntaped)
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
        }

        if(!isUntaped)
        {
            if (startTime < timeForSecondEnd)
                startTime += Time.deltaTime;
            else
            {
                EventManager.instance.isEnding_02 = true;
                EventManager.instance.OnEvent_EndingOpen();
                Debug.Log("2번째 엔딩");
            }
        }
    }

    protected override void Execute()
    {
        m_sprR.sprite = unboxingSprite;
        EventManager.instance.isEnding_01 = true;
        EventManager.instance.OnEvent_EndingOpen();

        Debug.Log("1번째 엔딩");
    }

    private void OnResetEvent()
    {
        Debug.Log("박스 리셋 이벤트");
        isUntaped = false;
        tapeObject.SetActive(true);
        m_sprR.sprite = boxingSprite;
        startTime = 0.0f;
    }
}
