using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvoiceCol : MonoBehaviour, IEventObject
{
    public void Execute()
    {
        BoxMain.instance.DoUnBoxing(MyEndings.UnboxingType.third);
    }
}
