using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvoiceCol : MonoBehaviour, IEventObject
{
    private int clickCnt = 0;
    private readonly int cnt_GlitchOn = 2;
    private readonly int cnt_Max = 3;
    private void OnEnable()
    {
        CountReset();
        MainEventManager.instance.ev_Reset += CountReset;
    }

    private void OnDisable()
    {
        if(MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= CountReset;
    }

    private void CountReset()
    {
        clickCnt = 0;
    }

    public void Execute()
    {
        if (CursorManager.instnace.GetCurrentCursorType() == MyCursor.CursorType.Magnifier)
            MagnifierManager.instance.SetInfoText(MyInfoText.Types.Invoice);
        else
            clickCnt++;

        if (clickCnt == cnt_GlitchOn)
            GlitchScreenManager.instance.GlitchOn(1.0f);
        else if (clickCnt >= cnt_Max)
            BoxMain.instance.DoUnBoxing(MyEndings.UnboxingType.third_1);
    }
}
