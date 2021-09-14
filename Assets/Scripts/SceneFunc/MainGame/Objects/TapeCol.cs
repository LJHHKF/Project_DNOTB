using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeCol : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        BoxMain.instance.DoUntaping();
    }
}
