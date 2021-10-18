using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialLockEventManager : MonoBehaviour
{
    [SerializeField] private float dialRollTime;
    public float prop_dialRollTime { get { return dialRollTime; } }

    private bool m_dialFin_1 = false;
    public bool dialFin_1 {
        get { return m_dialFin_1;}
        set {
            m_dialFin_1 = value;
            if(m_dialFin_1)
                ChkEnd();
            } 
    }
    private bool m_dialFin_2 = false;
    public bool dialFin_2 {
        get { return m_dialFin_2; }
        set { 
            m_dialFin_2 = value;
            if(m_dialFin_2)
                ChkEnd();
            } 
    }
    private bool m_dialFin_3 = false;
    public bool dialFin_3 {
        get { return m_dialFin_3; }
        set {
            m_dialFin_3 = value;
            if(m_dialFin_3)
                ChkEnd();
            }
    }
    private bool m_dialFin_4 = false;
    public bool dialFin_4 {
        get { return m_dialFin_4; }
        set {
            m_dialFin_4 = value;
            if(m_dialFin_4)
                ChkEnd();
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetEvent();
        EventManager.instance.ev_Reset += ResetEvent;
    }

    private void ResetEvent()
    {
        m_dialFin_1 = false;
        m_dialFin_2 = false;
        m_dialFin_3 = false;
        m_dialFin_4 = false;
    }

    private void ChkEnd()
    {
        if(m_dialFin_1 == true &&
           m_dialFin_2 == true &&
           m_dialFin_3 == true &&
           m_dialFin_4 == true)
        {
            SubPuzzleManager.instance.isDialClear = true;
        }
    }
}
