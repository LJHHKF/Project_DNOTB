using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour, IEventObject
{
    [SerializeField] private GameObject imageObject;
    //[SerializeField] private GameObject shadowLight_full;
    //[SerializeField] private GameObject shadowLight_down;

    private void OnEnable()
    {
        imageObject.SetActive(true);
        //shadowLight_full.SetActive(true);
        //shadowLight_down.SetActive(false);
        MainEventManager.instance.ev_Reset += BoxClosed;
        BoxMain.instance.ev_BoxOpend += BoxOpen;
    }

    public void Execute()
    {
        MyCursor.CursorType _type = CursorManager.instnace.GetCurrentCursorType();

        if(_type == MyCursor.CursorType.Normal)
        {
            CursorManager.instnace.MySetCursor(MyCursor.CursorType.Knife);
            imageObject.SetActive(false);
            //shadowLight_full.SetActive(false);
        }
        else if(_type == MyCursor.CursorType.Knife)
        {
            CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
            imageObject.SetActive(true);
            //shadowLight_full.SetActive(true);
        }
    }

    private void BoxClosed()
    {
        CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
        imageObject.SetActive(true);

        //shadowLight_full.SetActive(true);
        //shadowLight_down.SetActive(false);
    }

    private void BoxOpen()
    {
        if (imageObject.activeSelf)
        {
            //shadowLight_full.SetActive(false);
            //shadowLight_down.SetActive(true);
        }
        else
            CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
    }
}
