using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    public void ToMainGame()
    {
        MySceneManager.instance.LoadScene(MySceneName.SceneName.MainGame);
    }

    public void ToEndingList()
    {
        MySceneManager.instance.LoadScene(MySceneName.SceneName.EndingListPage);
    }

    public void ToExit()
    {
        MySceneManager.instance.LoadScene(MySceneName.SceneName.GameEnd);
    }
}
