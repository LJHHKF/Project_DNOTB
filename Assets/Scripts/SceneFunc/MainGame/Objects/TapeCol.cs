using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeCol : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        if (CursorManager.instnace.GetCurrentCursorType() != MyCursor.CursorType.Magnifier)
        {
            SoundManager.instance.SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.TapeTearing);
            BoxMain.instance.DoUntaping();
        }
    }
}
