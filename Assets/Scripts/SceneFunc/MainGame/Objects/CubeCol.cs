using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeCol : MonoBehaviour, IEventObject
{
    [Header("Way Points")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private Transform resetPoint;
    [SerializeField] private Transform firstWayPoint;
    [SerializeField] private Transform wayPointA;
    [SerializeField] private Transform wayPointB;

    [Header("Objects")]
    [SerializeField] private GameObject bottomShadow;
    [SerializeField] private GameObject normalShadow;
    //[SerializeField] private GameObject illumination;

    [Header("Sprite")]
    [SerializeField] private Sprite spr_normal;
    [SerializeField] private Sprite spr_illumination;
    private SpriteRenderer m_sprR;

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
    private bool isPuzzleOn = false;
    

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
        if (m_sprR == null)
            m_sprR = GetComponent<SpriteRenderer>();

        StopAllCoroutines();
        transform.position = resetPoint.position;
        isMoving = false;
        m_sprR.sprite = spr_normal;
        //illumination.SetActive(false);
        isMagnifiered = false;
        cnt_clicked = 0;
        cnt_clicked2 = 0;
        clickTime = 0.0f;
        isPuzzleOn = false;
    }

    public void SetCubeIllumination()
    {
        //illumination.SetActive(true);
        m_sprR.sprite = spr_illumination;
        bottomShadow.SetActive(false);
        normalShadow.SetActive(false);
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
                BoxMain.instance.SetCubeIllumination(false);
            else
            {
                //엔딩5분기
                if (cnt_clicked == 0)
                {
                    BoxMain.instance.SetCubeIllumination(true);
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
                        isPuzzleOn = true;
                    }
                }
                else if (cnt_clicked >= 2 && isPuzzleOn)
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

    public void CubeMovePortal(Vector3 _first, Vector3 _second, bool _isFOrenge)
    {
        StartCoroutine(CubeMoving_Portal(_first, _second, _isFOrenge));
    }

    public void CubeMovePortal_end05(Vector3 _blue)
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
        Vector3 _target = Vector3.zero;
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
            m_Transform.position = Vector3.MoveTowards(m_Transform.position, _target, moveSpeed * Time.deltaTime);

            if (((m_Transform.position) - _target).sqrMagnitude < 0.0000001f)
                isMoving = false;
            else
                yield return null;
        }
        m_Collider.enabled = true;
        yield break;
    }

    private IEnumerator CubeMoving_end05(Vector3 _target)
    {
        isMoving = true;
        m_Collider.enabled = false;
        while(isMoving)
        {
            m_Transform.position = Vector3.MoveTowards(m_Transform.position, _target, moveSpeed * Time.deltaTime);
            if (((m_Transform.position) - _target).sqrMagnitude < 0.0000001f)
                isMoving = false;
            else
                yield return null;
        }
        m_Collider.enabled = true;
        BoxMain.instance.OnCubeOut_end05();
        yield break;
    }

    private IEnumerator CubeMoving_Portal(Vector3 _first, Vector3 _second, bool _isFOrenge)
    {
        isMoving = true;
        m_Collider.enabled = false;
        while(isMoving)
        {
            m_Transform.position = Vector3.MoveTowards(m_Transform.position, _first, moveSpeed * Time.deltaTime);
            if (((m_Transform.position) - _first).sqrMagnitude < 0.0000001f)
                isMoving = false;
            else
                yield return null;
        }
        isMoving = true;
        while(isMoving)
        {
            m_Transform.position = Vector3.MoveTowards(m_Transform.position, _second, moveSpeed * Time.deltaTime);
            if (((m_Transform.position) - _second).sqrMagnitude < 0.0000001f)
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
        //illumination.SetActive(false);
        m_sprR.sprite = spr_normal;
        bottomShadow.SetActive(true);
        normalShadow.SetActive(true);
        yield break;
    }

    private IEnumerator DelayedOnAct(float _delay, Action _act)
    {
        yield return new WaitForSeconds(_delay);
        _act.Invoke();
        yield break;
    }
}
