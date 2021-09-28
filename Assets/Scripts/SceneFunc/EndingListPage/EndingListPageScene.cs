using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingListPageScene : MonoBehaviour
{
    private static EndingListPageScene m_instance;
    public static EndingListPageScene instance
    {   
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<EndingListPageScene>();
            return m_instance;
        }
    }

    [Header("Object linking")]
    [SerializeField] private GameObject endingRepeatWindow;

    [Header("end Image Set")]
    [SerializeField] private Sprite spr_end01;
    [SerializeField] private Sprite spr_end02;

    private void Start()
    {
        endingRepeatWindow.SetActive(false);
    }

    public void ToLobby()
    {
        MySceneManager.instance.LoadScene(MySceneName.SceneName.Lobby);
    }

    public void EndingRepeatWindowSetActive(bool _value)
    {
        endingRepeatWindow.SetActive(_value);
    }

    public void RepeatWindowOffBTN()
    {
        endingRepeatWindow.SetActive(false);
        ListSceneBoxMain.child_instance.OnListSceneReset();
    }

    public Sprite GetEndImageSprite(MyEndings.EndingIndex _index)
    {
        switch (_index)
        {
            case MyEndings.EndingIndex.first:
                return spr_end01;
            case MyEndings.EndingIndex.second:
                return spr_end02;
        }
        return null;
    }
}
