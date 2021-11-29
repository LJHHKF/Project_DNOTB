using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchScreenManager : MonoBehaviour
{
    private static GlitchScreenManager m_instance;
    public static GlitchScreenManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GlitchScreenManager>();
            return m_instance;
        }
    }

    [Header("Common Setting")]
    [SerializeField] private GameObject glitchedScreen;
    [Header("ev_list Scene Setting")]
    [SerializeField] private GameObject normalScreen;
    private bool isEvListScene = false;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;

        if (!isEvListScene && MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= ResetEvent;
    }

    private void Start()
    {
        
        glitchedScreen.SetActive(false);
        if (MySceneManager.instance.GetCurrentSceneName() == MySceneName.SceneName.EndingListPage)
            isEvListScene = true;
        else
        {
            isEvListScene = false;
            MainEventManager.instance.ev_Reset += ResetEvent;
        }
    }

    private void ResetEvent()
    {
        glitchedScreen.SetActive(false);
        StopAllCoroutines();
    }

    public void GlitchOn(float _time)
    {
        StartCoroutine(DelayedOn(0.0f,_time));
    }

    public void DelayedGlitchOn(float _delay, float _time)
    {
        StartCoroutine(DelayedOn(_delay, _time));
    }

    private IEnumerator DelayedOn(float _delay, float _time)
    {
        yield return new WaitForSeconds(_delay);
        glitchedScreen.SetActive(true);
        if (isEvListScene)
            normalScreen.SetActive(false);
        yield return new WaitForSeconds(_time);
        glitchedScreen.SetActive(false);
        if (isEvListScene)
            normalScreen.SetActive(true);
        yield break;
    }
}
