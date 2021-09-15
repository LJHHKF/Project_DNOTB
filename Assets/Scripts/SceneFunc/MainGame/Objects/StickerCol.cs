using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerCol : MonoBehaviour, IEventObject
{
    private int clickedCnt_withKnife = 0;
    private readonly int clickMax_withKnife = 5;
    private int clickedCnt_none = 0;
    private readonly int clickMax_none = 3;

    private void OnEnable()
    {
        CountReset();
        EventManager.instance.ev_Reset += CountReset;
    }

    private void OnDisable()
    {
        EventManager.instance.ev_Reset -= CountReset;
    }

    private void CountReset()
    {
        clickedCnt_withKnife = 0;
        clickedCnt_none = 0;
    }

    public void Execute()
    {
        MyCursor.CursorType _type = CursorManager.instnace.GetCurrentCursorType();
        if (_type == MyCursor.CursorType.Knife)
        {
            if (clickMax_withKnife <= ++clickedCnt_withKnife)
                BoxMain.instance.RemoveSticker();
        }
        else if (_type == MyCursor.CursorType.Normal)
        {
            if (clickMax_none <= ++clickedCnt_none)
                BoxMain.instance.CubeSetActive();
        }
    }
}
