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

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    public void LoadScene(MySceneName.SceneName name)
    {
        switch (name)
        {
            case SceneName.GameEnd:
                Application.Quit();
                break;
            case SceneName.Lobby:
                SceneManager.LoadScene("Lobby");
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
}
