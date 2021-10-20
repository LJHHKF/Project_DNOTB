using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlidingPuzzlePiece : MonoBehaviour
{
    [SerializeField] private GameObject obj_SelectHighright;
    [SerializeField] private TextMeshProUGUI m_Text;
    [SerializeField] private ObjectShaking shakeManager;
    private SlidingPuzzleSpace m_spaceManager;
    private SlidingPuzzleEventManager m_eventManager;
    private RectTransform m_rect;
    private bool m_isSelected = false;
    private bool isSelected {
        get { return m_isSelected; } 
        set
        {
            m_isSelected = value;
            obj_SelectHighright.SetActive(value);
        }
    }
    private int correctSpaceIndex;
    private int curSpaceIndex;
    private bool isMoved = false;
    private float moveChkDelay = 0.0f;
    //private int instantIndex;

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (isSelected)
        {
            if (Input.GetMouseButtonUp(0))
                isSelected = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
            transform.localPosition = Vector3.zero;

        if (moveChkDelay > 0.0f)
            moveChkDelay -= Time.deltaTime;
        else if (transform.localPosition != Vector3.zero)
            transform.localPosition = Vector3.zero;
    }

    public void InitSetting(GameObject _space,int _instantIndex,int _correctIndex)
    {
        if(m_eventManager == null)
        {
            m_eventManager = SubPuzzleManager.instance.slidingPuzzleEventManager; //transform.GetComponentInParent<SlidingPuzzleEventManager>();
            if (m_eventManager == null)
            {
                Debug.Log("슬라이딩 퍼즐 이벤트 매니저를 피스에 제대로 연결하지 못했습니다.");
                return;
            }
        }
        m_spaceManager = _space.GetComponent<SlidingPuzzleSpace>();
        if (m_spaceManager == null)
        {
            Debug.LogError("이동한 곳에서 공간 관리자를 얻어오지 못했습니다.");
            return;
        }
        isSelected = false;
        correctSpaceIndex = _correctIndex;
        m_Text.text = _correctIndex.ToString();
        m_eventManager.PieceSetParent(_instantIndex, _space.transform);
        //instantIndex = _instantIndex;
    }

    public void PieceSelected()
    {
        isSelected = true;

        if(m_rect == null)
            m_rect = GetComponent<RectTransform>();
    }

    private void PieceMoveTo(SlidingPuzzle.MoveTo _move)
    {
        if (m_spaceManager == null)
        {
            m_spaceManager = transform.GetComponentInParent<SlidingPuzzleSpace>();
        }

        Transform _t = m_spaceManager.GetToMoveTransform(_move);

        if(_t == null) // 실패
        {
            shakeManager.ShakeOn();
            return;
        }
        else
            MoveTo(_t);
    }

    private void MoveTo(Transform _t)
    {
        if (!isMoved)
        {
            gameObject.transform.SetParent(_t);
            StartCoroutine(moveOn());
        }

        IEnumerator moveOn()
        {
            //RectTransform t_rect = _t.GetComponent<RectTransform>();
            isMoved = true;
            moveChkDelay += 1.0f;
            while (true)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, Vector2.zero , 0.1f);
                if ((transform.localPosition).sqrMagnitude < 0.000001)
                    break;
                else
                    yield return null;
            }
            isMoved = false;
            m_spaceManager = _t.GetComponent<SlidingPuzzleSpace>();
            curSpaceIndex = m_spaceManager.prop_SpaceIndex;
            m_eventManager.SetCorrectValue(correctSpaceIndex, (correctSpaceIndex == curSpaceIndex));
            yield break;
        }
    }

    public void MoveToLeft()
    {
        if (isSelected)
        {
            PieceMoveTo(SlidingPuzzle.MoveTo.Left);
            isSelected = false;
        }
    }

    public void MoveToRight()
    {
        if (isSelected)
        {
            PieceMoveTo(SlidingPuzzle.MoveTo.Right);
            isSelected = false;
        }
    }

    public void MoveToUp()
    {
        if (isSelected)
        {
            PieceMoveTo(SlidingPuzzle.MoveTo.Up);
            isSelected = false;
        }
    }

    public void MoveToDown()
    {
        if (isSelected)
        {
            PieceMoveTo(SlidingPuzzle.MoveTo.Down);
            isSelected = false;
        }
    }

}
