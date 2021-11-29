using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCol : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        if (CursorManager.instnace.GetCurrentCursorType() == MyCursor.CursorType.Magnifier)
            MagnifierManager.instance.SetInfoText(MyInfoText.Types.Box);
        else
            BoxMain.instance.DoUnBoxing(MyEndings.UnboxingType.first);
    }
}
