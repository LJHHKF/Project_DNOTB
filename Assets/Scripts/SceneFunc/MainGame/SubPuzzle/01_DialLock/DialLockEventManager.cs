using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialLockEventManager : MonoBehaviour
{
    private static DialLockEventManager m_instance;
    public static DialLockEventManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<DialLockEventManager>();
            return m_instance;
        }
    }

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

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
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
