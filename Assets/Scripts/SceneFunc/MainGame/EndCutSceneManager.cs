using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class EndCutSceneManager : MonoBehaviour
{
    private static EndCutSceneManager m_instance;
    public static EndCutSceneManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<EndCutSceneManager>();
            return m_instance;
        }
    }

    [Header("Object")]
    [SerializeField] private GameObject cutScene_frame;
    private Image cutScene_img;
    [SerializeField] private GameObject p_dialogues;
    private GameObject[] obj_dialogues = new GameObject[3];
    private Text[] text_dialogues = new Text[3];
    private Image[] img_dialogues = new Image[3];
    private Image dialogues_BG;
    
    private float curAlpha = 0;

    [Header("Other Setting")]
    [SerializeField] private bool isListScene = false;

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

    // Start is called before the first frame update
    void Start()
    {
        cutScene_img = cutScene_frame.transform.Find("CutSceneImage").GetComponent<Image>();
        dialogues_BG = p_dialogues.GetComponent<Image>();
        StringBuilder name = new StringBuilder("DialogueN");
        for(int i = 0; i < obj_dialogues.Length; i++)
        {
            name.Remove(name.Length - 1, 1);
            name.Append(i + 1);
            obj_dialogues[i] = p_dialogues.transform.Find(name.ToString()).gameObject;
            img_dialogues[i] = obj_dialogues[i].GetComponent<Image>();
            text_dialogues[i] = obj_dialogues[i].transform.Find("Text").GetComponent<Text>();
            obj_dialogues[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventManager.instance.ev_Reset += OffCutScene;
        if (isListScene)
            ListSceneBoxMain.child_instance.ev_endingListReset += OffCutScene;
    }

    private void OnDisable()
    {
        EventManager.instance.ev_Reset -= OffCutScene;
        if (isListScene)
            ListSceneBoxMain.child_instance.ev_endingListReset -= OffCutScene;
    }

    private void OffCutScene()
    {
        StopAllCoroutines();

        curAlpha = 0;
        cutScene_frame.SetActive(false);

        for (int i = 0; i < obj_dialogues.Length; i++)
        {
            text_dialogues[i].text = null;
            obj_dialogues[i].SetActive(false);
        }
        p_dialogues.SetActive(false);
    }

    public void OnCutScene(MyEndings.EndingIndex _index)
    {
        switch(_index)
        {
            case MyEndings.EndingIndex.first:
                StartCoroutine(DelayedCutSceneOpen(3.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end01));
                break;
            case MyEndings.EndingIndex.second:
                StartCoroutine(DialoguesOn(_index));
                break;
            case MyEndings.EndingIndex.third:
                StartCoroutine(DelayedCutSceneOpen(3.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end03));
                break;
            case MyEndings.EndingIndex.fourth:
                StartCoroutine(DelayedCutSceneOpen(3.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end04));
                break;
        }
    }

    IEnumerator DelayedCutSceneOpen(float _startTime, float _endTime,Sprite _spr)
    {
        yield return new WaitForSeconds(_startTime);
        cutScene_frame.SetActive(true);
        cutScene_img.sprite = _spr;
        yield return new WaitForSeconds(_endTime);
        if (isListScene)
        {
            ListSceneBoxMain.child_instance.OnListSceneReset(); // OffCutScene을 포함한 이벤트.
            EndingListPageScene.instance.EndingRepeatWindowSetActive(false);
        }
        else
            OffCutScene();
        yield break;
    }

    IEnumerator DialoguesOn(MyEndings.EndingIndex _index)
    {
        if (_index == MyEndings.EndingIndex.second)
        {
            p_dialogues.SetActive(true);
            while (curAlpha < EndCutSceneDataManager.instance.prop_fadeOut_max_alpha)
            {
                if (EndCutSceneDataManager.instance.prop_fadeOut_max_second != 0)
                    curAlpha += (Time.deltaTime / EndCutSceneDataManager.instance.prop_fadeOut_max_second) * EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;
                else
                    curAlpha = EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;

                dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
                yield return null;
            }
            bool isEnd = false;
            int cnt = 0;
            while(!isEnd)
            {
                if(cnt < obj_dialogues.Length)
                {
                    obj_dialogues[cnt].SetActive(true);
                }
                switch(_index)
                {
                    case MyEndings.EndingIndex.second:
                        if (cnt >= EndCutSceneDataManager.instance.prop_dialogue_end02.Length)
                        {
                            isEnd = true;
                            for (int i = 0; i < obj_dialogues.Length; i++)
                            {
                                text_dialogues[i].text = null;
                                obj_dialogues[i].SetActive(false);
                            }
                            while(curAlpha > 0)
                            {
                                if (EndCutSceneDataManager.instance.prop_fadeOut_max_alpha != 0)
                                    curAlpha -= (Time.deltaTime / EndCutSceneDataManager.instance.prop_fadeOut_max_second) * EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;
                                else
                                    curAlpha = 0;

                                dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
                                yield return null;
                            }
                            p_dialogues.SetActive(false);
                            StartCoroutine(DelayedCutSceneOpen(0.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end02));
                            break;
                        }
                        else if (cnt == 0)
                        {
                            img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
                            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].dialogue;
                        }
                        else if (cnt == 1)
                        {
                            img_dialogues[1].sprite = img_dialogues[0].sprite;
                            text_dialogues[1].text = text_dialogues[0].text;
                            img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
                            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].dialogue;
                        }
                        else if (cnt >= 2)
                        {
                            img_dialogues[2].sprite = img_dialogues[1].sprite;
                            text_dialogues[2].text = text_dialogues[1].text;
                            img_dialogues[1].sprite = img_dialogues[0].sprite;
                            text_dialogues[1].text = text_dialogues[0].text;
                            img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
                            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].dialogue;
                        }
                    break;
                }
                cnt++;
                yield return new WaitForSeconds(EndCutSceneDataManager.instance.prop_dialogueDelay);
            }
        }
        yield break;
    }
}
