using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PipelinePuzzle
{   
    public enum ClockwiseRotate
    {
        Clockwise,
        ConterClockwise
    }
}

public class PipelinePuzzleEventManager : MonoBehaviour
{
    private static PipelinePuzzleEventManager m_instance;
    public static PipelinePuzzleEventManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<PipelinePuzzleEventManager>();
            return m_instance;
        }
    }




    private PipelinePiece[] pieces;
    private PipelinePiece selected = null;

    // Start is called before the first frame update
    void Start()
    {
        pieces = transform.GetComponentsInChildren<PipelinePiece>();
    }

    private void ResetEvent()
    {
        selected = null;
    }

    public void RotateBTN_clcokwise()
    {
        if (selected != null)
        {
            selected.Rotate(PipelinePuzzle.ClockwiseRotate.Clockwise);
        }
    }

    public void RotateBTN_conterclockwise()
    {
        if (selected != null)
        {
            selected.Rotate(PipelinePuzzle.ClockwiseRotate.ConterClockwise);
        }
    }

    public void SetSelectedPiece(PipelinePiece _piece)
    {
        if (selected != null)
            selected.UnsetSelect();

        selected = _piece;
    }

    public void UnSetSelectedPiece()
    {
        if (selected != null)
            selected.UnsetSelect();

        selected = null;
    }

    public void CheckEnd()
    {
        for(int i = 0; i < pieces.Length; i++)
        {
            if(!pieces[i].CheckRotCorrect())
                return;
        }
        SubPuzzleManager.instance.isPipelinePuzzleClear = true;
    }
}
