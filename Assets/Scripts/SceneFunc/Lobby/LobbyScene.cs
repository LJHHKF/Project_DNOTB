using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private Animator lobbyAnim;
    [SerializeField] private float animPlayTime = 1.0f;

    public void Start()
    {
        CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
        lobbyAnim.enabled = false;
    }

    public void ToMainGame()
    {
        lobbyAnim.enabled = true;
        StartCoroutine(DelayedLoadMainGameScene());
    }

    public void ToEndingList()
    {
        MySceneManager.instance.LoadScene(MySceneName.SceneName.EndingListPage);
    }

    public void ToExit()
    {
        MySceneManager.instance.LoadScene(MySceneName.SceneName.GameEnd);
    }

    private IEnumerator DelayedLoadMainGameScene()
    {
        yield return new WaitForSeconds(animPlayTime);
        MySceneManager.instance.LoadScene(MySceneName.SceneName.MainGame);
        yield break;
    }
}
