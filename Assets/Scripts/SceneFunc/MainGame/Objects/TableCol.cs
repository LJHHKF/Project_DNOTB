using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCol : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        MagnifierManager.instance.SetInfoText(MyInfoText.Types.Table);
    }
}
