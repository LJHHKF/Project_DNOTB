using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private Animator lobbyAnim;
    [SerializeField] private float animPlayTime = 1.0f;
    [SerializeField] private GameObject blackOut;
    [SerializeField] private float maxFadeInTime;
    private Image blackOut_img;
    private bool isFadeOut = false;

    public void Start()
    {
        CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
        SoundManager.instance.SetBG(MySound.MyBGs.Lobby);
        lobbyAnim.enabled = false;
        if (MySceneManager.instance.isSpecialLobbyReturn)
        {
            blackOut.SetActive(true);
            blackOut_img = blackOut.GetComponent<Image>();
            MySceneManager.instance.isSpecialLobbyReturn = false;
            StartCoroutine(BlackOutGoing());
        }
        else
        {
            blackOut.SetActive(false);
            isFadeOut = false;
        }
     }

        public void ToMainGame()
    {
        if (!isFadeOut)
        {
            lobbyAnim.enabled = true;
            StartCoroutine(DelayedLoadMainGameScene());
        }
    }

    public void ToEndingList()
    {
        if (!isFadeOut)
        {
            MySceneManager.instance.LoadScene(MySceneName.SceneName.EndingListPage);
        }
    }

    public void ToExit()
    {
        if (!isFadeOut)
        {
            MySceneManager.instance.LoadScene(MySceneName.SceneName.GameEnd);
        }
    }

    private IEnumerator DelayedLoadMainGameScene()
    {
        if (!isFadeOut)
        {
            yield return new WaitForSeconds(animPlayTime);
            MySceneManager.instance.LoadScene(MySceneName.SceneName.MainGame);
            yield break;
        }
    }

    private IEnumerator BlackOutGoing()
    {
        isFadeOut = true;
        float curAlpha = 1.0f;
        blackOut_img.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        while (curAlpha > 0.01f)
        {
            if (maxFadeInTime > 0)
                curAlpha -= (Time.deltaTime / maxFadeInTime) * 1.0f;
            else
                curAlpha = 0.0f;

            blackOut_img.color = new Color(0.0f, 0.0f, 0.0f, curAlpha);
            yield return null;
        }
        isFadeOut = false;
        blackOut.SetActive(false);
        yield break;
    }
}
