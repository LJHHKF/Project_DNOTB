using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : DetectOnMouseClick
{
    [Header("Knife Setting")]
    [SerializeField] private GameObject imageObject;

    protected override void Execute()
    {
        MyCursor.CursorType _type = CursorManager.instnace.GetCurrentCursorType();

        if (_type == MyCursor.CursorType.Normal)
        {
            CursorManager.instnace.MySetCursor(MyCursor.CursorType.Knife);
            EventManager.instance.hadKnife = true;
        }
        else if (_type == MyCursor.CursorType.Knife)
        {
            CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
            EventManager.instance.hadKnife = false;
        }
    }
}
