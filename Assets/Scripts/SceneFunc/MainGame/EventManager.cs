using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        m_instace = null;
    }

    private void Start()
    {
        MyEventReset();
    }

    public void MyEventReset()
    {
        Debug.Log("리셋 실행");
    }
}
