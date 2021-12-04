using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SlidingPuzzle
{
    public enum MoveTo
    {
        Left,
        Right,
        Up,
        Down
    }
}

public class SlidingPuzzleEventManager : MonoBehaviour
{
    private static SlidingPuzzleEventManager m_instance;
    public static SlidingPuzzleEventManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<SlidingPuzzleEventManager>();
            return m_instance;
        }
    }

    [Serializable]
    private struct PieceInfo
    {
        public int correctSpaceIndex;
        public Sprite sprite;
    }

    [SerializeField] private GameObject[] spaces;
    [SerializeField] private PieceInfo[] piecesInfo;
    [SerializeField] private GameObject completeImage;
    [SerializeField] private float completeDelay = 1.0f;
    [SerializeField] private GameObject piecePrefab;

    private List<int> list_CorrectSpaceIndex = new List<int>();
    private bool[] array_PieceCorrect = new bool[8];
    private GameObject[] array_Pieces = new GameObject[8];
    private int[] array_correctNum = new int[8];
    private int endCnt;


    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    public void InitSetting()
    {
        list_CorrectSpaceIndex.Capacity = 9;
        InitSetPieces();
        MainEventManager.instance.ev_Reset += InitSetPieces;
    }

    private void OnDestroy()
    {
        if (m_instance == this)
        {
            m_instance = null;
            if(MainEventManager.instance != null)
                MainEventManager.instance.ev_Reset -= InitSetPieces;
        }
    }

    private void OnEnable()
    {
        if (completeImage.activeSelf)
            SubPuzzleManager.instance.isSlidingPuzzleClear = true;
    }

    private void SetList()
    {
        list_CorrectSpaceIndex.Clear();
        for (int i = 0; i < spaces.Length; i++)
            list_CorrectSpaceIndex.Add(i);
        for (int i = 0; i < array_PieceCorrect.Length; i++)
            array_PieceCorrect[i] = false;
    }

    /// <summary>
    /// 피스가 이동했을 때, 현재 이 피스가 적절한 위치 값으로 이동했는지 아닌지 체크한 결과값 함수. 완료 체크도 동시 진행.
    /// </summary>
    /// <param name="_index">PieceInfo의 spaceIndex 값이자 피스 관리자의 correctSpaceIndex 값. 1~9 value to change 0~8.</param>
    /// <param name="_value">변경할 값</param>
    public void SetCorrectValue(int _index, bool _value)
    {
        array_PieceCorrect[_index - 1] = _value;
        for (int i = 0; i < array_PieceCorrect.Length; i++)
        {
            if (!array_PieceCorrect[i])
                return;
        }
        StartCoroutine(EndMove());
        IEnumerator EndMove()
        {
            endCnt = 0;
            for (int i = 0; i < array_Pieces.Length; i++)
                array_Pieces[i].GetComponent<SlidingPuzzlePiece>().moveEndPos();
            while(true)
            {
                if (endCnt >= array_Pieces.Length)
                    break;
                else
                    yield return null;
            }
            completeImage.SetActive(true);
            yield return new WaitForSeconds(completeDelay);
            completeImage.SetActive(false);
            SubPuzzleManager.instance.isSlidingPuzzleClear = true;
            yield break;
        }
    }

    private void InitSetPieces()
    {
        StopAllCoroutines();
        endCnt = 0;
        //위치 값은 피스 OnEnable
        completeImage.SetActive(false);

        if (array_Pieces[0] == null)
        {
            for (int i = 0; i < array_Pieces.Length; i++)
                array_Pieces[i] = Instantiate(piecePrefab, gameObject.transform);
        }

        completeImage.SetActive(false);
        SetList();
        int rand;
        int cnt = 0;
        for (int i = 0; i < spaces.Length; i++)
        {
            rand = UnityEngine.Random.Range(0, list_CorrectSpaceIndex.Count);
            if (list_CorrectSpaceIndex[rand] == 0)
            {
                spaces[i].GetComponent<SlidingPuzzleSpace>().isHad = false;
                list_CorrectSpaceIndex.RemoveAt(rand);
            }
            else
            {
                spaces[i].GetComponent<SlidingPuzzleSpace>().isHad = true;
                array_correctNum[cnt] = piecesInfo[list_CorrectSpaceIndex[rand] - 1].correctSpaceIndex;
                array_Pieces[cnt].GetComponent<SlidingPuzzlePiece>().InitSetting
                    (spaces[i],
                    piecesInfo[list_CorrectSpaceIndex[rand] - 1].correctSpaceIndex,
                    piecesInfo[list_CorrectSpaceIndex[rand] - 1].sprite
                    ); // InitSetting 함수는 추후 스프라이트도 넣고 해야함. 
                list_CorrectSpaceIndex.RemoveAt(rand);
                cnt++;
            }
        }
    }

    //public void GetInitPiecesSetting(int _index, out int _spaceIndex)
    //{
    //    _spaceIndex = array_correctNum[_index];
    //}

    public void EndingMoveCompleteCountUp()
    {
        endCnt++;
    }
}
