using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeButtonCol : MonoBehaviour, IEventObject
{
    [SerializeField] private Sprite spr_ButtonBlcok;
    [SerializeField] private Sprite spr_ButtonOpen;
    private SpriteRenderer m_sprR;

    private void Start()
    {
        m_sprR = GetComponent<SpriteRenderer>();
        ResetEvent();
        EventManager.instance.ev_Reset += ResetEvent;
    }

    private void ResetEvent()
    {
        m_sprR.sprite = spr_ButtonBlcok;
    }

    private void OnDestroy()
    {
        EventManager.instance.ev_Reset -= ResetEvent;
    }

    public void Execute()
    {
        if (!BoxMain.instance.isCubeTape_Untaped)
        {
            BoxMain.instance.isCubeTape_Untaped = true;
            m_sprR.sprite = spr_ButtonOpen;
        }
        else
            BoxMain.instance.DoUnBoxing(MyEndings.UnboxingType.fourth);
    }
}
