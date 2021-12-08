using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCol : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        if(CursorManager.instnace.GetCurrentCursorType() == MyCursor.CursorType.Magnifier)
            MagnifierManager.instance.SetInfoText(MyInfoText.Types.Table);
    }
}
