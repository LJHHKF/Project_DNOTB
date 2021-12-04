using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeCol : MonoBehaviour, IEventObject
{
    [Header("Way Points")]
    [SerializeField] private Transform resetPoint;
    [SerializeField] private Transform firstWayPoint;
    [SerializeField] private Transform wayPointA;
    [SerializeField] private Transform wayPointB;

    [Header("Objects")]
    [SerializeField] private GameObject bottomShadow;
    [SerializeField] private GameObject illumination;

    private Collider2D m_Collider;
    private Transform m_Transform;
    private bool p_isMoving;
    private bool isMoving {
        get { return p_isMoving; }
        set
        { 
            p_isMoving = value;
            bottomShadow.SetActive(!p_isMoving);
        }
    }
    private bool m_isButtonClicked = false;
    public bool isButtonClicked { get { return m_isButtonClicked; } set { m_isButtonClicked = value; } } //버튼이 클릭되지 않고 먼저 클릭되었다면 엔딩 05 분기임.
    private bool isMagnifiered = false;
    private int cnt_clicked = 0;
    private int cnt_clicked2 = 0;
    private float clickTime = 0.0f;
    private readonly float moveSpeed = 0.01f;

    private void Start()
    {
        //m_Collider = GetComponent<Collider2D>();
        //m_Transform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        MyReset();
        MainEventManager.instance.ev_Reset += MyReset;
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        StartCoroutine(CubeMoving(0));
    }

    private void OnDisable()
    {
        if(MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= MyReset;
    }

    private void MyReset()
    {
        StopAllCoroutines();
        transform.position = resetPoint.position;
        isMoving = false;
        illumination.SetActive(false);
        isMagnifiered = false;
        cnt_clicked = 0;
        cnt_clicked2 = 0;
        clickTime = 0.0f;
    }

    public void SetCubeIllumination()
    {
        illumination.SetActive(true);
        StartCoroutine(IlluminationOff());
        isMagnifiered = false;
    }

    public void Execute()
    {
        if(CursorManager.instnace.GetCurrentCursorType() == MyCursor.CursorType.Magnifier)
        {
            if (!BoxMain.instance.isCubeIlluminated)
                MagnifierManager.instance.SetInfoText(MyInfoText.Types.Cube_Init);
            else
            {
                if (!isMagnifiered)
                {
                    MagnifierManager.instance.SetInfoText(MyInfoText.Types.Cube_Second);
                    isMagnifiered = true;
                }
                else
                    MagnifierManager.instance.SetInfoText(MyInfoText.Types.Cube_Final);
            }
        }
        else
        {
            if (BoxMain.instance.GetButtonClickedCount() >= 2)
                BoxMain.instance.SetCubeIllumination();
            else
            {
                //엔딩5분기
                if (cnt_clicked == 0)
                {
                    BoxMain.instance.SetCubeIllumination();
                    cnt_clicked++;
                }
                else if(cnt_clicked == 1)
                {
                    GlitchScreenManager.instance.DelayedGlitchOn(1.0f, 0.5f);
                    StartCoroutine(DelayedOnAct(1.5f, Execute));
                    cnt_clicked++;
                    void Execute()
                    {
                        SubPuzzleManager.instance.ActiveSlidingPuzzle();
                    }
                }
                else if (cnt_clicked >= 2)
                {
                    if (!SubPuzzleManager.instance.isSlidingPuzzleClear)
                        SubPuzzleManager.instance.ActiveSlidingPuzzle();
                    else
                    {
                        clickTime = 1.0f;
                        cnt_clicked2++;
                        if(cnt_clicked2 >= 2)
                        {
                            BoxMain.instance.CubeMoveToPortal_end05();
                        }
                    }
                }
            }
        }
        if (clickTime > 0.0f)
            clickTime -= Time.deltaTime;
        else
            cnt_clicked2 = 0;
    }

    public void CubeMovePortal(Vector2 _first, Vector2 _second, bool _isFOrenge)
    {
        StartCoroutine(CubeMoving_Portal(_first, _second, _isFOrenge));
    }

    public void CubeMovePortal_end05(Vector2 _blue)
    {
        StartCoroutine(CubeMoving_end05(_blue));
    }

    private IEnumerator CubeMoving(int _index)
    {
        if (m_Collider == null)
            m_Collider = GetComponent<Collider2D>();
        if (m_Transform == null)
            m_Transform = GetComponent<Transform>();

        isMoving = true;
        Vector2 _target = Vector2.zero;
        m_Collider.enabled = false;
        switch (_index)
        {
            case 0:
                _target = firstWayPoint.position;
                    break;
            case 1:
                _target = wayPointA.position;
                break;
            case 2:
                _target = wayPointB.position;
                break;
            default:
                m_Collider.enabled = true;
                isMoving = false;
                yield break;
        }
        while (isMoving)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, _target, moveSpeed);

            if ((((Vector2)m_Transform.position) - _target).sqrMagnitude < 0.0000001f)
                isMoving = false;
            else
                yield return null;
        }
        m_Collider.enabled = true;
        yield break;
    }

    private IEnumerator CubeMoving_end05(Vector2 _target)
    {
        isMoving = true;
        m_Collider.enabled = false;
        while(isMoving)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, _target, moveSpeed);
            if ((((Vector2)m_Transform.position) - _target).sqrMagnitude < 0.0000001f)
                isMoving = false;
            else
                yield return null;
        }
        m_Collider.enabled = true;
        BoxMain.instance.OnCubeOut_end05();
        yield break;
    }

    private IEnumerator CubeMoving_Portal(Vector2 _first, Vector2 _second, bool _isFOrenge)
    {
        isMoving = true;
        m_Collider.enabled = false;
        while(isMoving)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, _first, moveSpeed);
            if ((((Vector2)m_Transform.position) - _first).sqrMagnitude < 0.0000001f)
                isMoving = false;
            else
                yield return null;
        }
        isMoving = true;
        while(isMoving)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, _second, moveSpeed);
            if ((((Vector2)m_Transform.position) - _second).sqrMagnitude < 0.0000001f)
                isMoving = false;
            else
                yield return null;
        }
        if (_isFOrenge)
            StartCoroutine(CubeMoving(1));
        else
            StartCoroutine(CubeMoving(2));
        yield break;
    }

    private IEnumerator IlluminationOff()
    {
        yield return new WaitForSeconds(1.0f);
        illumination.SetActive(false);
        yield break;
    }

    private IEnumerator DelayedOnAct(float _delay, Action _act)
    {
        yield return new WaitForSeconds(_delay);
        _act.Invoke();
        yield break;
    }
}
