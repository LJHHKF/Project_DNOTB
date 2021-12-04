using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerCol : MonoBehaviour, IEventObject
{
    private int clickedCnt_withKnife = 0;
    private readonly int clickMax_withKnife = 5;
    private Animator m_anim;
    private SpriteRenderer m_sprR;
    [SerializeField] private float animPlayTime = 1.0f;
    [SerializeField] private Sprite spr_defualt;

    private void OnEnable()
    {
        if (m_anim == null)
            m_anim = GetComponent<Animator>();
        if (m_sprR == null)
            m_sprR = GetComponent<SpriteRenderer>();
        //m_anim.enabled = false;
        //m_sprR.sprite = spr_defualt;
        
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
        clickedCnt_withKnife = 0;

        StopAllCoroutines();
        m_anim.enabled = false;
        m_sprR.sprite = spr_defualt;
    }

    public void Execute()
    {
        if (!m_anim.enabled)
        {
            MyCursor.CursorType _type = CursorManager.instnace.GetCurrentCursorType();
            if (_type == MyCursor.CursorType.Magnifier)
                MagnifierManager.instance.SetInfoText(MyInfoText.Types.InvoiceCover);
            else if (_type == MyCursor.CursorType.Knife)
            {
                if (clickMax_withKnife <= ++clickedCnt_withKnife)
                    StartCoroutine(DelayedRemoveSticker());
            }
        }
    }
    private IEnumerator DelayedRemoveSticker()
    {
        m_anim.enabled = true;
        yield return new WaitForSeconds(animPlayTime);
        m_anim.enabled = false;
        BoxMain.instance.RemoveSticker();
        yield break;
    }
}
