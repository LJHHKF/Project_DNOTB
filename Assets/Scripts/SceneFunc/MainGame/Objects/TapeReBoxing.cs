using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeReBoxing : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        if (CursorManager.instnace.GetCurrentCursorType() == MyCursor.CursorType.Magnifier)
            MagnifierManager.instance.SetInfoText(MyInfoText.Types.Tape);
        else
        {
            SoundManager.instance.SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.Taping);
            MainEventManager.instance.MyEventReset();
        }
    }
}
