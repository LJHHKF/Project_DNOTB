using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DialLock : MonoBehaviour
{
    [Header("Component Link")]
    [SerializeField] private Text m_Text;

    public enum DialOrder
    {
        first,
        second,
        third,
        fourth
    }

    public enum DialValue
    {
        _0 = 0,
        _1 = 1,
        _2 = 2,
        _3 = 3,
        _4 = 4,
        _5 = 5,
        _6 = 6,
        _7 = 7,
        _8 = 8,
        _9 = 9
    }

    [Header("Set Dial")]
    [SerializeField] private DialOrder order;
    [SerializeField] private DialValue setValue;
    private DialValue curValue;
    private float rollTime;
    private bool isRolled = false;

    // Start is called before the first frame update
    void Start()
    {
        rollTime = DialLockEventManager.instance.prop_dialRollTime;
        SetRandomValue();
        EventManager.instance.ev_Reset += SetRandomValue;
    }

    private void OnEnable()
    {
        m_Text.text = ((int)curValue).ToString();
        isRolled = false;
        ChkDial();
    }

    private void OnDestroy()
    {
        EventManager.instance.ev_Reset -= SetRandomValue;
    }

    private void SetRandomValue()
    {
        int rand = UnityEngine.Random.Range(0, 9);
        curValue = (DialValue)rand;
        m_Text.text = rand.ToString(); // ((int)curValue).ToString();
        ChkDial();
    }

    public void UpBTN()
    {
        if (!isRolled)
        {
            int temp = (int)curValue;
            if (temp >= 9)
                temp = 0;
            else
                temp++;
            StartCoroutine(DialRoll(()=>curValue = (DialValue)temp));
        }
    }

    public void DownBTN()
    {
        
        if (!isRolled)
        {
            int temp = (int)curValue;
            if (temp <= 0)
                temp = 9;
            else
                temp--;
            StartCoroutine(DialRoll(()=>curValue = (DialValue)temp));
        }

    }

    private IEnumerator DialRoll(Action _act)
    {
        isRolled = true;
        m_Text.text = "(È¸Àü)";
        yield return new WaitForSeconds(rollTime);
        isRolled = false;
        _act.Invoke();
        m_Text.text = ((int)curValue).ToString();
        ChkDial();
        yield break;
    }

    private void ChkDial()
    {
        bool isEqual = false;
        if (curValue == setValue)
            isEqual = true;
        else
            isEqual = false;

        switch(order)
        {
            case DialOrder.first:
                DialLockEventManager.instance.dialFin_1 = isEqual;
                break;
            case DialOrder.second:
                DialLockEventManager.instance.dialFin_2 = isEqual;
                break;
            case DialOrder.third:
                DialLockEventManager.instance.dialFin_3 = isEqual;
                break;
            case DialOrder.fourth:
                DialLockEventManager.instance.dialFin_4 = isEqual;
                break;
        }
    }
}
