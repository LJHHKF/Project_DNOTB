using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour, IEventObject
{
    [SerializeField] private GameObject imageObject;

    private void OnEnable()
    {
        imageObject.SetActive(true);
    }

    public void Execute()
    {
        MyCursor.CursorType _type = CursorManager.instnace.GetCurrentCursorType();

        if(_type == MyCursor.CursorType.Normal)
        {
            CursorManager.instnace.MySetCursor(MyCursor.CursorType.Knife);
            imageObject.SetActive(false);
        }
        else if(_type == MyCursor.CursorType.Knife)
        {
            CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
            imageObject.SetActive(true);
        }
    }
}
