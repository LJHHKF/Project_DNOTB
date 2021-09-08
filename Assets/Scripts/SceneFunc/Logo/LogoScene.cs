using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySceneName;

public class LogoScene : MonoBehaviour
{
    [SerializeField] private float delayTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedLoadScene());
    }

    private IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSeconds(delayTime);
        MySceneManager.instance.LoadScene(SceneName.Lobby);
        yield break;
    }
}
