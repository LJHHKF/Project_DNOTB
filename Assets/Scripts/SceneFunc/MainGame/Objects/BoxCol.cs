using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCol : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        BoxMain.instance.DoUnBoxing(MyEndings.UnboxingType.first);
    }
}
