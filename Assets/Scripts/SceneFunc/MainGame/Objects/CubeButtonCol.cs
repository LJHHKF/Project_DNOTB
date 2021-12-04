using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeButtonCol : MonoBehaviour, IEventObject
{
    [SerializeField] private float animPlayTime = 1.0f;
    [SerializeField] private GameObject imgObject;
    [SerializeField] private Sprite spr_ButtonBlcok;
    [SerializeField] private Sprite spr_ButtonOpen;
    private SpriteRenderer m_sprR;
    private Animator m_anim;
    private Collider2D m_col;
    private bool isBlocked;
    private bool isFirstMagnifier = true;
    private int cnt_clicked = 0;

    private void Start()
    {
        m_sprR = imgObject.GetComponent<SpriteRenderer>();
        m_anim = imgObject.GetComponent<Animator>();
        m_col = GetComponent<Collider2D>();
        ResetEvent();
        MainEventManager.instance.ev_Reset += ResetEvent;
    }

    private void ResetEvent()
    {
        StopAllCoroutines();
        m_sprR.sprite = spr_ButtonBlcok;
        isBlocked = true;
        m_anim.enabled = false;
        isFirstMagnifier = true;
        cnt_clicked = 0;
        
    }

    private void OnDestroy()
    {
        if(MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= ResetEvent;
    }

    public void Execute()
    {
        if (CursorManager.instnace.GetCurrentCursorType() == MyCursor.CursorType.Magnifier)
        {
            if (isBlocked)
            {
                if (isFirstMagnifier)
                {
                    MagnifierManager.instance.SetInfoText(MyInfoText.Types.TableButtonCover_Init);
                    isFirstMagnifier = false;
                }
                else
                    MagnifierManager.instance.SetInfoText(MyInfoText.Types.TableButtonCover_Final);
            }
            else
            {
                if (isFirstMagnifier)
                {
                    MagnifierManager.instance.SetInfoText(MyInfoText.Types.TableButton_Init);
                    isFirstMagnifier = false;
                }
                else
                    MagnifierManager.instance.SetInfoText(MyInfoText.Types.TableButton_Final);
            }
        }
        else
        {
            if(isBlocked)
            {
                cnt_clicked++;
                if (!isFirstMagnifier)
                {
                    if (cnt_clicked >= 1)
                        CoverRemove();
                }
                else
                    if (cnt_clicked >= 3)
                        CoverRemove();
            }
            else if (BoxMain.instance.isCubeOut)
            {
                BoxMain.instance.DoUnBoxing(MyEndings.UnboxingType.fifth);
                SoundManager.instance.SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.SpaceShip);
            }
            else
            {
                if(cnt_clicked == 0)
                {
                    StartCoroutine(DelayedOnAct(Execute));
                    void Execute()
                    {
                        BoxMain.instance.CubeSetActive();
                        cnt_clicked++;
                    }
                }
                else if(cnt_clicked == 1)
                {
                    StartCoroutine(DelayedOnAct(Execute));
                    void Execute()
                    {
                        if (MagnifierManager.instance.isCubeInvest)
                            BoxMain.instance.PortalSetActive(true); // _isOrenge = true
                        else
                            BoxMain.instance.PortalSetActive(false);
                        cnt_clicked++;
                    }
                }
                else if(cnt_clicked == 2 && BoxMain.instance.isCubeIlluminated)
                {
                    StartCoroutine(DelayedOnAct(Execute));
                    void Execute()
                    {
                        BoxMain.instance.CubeMoveToPortal();
                        cnt_clicked++;
                    }
                }
                else if(cnt_clicked >= 3 && BoxMain.instance.isCubeSecondIlluminated)
                {
                    StartCoroutine(DelayedOnAct(Execute));
                    void Execute()
                    {
                        BoxMain.instance.DoUnBoxing(MyEndings.UnboxingType.fourth);
                    }
                }
            }
        }
    }

    //public void AnotherPortalSet()  // BoxMain¼­ °ü¸®ÇØµµ ÃæºÐÇÔÀ» ±ú´Ý°í »­.
    //{
    //}

    private void CoverRemove()
    {
        m_sprR.sprite = spr_ButtonOpen;
        isBlocked = false;
        isFirstMagnifier = true;
        cnt_clicked = 0;
    }

    private IEnumerator DelayedOnAct(Action _act)
    {
        imgObject.SetActive(false);
        imgObject.SetActive(true);
        m_anim.enabled = true;
        m_col.enabled = false;
        yield return new WaitForSeconds(animPlayTime);
        m_anim.enabled = false;
        m_col.enabled = true;
        _act.Invoke();
        yield break;
    }

    public int GetCntClicked()
    {
        return cnt_clicked; 
    }
}
