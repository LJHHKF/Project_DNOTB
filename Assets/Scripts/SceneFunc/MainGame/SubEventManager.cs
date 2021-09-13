using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEventManager : MonoBehaviour
{
    [SerializeField] private GameObject[] unActiveObjects_ending;

    private void Start()
    {
        EventManager.instance.SetSubEvManager(gameObject);
    }

    public void UnActiveObjects_ending()
    {
        if (unActiveObjects_ending.Length > 0)
        {
            for (int i = 0; i < unActiveObjects_ending.Length; i++)
                unActiveObjects_ending[i].SetActive(false);
        }
    }

    public void ActiveObject_ending()
    {
        if (unActiveObjects_ending.Length > 0)
        {
            for (int i = 0; i < unActiveObjects_ending.Length; i++)
                unActiveObjects_ending[i].SetActive(true);
        }
    }
}
