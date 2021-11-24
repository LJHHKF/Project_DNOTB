using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainEventManager : MonoBehaviour
{
    private static MainEventManager m_instance;
    public static MainEventManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<MainEventManager>();
            return m_instance;
        }
    }

    [Header("Event Objects Setting")]
    //[SerializeField] private GameObject[] unActiveObjects_ending;
    [SerializeField] private GameObject[] activeObjects_least_1;
    private int endDepth = 0;

    public event Action ev_Reset;
    public event Action ev_Endleast_Set_1;
    public event Action ev_Endleast_UnSet_1;

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

    private void Start()
    {
        StartCoroutine(DelayedReset(0.01f));
    }

    //public void UnActiveObjects_ending()
    //{
    //    for (int i = 0; i < unActiveObjects_ending.Length; i++)
    //        unActiveObjects_ending[i].SetActive(false);
    //}

    public void ActiveObject_ending()
    {
        endDepth = DataRWManager.instance.CheckDoneEndingCount_all();
        if(endDepth > 0)
        {
            Active_least_1();
        }
        else
        {
            UnActive_least_1();
        }

        //for (int i = 0; i < unActiveObjects_ending.Length; i++)
        //{
        //    if (unActiveObjects_ending[i].transform.parent.gameObject.activeSelf)
        //        unActiveObjects_ending[i].SetActive(true);
        //}
    }

    private void UnActive_least_1()
    {
        ev_Endleast_UnSet_1?.Invoke();

        for (int i = 0; i < activeObjects_least_1.Length; i++)
            activeObjects_least_1[i].SetActive(false);   
    }

    private void Active_least_1()
    {
        ev_Endleast_Set_1?.Invoke();

        for (int i = 0; i < activeObjects_least_1.Length; i++)
            activeObjects_least_1[i].SetActive(true);
    }

    IEnumerator DelayedReset(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        MyEventReset();
        yield break;
    }

    public void MyEventReset()
    {
        ev_Reset?.Invoke();
        ActiveObject_ending();
    }

    public void OnEvent_EndingOpen()
    {
        //UnActiveObjects_ending();
        DataRWManager.WriteData("DNOTB_save_event.csv", DataRWManager.instance.mySaveData_event);
        Debug.Log("엔딩 세이브 진행");
    }
}
