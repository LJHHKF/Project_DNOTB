using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PortalCol : MonoBehaviour, IEventObject
{
    [Serializable]
    private enum PortalColor
    {
        Blue,
        Orenge
    }
    [SerializeField] private PortalColor m_color;

    private int cnt_clicked = 0;
    private float clickedTime = 0.0f;

    private void OnEnable()
    {
        ResetEvent();
        MainEventManager.instance.ev_Reset += ResetEvent;
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    private void OnDisable()
    {
        MainEventManager.instance.ev_Reset -= ResetEvent;
    }

    private void ResetEvent()
    {
        cnt_clicked = 0;
        clickedTime = 0.0f;
    }

    private void Update()
    {
        if (clickedTime > 0.0f)
            clickedTime -= Time.deltaTime;
        else
            cnt_clicked = 0;
    }

    public void Execute()
    {
        if (CursorManager.instnace.GetCurrentCursorType() == MyCursor.CursorType.Magnifier)
        {
            switch (m_color)
            {
                case PortalColor.Blue:
                    MagnifierManager.instance.SetInfoText(MyInfoText.Types.PortalBlue);
                    break;
                case PortalColor.Orenge:
                    MagnifierManager.instance.SetInfoText(MyInfoText.Types.PortalOrenge);
                    break;
            }
        }
        else
        {
            cnt_clicked++;
            clickedTime = 1.0f;
            if (cnt_clicked >= 2)
                BoxMain.instance.SetCubeIllumination(false);
        }
    }
}
