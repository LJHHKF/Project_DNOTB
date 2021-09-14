using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerCol : MonoBehaviour, IEventObject
{
    private int clickedCnt = 0;
    private readonly int clickMax = 5;

    private void OnEnable()
    {
        clickedCnt = 0;
    }

    public void Execute()
    {
        if (EventManager.instance.hadKnife)
        {
            if (clickMax <= ++clickedCnt)
                BoxMain.instance.RemoveSticker();
        }
    }
}
