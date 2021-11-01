using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrationCard : MonoBehaviour
{
    [Header("Object Link")]
    [SerializeField] private Text m_Text;

    private int m_curValue;
    public int curValue
    {
        get { return m_curValue; }
        set { m_curValue = value; }
    }
    private readonly string backString = "(µÞ¸é)";
    private readonly string flipString = "(È¸Àü)";
    private float cardFlipTime;

    // Start is called before the first frame update
    void Start()
    {
        cardFlipTime = ConcentrationEventManager.instance.cardFlipTime;
    }

    private void OnEnable()
    {
        m_Text.text = backString;
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
        m_Text.text = curValue.ToString();
    }

    public void ResetCardToBackFace()
    {
        m_Text.text = backString;
    }

    private IEnumerator FlipCard()
    {
        m_Text.text = flipString;
        yield return new WaitForSeconds(cardFlipTime);
        m_Text.text = curValue.ToString();
        yield break;
    }

    private IEnumerator UnSetFlipCard()
    {
        m_Text.text = flipString;
        yield return new WaitForSeconds(cardFlipTime);
        m_Text.text = backString;
        yield break;
    }
}
