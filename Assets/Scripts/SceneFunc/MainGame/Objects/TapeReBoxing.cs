using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeReBoxing : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        EventManager.instance.MyEventReset();
    }
}
