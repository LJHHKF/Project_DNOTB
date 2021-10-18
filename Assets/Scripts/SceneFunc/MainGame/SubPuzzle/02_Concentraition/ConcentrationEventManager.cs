using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentrationEventManager : MonoBehaviour
{
    [Header("Object Link")]
    [SerializeField] private ConcentrationCard[] cards;

    //Match Set Setting 시작
    private bool m_firstSet = false;
    public bool firstSet
    {
        get { return m_firstSet; }
        set
        {
            m_firstSet = value;
            if (m_fifthSet == true)
                ChkConcentrationEnd();
        }
    }
    [Header("Match Set Setting")] // 인스펙터 뷰에 제대로 나오지 않아서 이 위치로 옮김.
    [SerializeField] private int firstSetValue;

    private bool m_secondSet = false;
    public bool secondSet
    {
        get { return m_secondSet; }
        set
        {
            m_secondSet = value;
            if (m_secondSet == true)
                ChkConcentrationEnd();
        }
    }
    [SerializeField] private int secondSetValue;

    private bool m_thirdSet = false;
    public bool thirdSet
    {
        get { return m_thirdSet; }
        set 
        {
            m_thirdSet = value;
            if (m_thirdSet == true)
                ChkConcentrationEnd();
        }
    }
    [SerializeField] private int thirdSetValue;

    private bool m_fourthSet = false;
    public bool fourthSet
    {
        get { return m_fourthSet; }
        set
        {
            m_fourthSet = value;
            if (m_fourthSet == true)
                ChkConcentrationEnd();
        }
    }
    [SerializeField] private int fourthSetValue;

    private bool m_fifthSet = false;
    public bool fifthSet
    {
        get { return m_fifthSet; }
        set 
        {
            m_fifthSet = value;
            if (m_fifthSet == true)
                ChkConcentrationEnd();
        }
    }
    [SerializeField] private int fifthSetValue;

    private bool m_isFirstSelected = false;
    public bool isFirstSelected
    {
        get {return m_isFirstSelected; }
        set { m_isFirstSelected = value; }
    }
    private bool m_isSecondFliping = false;
    public bool isSecondFliping
    {
        get { return m_isSecondFliping; }
        set { m_isSecondFliping = value; }
    }

    [Header("Other")]
    [SerializeField] private float m_CardFlipTime;
    public float cardFlipTime { get { return m_CardFlipTime; } }

    private ConcentrationCard[] selectedCards = new ConcentrationCard[2];
    private int[] valueArray = new int[5];
  
    private List<int> list_SettedOrder = new List<int>();
    private readonly int firstValueSetted_order = 1;
    private readonly int secondValueSetted_order = 2;
    private readonly int thirdValueSetted_order = 3;
    private readonly int fourthValueSetted_order = 4;
    private readonly int fifthValueSetted_order = 5;

    private int[] array_SettedCount = new int[5];

    private void Start()
    {
        valueArray[0] = firstSetValue;
        valueArray[1] = secondSetValue;
        valueArray[2] = thirdSetValue;
        valueArray[3] = fourthSetValue;
        valueArray[4] = fifthSetValue;


        EventManager.instance.ev_Reset += ResetEvent;
        ResetEvent();
    }

    private void OnEnable()
    {
        HideCards();
    }

    private void OnDestroy()
    {
        EventManager.instance.ev_Reset -= ResetEvent;
    }

    private void ResetEvent()
    {
        SetValues();
        HideCards();
    }

    private void HideCards()
    {
        m_firstSet = false;
        m_secondSet = false;
        m_thirdSet = false;
        m_fourthSet = false;
        m_fifthSet = false;
        SelectArrayClear();
        for (int i = 0; i < cards.Length; i++)
        {
            int _i = i;
            cards[_i].ResetCardToBackFace();
        }
    }

    private void SelectArrayClear()
    {
        for (int i = 0; i < selectedCards.Length; i++)
        {
            int _i = i;
            selectedCards[_i] = null;
        }

        m_isFirstSelected = false;
        m_isSecondFliping = false;
    }

    private void ResetListSetted()
    {
        for (int i = 0; i < array_SettedCount.Length; i++)
        {
            int _i = i;
            array_SettedCount[_i] = 0;
        }

        list_SettedOrder.Clear();
        list_SettedOrder.Add(firstValueSetted_order);
        list_SettedOrder.Add(secondValueSetted_order);
        list_SettedOrder.Add(thirdValueSetted_order);
        list_SettedOrder.Add(fourthValueSetted_order);
        list_SettedOrder.Add(fifthValueSetted_order);
    }

    private void DeleteIntList_byValue(ref List<int> _list, int _value)
    {
        if (_list.Count < 1)
            return;

        for(int i = 0; i < _list.Count; i++)
        {
            int _i = i;
            if(_list[_i] == _value)
            {
                _list.RemoveAt(_i);
                return;
            }
        }
    }

    private void SetValues()
    {
        int rand = 0;
        ResetListSetted();
        for (int i = 0; i < cards.Length; i++) // 카드 길이는 10으로 상정되어 있음.
        {
            int _i = i;
            rand = UnityEngine.Random.Range(0, list_SettedOrder.Count);

            switch(list_SettedOrder[rand])
            {
                case 1:
                    cards[_i].curValue = firstSetValue;
                    if (++array_SettedCount[0] >= 2)
                        DeleteIntList_byValue(ref list_SettedOrder, 1);
                    break;
                case 2:
                    cards[_i].curValue = secondSetValue;
                    if (++array_SettedCount[1] >= 2)
                        DeleteIntList_byValue(ref list_SettedOrder, 2);
                    break;
                case 3:
                    cards[_i].curValue = thirdSetValue;
                    if (++array_SettedCount[2] >= 2)
                        DeleteIntList_byValue(ref list_SettedOrder, 3);
                    break;
                case 4:
                    cards[_i].curValue = fourthSetValue;
                    if (++array_SettedCount[3] >= 2)
                        DeleteIntList_byValue(ref list_SettedOrder, 4);
                    break;
                case 5:
                        cards[_i].curValue = fifthSetValue;
                    if (++array_SettedCount[4] >= 2)
                        DeleteIntList_byValue(ref list_SettedOrder, 5);
                    break;
            }
        }
    }

    public void CardSelect(ConcentrationCard _selected)
    {
        if(!isFirstSelected)
        {
            isFirstSelected = true;
            selectedCards[0] = _selected;
            return;
        }
        else if(!isSecondFliping)
        {
            isSecondFliping = true;
            selectedCards[1] = _selected;
            StartCoroutine(UnSetSecond());
        }
    }
    private IEnumerator UnSetSecond()
    {
        yield return new WaitForSeconds(cardFlipTime);
        ChkSelectedCards();
        yield break;
    }

    private void ChkSelectedCards()
    {
        if(selectedCards[0].curValue == selectedCards[1].curValue)
        {
            int _cnt = -1;
            for(int i = 0; i < valueArray.Length; i++)
            {
                int _i = i;
                if(selectedCards[0].curValue == valueArray[i])
                {
                    _cnt = _i;
                    break;
                }
            }
            if (_cnt > -1)
            {
                switch (_cnt)
                {
                    case 0:
                        firstSet = true;
                        break;
                    case 1:
                        secondSet = true;
                        break;
                    case 2:
                        thirdSet = true;
                        break;
                    case 3:
                        fourthSet = true;
                        break;
                    case 4:
                        fifthSet = true;
                        break;
                }
                for (int i = 0; i < selectedCards.Length; i++)
                    selectedCards[i].Matched();
            }
            else
            {
                Debug.LogError("설정된 카드 값 체크 중 오류 발생.");
                for (int i = 0; i < selectedCards.Length; i++)
                    selectedCards[i].UnMatched();
            }
        }
        else
        {
            for (int i = 0; i < selectedCards.Length; i++)
                selectedCards[i].UnMatched();
        }
        SelectArrayClear();
    }

    private void ChkConcentrationEnd()
    {
        if(m_fifthSet == true &&
           m_secondSet == true &&
           m_thirdSet == true &&
           m_fourthSet == true &&
           m_fifthSet == true)
        {
            SubPuzzleManager.instance.isConcentrationClear = true;
        }
    }
}
