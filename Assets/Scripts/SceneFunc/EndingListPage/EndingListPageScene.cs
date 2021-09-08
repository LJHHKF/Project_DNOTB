using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingListPageScene : MonoBehaviour
{
    public void ToLobby()
    {
        MySceneManager.instance.LoadScene(MySceneName.SceneName.Lobby);
    }
}
