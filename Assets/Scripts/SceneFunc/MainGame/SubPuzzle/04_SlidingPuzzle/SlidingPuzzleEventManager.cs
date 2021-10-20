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
    [Serializable]
    private struct PieceInfo
    {
        public int correctSpaceIndex;
        public Sprite sprite;
    }

    [SerializeField] private GameObject[] spaces;
    [SerializeField] private PieceInfo[] piecesInfo;
    [SerializeField] private GameObject piecePrefab;

    private List<int> list_CorrectSpaceIndex = new List<int>();
    private bool[] array_PieceCorrect = new bool[8];
    private GameObject[] array_Pieces = new GameObject[8];
    private int[] array_correctNum = new int[8];


    // Start is called before the first frame update
    void Start()
    {
        list_CorrectSpaceIndex.Capacity = 9;
        InitSetPieces();
        EventManager.instance.ev_Reset += InitSetPieces;
    }

    private void OnDestroy()
    {
        EventManager.instance.ev_Reset -= InitSetPieces;
    }

    private void SetList()
    {
        list_CorrectSpaceIndex.Clear();
        for (int i = 0; i < spaces.Length; i++)
        {
            list_CorrectSpaceIndex.Add(i);
        }
        for (int i = 0; i < array_PieceCorrect.Length; i++)
            array_PieceCorrect[i] = false;
    }

    /// <summary>
    /// �ǽ��� �̵����� ��, ���� �� �ǽ��� ������ ��ġ ������ �̵��ߴ��� �ƴ��� üũ�� ����� �Լ�. �Ϸ� üũ�� ���� ����.
    /// </summary>
    /// <param name="_index">PieceInfo�� spaceIndex ������ �ǽ� �������� correctSpaceIndex ��. 1~9 value to change 0~8.</param>
    /// <param name="_value">������ ��</param>
    public void SetCorrectValue(int _index, bool _value)
    {
        array_PieceCorrect[_index - 1] = _value;
        bool isEnd = true;
        for(int i = 0; i < array_PieceCorrect.Length; i++)
        {
            if (!array_PieceCorrect[i])
            {
                isEnd = false;
                Debug.Log($"Chk Failed:{i}");
                break;
            }
        }
        if(isEnd)
            SubPuzzleManager.instance.isSlidingPuzzleClear = true;
    }

    private void InitSetPieces()
    {
        if (array_Pieces[0] == null)
        {
            for (int i = 0; i < array_Pieces.Length; i++)
                array_Pieces[i] = Instantiate(piecePrefab, gameObject.transform) as GameObject;
        }

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
                    cnt,
                    piecesInfo[list_CorrectSpaceIndex[rand] - 1].correctSpaceIndex
                    ); // InitSetting �Լ��� ���� ��������Ʈ�� �ְ� �ؾ���. 
                list_CorrectSpaceIndex.RemoveAt(rand);
                cnt++;
            }
        }
    }

    public void PieceSetParent(int _pieceIndex, Transform _parent)
    {
        array_Pieces[_pieceIndex].transform.SetParent(_parent);
        array_Pieces[_pieceIndex].transform.localPosition = Vector2.zero;
    }

    public void GetInitPiecesSetting(int _index, out int _spaceIndex)
    {
        _spaceIndex = array_correctNum[_index];
    }
}