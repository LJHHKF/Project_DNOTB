using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrationCard : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Sprite backFaceImage;

    private int m_curValue;
    public int curValue
    {
        get { return m_curValue; }
        set { m_curValue = value; }
    }
    private Sprite m_curImage;
    public Sprite curImage { set { m_curImage = value; } }
    private float cardFlipTime;
    private Image m_Image;
    private Button m_Button;
    //private bool isMataced = false;

    // Start is called before the first frame update
    void Start()
    {
        cardFlipTime = ConcentrationEventManager.instance.cardFlipTime;
        EventManager.instance.ev_Reset += OnResetEvent;
        OnResetEvent();
    }

    private void OnDestroy()
    {
        EventManager.instance.ev_Reset -= OnResetEvent;
    }

    private void OnResetEvent()
    {
        ChkAndGetImage();
        ChkAndGetButton();
        m_Image.sprite = backFaceImage;
        m_Button.enabled = true;
        //isMataced = false;
    }

    private void ChkAndGetImage()
    {
        if (m_Image == null)
            m_Image = GetComponent<Image>();
    }

    private void ChkAndGetButton()
    {
        if (m_Button == null)
            m_Button = GetComponent<Button>();
    }

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
        ChkAndGetImage();
        ChkAndGetButton();
        m_Image.sprite = m_curImage;
        m_Button.enabled = false;
        //isMataced = true;
    }

    public void ResetCardToBackFace()
    {
        ChkAndGetImage();
        m_Image.sprite = backFaceImage;
    }

    private IEnumerator FlipCard()
    {
        ChkAndGetImage();
        //m_text.text = flipString;
        //yield return new WaitForSeconds(cardFlipTime);
        m_Image.sprite = m_curImage;
        yield break;
    }

    private IEnumerator UnSetFlipCard()
    {
        ChkAndGetImage();
        //m_Text.text = flipString;
        yield return new WaitForSeconds(cardFlipTime);
        m_Image.sprite = backFaceImage;
        yield break;
    }
}
