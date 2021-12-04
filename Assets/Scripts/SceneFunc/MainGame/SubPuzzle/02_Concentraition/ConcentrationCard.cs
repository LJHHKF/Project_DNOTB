using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrationCard : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Sprite backFaceImage;

    [Header("Set Self Components")]
    [SerializeField]private Image m_Image;
    [SerializeField]private Button m_Button;

    private int m_curValue;
    public int curValue
    {
        get { return m_curValue; }
        set { m_curValue = value; }
    }
    private Sprite m_curImage;
    public Sprite curImage { set { m_curImage = value; } }
    private float cardFlipTime;
    private bool isEvSet = false;

    //private bool isMataced = false;

    // Start is called before the first frame update
    void Start()
    {
        cardFlipTime = ConcentrationEventManager.instance.cardFlipTime;
        MainEventManager.instance.ev_Reset += OnResetEvent;
        isEvSet = false;
        OnResetEvent();
    }

    private void OnDestroy()
    {
        if(MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= OnResetEvent;
        if(isEvSet && ConcentrationEventManager.instance != null)
        {
            ConcentrationEventManager.instance.ev_cardBtnOff -= SelfButtonOff;
            ConcentrationEventManager.instance.ev_cardBtnOn -= SelfButtonOn;
        }
    }

    private void OnResetEvent()
    {
        //ChkAndGetImage();
        //ChkAndGetButton();
        StopAllCoroutines();
        m_Image.sprite = backFaceImage;
        m_Button.enabled = true;
        if (!isEvSet)
        {
            ConcentrationEventManager.instance.ev_cardBtnOff += SelfButtonOff;
            ConcentrationEventManager.instance.ev_cardBtnOn += SelfButtonOn;
            isEvSet = true;
        }
        //isMataced = false;
    }

    //private void ChkAndGetImage()
    //{
    //    if (m_Image == null)
    //        m_Image = GetComponent<Image>();
    //}

    //private void ChkAndGetButton()
    //{
    //    if (m_Button == null)
    //        m_Button = GetComponent<Button>();
    //}

    public void CardClicked()
    {
        if(!ConcentrationEventManager.instance.isSecondFliping)
        {
            ConcentrationEventManager.instance.CardSelect(this);
            StartCoroutine(FlipCard());
        }
    }

    public void UnMatched()
    {
        StartCoroutine(UnSetFlipCard());
    }

    public void Matched()
    {
        //ChkAndGetImage();
        //ChkAndGetButton();
        m_Image.sprite = m_curImage;
        m_Button.enabled = false;
        isEvSet = false;
        if (isEvSet)
        {
            ConcentrationEventManager.instance.ev_cardBtnOff -= SelfButtonOff;
            ConcentrationEventManager.instance.ev_cardBtnOn -= SelfButtonOn;
        }
        //isMataced = true;
    }

    public void ResetCardToBackFace()
    {
        //ChkAndGetImage();
        m_Image.sprite = backFaceImage;
    }

    private IEnumerator FlipCard()
    {
        //ChkAndGetImage();
        //m_text.text = flipString;
        //yield return new WaitForSeconds(cardFlipTime);
        m_Image.sprite = m_curImage;
        m_Button.enabled = false;
        SoundManager.instance.SetSoundEffect_Overlap(MySound.MySoundEffects_Overlap.CardFlip);
        yield break;
    }

    private IEnumerator UnSetFlipCard()
    {
        //ChkAndGetImage();
        //m_Text.text = flipString;
        yield return new WaitForSeconds(cardFlipTime);
        m_Image.sprite = backFaceImage;
        //SoundManager.instance.SetSoundEffect_Overlap(MySound.MySoundEffects_Overlap.CardFlip);
        ConcentrationEventManager.instance.OnEvCardBtnOn();
        yield break;
    }

    private void SelfButtonOff()
    {
        m_Button.enabled = false;
    }

    private void SelfButtonOn()
    {
        m_Button.enabled = true;
    }
}
