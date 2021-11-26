using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeReBoxing : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        SoundManager.instance.SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.Taping);
        MainEventManager.instance.MyEventReset();
    }
}
