using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    private static EventManager m_instace;
    public static EventManager instance
    {
        get
        {
            if (m_instace == null)
                m_instace = FindObjectOfType<EventManager>();
            return m_instace;
        }
    }

    public event Action ev_Reset;
    //public event Action ev_EndingOpen;

    private bool m_isEnding_01;
    public bool isEnding_01 { get { return m_isEnding_01; } set { m_isEnding_01 = value; } }
    private bool m_isEnding_02;
    public bool isEnding_02 { get { return m_isEnding_02; } set { m_isEnding_02 = value; } }

    private SubEventManager m_subManager;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        m_instace = null;
    }

    //private void Start()
    //{
    //    //MyEventReset();
    //}

    public void SetSubEvManager(GameObject _hadObject)
    {
        m_subManager = _hadObject.GetComponent<SubEventManager>();
    }

    public void MyEventReset()
    {
        m_subManager?.ActiveObject_ending();
        ev_Reset?.Invoke();

        isEnding_01 = false;
        isEnding_02 = false;

        Debug.Log("리셋 실행");
    }

    public void OnEvent_EndingOpen()
    {
        //ev_EndingOpen?.Invoke();
        m_subManager?.UnActiveObjects_ending();
        Debug.Log("엔딩 오픈");
    }
}
