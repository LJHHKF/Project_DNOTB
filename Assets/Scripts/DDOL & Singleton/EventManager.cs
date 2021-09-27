using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    private static EventManager m_instance;
    public static EventManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<EventManager>();
            return m_instance;
        }
    }

    public event Action ev_Reset;
    //public event Action ev_EndingOpen;

    private SubEventManager m_subManager;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(m_instance == this)
            m_instance = null;
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

        CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);

        Debug.Log("리셋 실행");
    }

    public void OnEvent_EndingOpen()
    {
        //ev_EndingOpen?.Invoke();
        m_subManager?.UnActiveObjects_ending();
        DataRWManager.WriteData("DNOTB_save_event.csv", DataRWManager.instance.mySaveData_event);
        Debug.Log("엔딩 오픈, 저장 진행");
    }

    public bool CheckDoneEndingCount_each(int _min)
    {
        int hit = 0;
        bool result = false;
        foreach(KeyValuePair<string, int> items in DataRWManager.instance.mySaveData_event)
        {
            if (items.Value > 0) hit += 1;
            if (hit >= _min)
            {
                result = true;
                break;
            }
        }
        return result;
    }

    public int CheckDoneEndingCount_all()
    {
        int hit = 0;
        foreach (KeyValuePair<string, int> items in DataRWManager.instance.mySaveData_event)
        {
            if (items.Value > 0) hit += 1;
        }
        return hit;
    }
}
