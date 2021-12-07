using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    private List<GameObject> unActiveButtons = new List<GameObject>();

    //[Header("end Image Set")]
    //[SerializeField] private Sprite spr_end01;
    //[SerializeField] private Sprite spr_end02;
    //[SerializeField] private Sprite spr_end03;

    [Header("Other Setting")]
    [SerializeField] private int maxPage = 1;

    public event Action ev_pageChange;
    private int m_curPage = 0;
    public int curPage { get { return m_curPage; } private set { m_curPage = value; } }

    private void Start()
    {
        endingRepeatWindow.SetActive(false);
        curPage = 0;
        unActiveButtons.Capacity = 4;
    }

    public void ToLobby()
    {
        MySceneManager.instance.LoadScene(MySceneName.SceneName.Lobby);
    }

    public void EndingRepeatWindowSetActive(bool _value)
    {
        endingRepeatWindow.SetActive(_value);
        if (_value)
            SoundManager.instance.SetBG(MySound.MyBGs.MainGame);
        else
            SoundManager.instance.SetBG(MySound.MyBGs.Lobby);
    }

    public void RepeatWindowOffBTN()
    {
        endingRepeatWindow.SetActive(false);
        ListSceneBoxMain.instance.OnListSceneReset();
        SoundManager.instance.UnSetSoundEffects();
        SoundManager.instance.SetBG(MySound.MyBGs.Lobby);
    }

    public Sprite GetEndImageSprite(MyEndings.EndingIndex _index)
    {
        switch (_index)
        {
            case MyEndings.EndingIndex.first:
                return EndCutSceneDataManager.instance.prop_cs_spr_end01;
            case MyEndings.EndingIndex.second:
                return EndCutSceneDataManager.instance.prop_cs_spr_end02;
            case MyEndings.EndingIndex.third_1:
                return EndCutSceneDataManager.instance.prop_cs_spr_end03_1;
            case MyEndings.EndingIndex.third_2:
                return EndCutSceneDataManager.instance.prop_cs_spr_end03_2;
            case MyEndings.EndingIndex.fourth:
                return EndCutSceneDataManager.instance.prop_cs_spr_end04;
            case MyEndings.EndingIndex.fifth:
                return EndCutSceneDataManager.instance.prop_cs_spr_end05;
        }
        return null;
    }

    public void ChangeListPageBTN(bool _isNext)
    {
        bool pageChanged = false;
        if (_isNext)
        {
            if (curPage < maxPage)
            {
                curPage++;
                pageChanged = true;
            }
        }
        else
        {
            if (curPage > 0)
            {
                curPage--;
                pageChanged = true;
            }
        }
        if (pageChanged)
        {
            for (int i = 0; i < unActiveButtons.Count; i++)
                unActiveButtons[i].SetActive(true);
            unActiveButtons.Clear();

            ev_pageChange?.Invoke();
        }
    }

    public void UnActiveButton(GameObject _self)
    {
        unActiveButtons.Add(_self);
        _self.SetActive(false);
    }
}
