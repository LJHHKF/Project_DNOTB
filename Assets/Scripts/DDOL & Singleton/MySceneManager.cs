using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySceneName;

namespace MySceneName
{
    public enum SceneName
    {
        GameEnd,
        Lobby,
        MainGame,
        EndingListPage
    }
}

public class MySceneManager : MonoBehaviour
{
    private static MySceneManager m_instance;
    public static MySceneManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<MySceneManager>();
            return m_instance;
        }
    }

    private MySceneName.SceneName curSceneName;
    private bool m_isSpecialLobbyReturn = false;
    public bool isSpecialLobbyReturn { get { return m_isSpecialLobbyReturn; } set { m_isSpecialLobbyReturn = value; } }

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

    public void LoadScene(MySceneName.SceneName _name)
    {
        curSceneName = _name;
        switch (_name)
        {
            case SceneName.GameEnd:
                Application.Quit();
                break;
            case SceneName.Lobby:
                SceneManager.LoadScene("Lobby");
                SoundManager.instance.UnSetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.Alarm01);
                break;
            case SceneName.MainGame:
                SceneManager.LoadScene("MainGame");
                break;
            case SceneName.EndingListPage:
                SceneManager.LoadScene("EndingListPage");
                break;
            default:
                Debug.LogError("잘못된 씬 이름 지정으로 씬을 넘어가지 못했습니다.");
                break;
        }
    }

    public MySceneName.SceneName GetCurrentSceneName()
    {
        return curSceneName;
    }
}
