using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SubPuzzleSkipButton : MonoBehaviour
{
    [Serializable] private enum PuzzleType
    {
        DialLock,
        Concentration,
        Pipeline,
        SlidingPuzzle
    }
    [SerializeField] private PuzzleType type;

    public void ButtonClicked()
    {
        switch(type)
        {
            case PuzzleType.DialLock:
                SubPuzzleManager.instance.isDialClear = true;
                break;
            case PuzzleType.Concentration:
                SubPuzzleManager.instance.isConcentrationClear = true;
                break;
            case PuzzleType.Pipeline:
                SubPuzzleManager.instance.isPipelinePuzzleClear = true;
                break;
            case PuzzleType.SlidingPuzzle:
                SubPuzzleManager.instance.isSlidingPuzzleClear = true;
                break;
        }
    }
}
