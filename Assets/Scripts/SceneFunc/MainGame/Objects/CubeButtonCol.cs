using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeButtonCol : MonoBehaviour, IEventObject
{
    [SerializeField] private float animPlayTime = 1.0f;
    [SerializeField] private Sprite spr_ButtonBlcok;
    [SerializeField] private Sprite spr_ButtonOpen;
    private SpriteRenderer m_sprR;
    private Animator m_anim;


    private void Start()
    {
        m_sprR = GetComponent<SpriteRenderer>();
        m_anim = GetComponent<Animator>();
        ResetEvent();
        MainEventManager.instance.ev_Reset += ResetEvent;
    }

    private void ResetEvent()
    {
        m_sprR.sprite = spr_ButtonBlcok;
        m_anim.enabled = false;
    }

    private void OnDestroy()
    {
        MainEventManager.instance.ev_Reset -= ResetEvent;
    }

    public void Execute()
    {
        if (!BoxMain.instance.isCubeTape_Untaped)
        {
            BoxMain.instance.isCubeTape_Untaped = true;
            m_sprR.sprite = spr_ButtonOpen;
        }
        else if (!m_anim.enabled)
            StartCoroutine(DelayedEndingOn());
    }

    private IEnumerator DelayedEndingOn()
    {
        m_anim.enabled = true;
        yield return new WaitForSeconds(animPlayTime);
        m_anim.enabled = false;
        BoxMain.instance.DoUnBoxing(MyEndings.UnboxingType.fourth);
        yield break;
    }
}
