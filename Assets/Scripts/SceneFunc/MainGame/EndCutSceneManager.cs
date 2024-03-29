using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MyEndings
{
    public enum UnboxingType
    {
        first,
        third_1,
        third_2,
        fourth,
        fifth
    }

    public enum EndingIndex
    {
        first = 0,
        second, //1
        third_1, //2
        third_2, //3
        fourth,  //4
        fifth, //5
        temp_1, //6
        temp_2 //7
    }
}

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
    [SerializeField] private Collider2D boxingTapeCollider;
    [SerializeField] private GameObject cutScene_BG;
    [SerializeField] private GameObject cutScene_frame;
    private Image cutScene_img;
    private Image cutScene_BG_img;
    [SerializeField] private GameObject p_dialogues;
    private GameObject[] obj_dialogues = new GameObject[3];
    private Text[] text_dialogues = new Text[3];
    private Text[] text_head = new Text[3];
    private Image[] img_dialogues = new Image[3];
    private Image dialogues_BG;
    private float curAlpha = 0;

    private bool m_isMagnifierSet = false;
    public bool isMagnifierSet
    {
        get { return m_isMagnifierSet; }
        set
        {
            m_isMagnifierSet = value;
            if (value == true)
            {
                p_dialogues.SetActive(true);
                dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                for(int i = 0; i < obj_dialogues.Length; i++)
                {
                    if (i > 0)
                        obj_dialogues[i].SetActive(false);
                    else
                        obj_dialogues[i].SetActive(true);
                }
            }
            else
            {
                dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                for (int i = 0; i < obj_dialogues.Length; i++)
                    obj_dialogues[i].SetActive(false);
                p_dialogues.SetActive(false);
            }
        }
    }

    [Header("Other Setting")]
    private bool m_isEndingOn = false;
    public bool isEndingOn { get { return m_isEndingOn; } private set { m_isEndingOn = value; } }

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
        cutScene_BG_img = cutScene_BG.GetComponent<Image>();
        dialogues_BG = p_dialogues.GetComponent<Image>();
        StringBuilder name = new StringBuilder("DialogueN");
        for(int i = 0; i < obj_dialogues.Length; i++)
        {
            name.Remove(name.Length - 1, 1);
            name.Append(i + 1);
            obj_dialogues[i] = p_dialogues.transform.Find(name.ToString()).gameObject;
            img_dialogues[i] = obj_dialogues[i].GetComponent<Image>();
            text_dialogues[i] = obj_dialogues[i].transform.Find("Text").GetComponent<Text>();
            text_head[i] = obj_dialogues[i].transform.Find("Text_Head").GetComponent<Text>();
            obj_dialogues[i].SetActive(false);
        }
        OffCutScene();
        m_isMagnifierSet = false;
    }

    private void OnEnable()
    {
        MySceneName.SceneName _name = MySceneManager.instance.GetCurrentSceneName();
        if(_name == MySceneName.SceneName.MainGame)
            MainEventManager.instance.ev_Reset += OffCutScene;
        else if (_name == MySceneName.SceneName.EndingListPage)
            ListSceneBoxMain.instance.ev_endingListReset += OffCutScene;
    }

    private void OnDisable()
    {
        MySceneName.SceneName _name = MySceneManager.instance.GetCurrentSceneName();
        if (_name == MySceneName.SceneName.MainGame && MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= OffCutScene;
        else if (_name == MySceneName.SceneName.EndingListPage && ListSceneBoxMain.instance != null)
            ListSceneBoxMain.instance.ev_endingListReset -= OffCutScene;
    }

    //ev_Reset에 등록되어 있으므로 OnReset 기능을 포함시킬 수 있음.
    private void OffCutScene()
    {
        StopAllCoroutines();
        isEndingOn = false; // OnReset 것.

        curAlpha = 0;
        //dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        //cutScene_BG_img.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        cutScene_frame.SetActive(false);
        cutScene_BG.SetActive(false);

        for (int i = 0; i < obj_dialogues.Length; i++)
        {
            text_dialogues[i].text = null;
            obj_dialogues[i].SetActive(false);
        }
        p_dialogues.SetActive(false);
    }

    public void OnCutScene(MyEndings.EndingIndex _index)
    {
        CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
        switch(_index)
        {
            case MyEndings.EndingIndex.first:
                //StartCoroutine(DelayedCutSceneOpen(3.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end01));
                SoundManager.instance.SetBG(MySound.MyBGs.End01);
                StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end01));
                break;
            case MyEndings.EndingIndex.second:
                SoundManager.instance.SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.Bell);
                StartCoroutine(DialoguesOn(_index));
                break;
            case MyEndings.EndingIndex.third_1:
            case MyEndings.EndingIndex.third_2:
                StartCoroutine(DialoguesOn(_index));
                break;
            case MyEndings.EndingIndex.fourth:
                //StartCoroutine(DelayedCutSceneOpen(3.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end04));
                //StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end04));
                StartCoroutine(DialoguesOn(_index));
                break;
            case MyEndings.EndingIndex.fifth:
                StartCoroutine(DialoguesOn(_index));
                break;
        }
    }

    private IEnumerator DelayedCutSceneOpen(float _startTime, float _endTime,Sprite _spr)
    {
        isEndingOn = true;
        yield return new WaitForSeconds(_startTime);
        cutScene_frame.SetActive(true);
        cutScene_img.sprite = _spr;
        yield return new WaitForSeconds(_endTime);
        if (MySceneManager.instance.GetCurrentSceneName() == MySceneName.SceneName.EndingListPage)
        {
            SoundManager.instance.SetBG(MySound.MyBGs.Lobby);
            ListSceneBoxMain.instance.OnListSceneReset(); // OffCutScene을 포함한 이벤트.
            EndingListPageScene.instance.EndingRepeatWindowSetActive(false);
        }
        else
            OffCutScene();
        yield break;
    }

    private IEnumerator CutSceneOpen(float _endTime,MyEndings.EndingIndex _index ,Sprite _spr)
    {
        if (_index == MyEndings.EndingIndex.first && MySceneManager.instance.GetCurrentSceneName() == MySceneName.SceneName.MainGame)
            boxingTapeCollider.enabled = false;

        isEndingOn = true;
        cutScene_BG.SetActive(true);
        cutScene_frame.SetActive(false);
        if(curAlpha >= EndCutSceneDataManager.instance.prop_fadeOut_max_alpha)
            cutScene_BG_img.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
        else
        {
            curAlpha = 0.0f;
            cutScene_BG_img.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            while (curAlpha < EndCutSceneDataManager.instance.prop_fadeOut_max_alpha)
            {
                if (EndCutSceneDataManager.instance.prop_fadeOut_max_second > 0)
                    curAlpha += (Time.deltaTime / EndCutSceneDataManager.instance.prop_fadeOut_max_second) * EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;
                else
                    curAlpha = EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;

                cutScene_BG_img.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
                yield return null;
            }
        }
        if(_index == MyEndings.EndingIndex.first)
            SoundManager.instance.SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.Alarm02);
        cutScene_frame.SetActive(true);
        cutScene_img.sprite = _spr;
        yield return new WaitForSeconds(_endTime);
        if (_index == MyEndings.EndingIndex.first)
        {
            cutScene_frame.SetActive(false);
            while (curAlpha < 1.0f)
            {
                if (EndCutSceneDataManager.instance.prop_fadeOut_max_second > 0)
                    curAlpha += (Time.deltaTime / EndCutSceneDataManager.instance.prop_fadeOut_max_second) * (1.0f - EndCutSceneDataManager.instance.prop_fadeOut_max_alpha);
                else
                    curAlpha = 1.0f;

                cutScene_BG_img.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
                yield return null;
            }
            //사운드 재생
            cutScene_BG_img.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            yield return new WaitForSeconds(1.0f);
            if (MySceneManager.instance.GetCurrentSceneName() == MySceneName.SceneName.EndingListPage)
            {
                ListSceneBoxMain.instance.OnListSceneReset(); // OffCutScene을 포함한 이벤트.
                EndingListPageScene.instance.EndingRepeatWindowSetActive(false);
            }
            else
            {
                MySceneManager.instance.isSpecialLobbyReturn = true;
                MySceneManager.instance.LoadScene(MySceneName.SceneName.Lobby);
            }
        }
        else
        {
            cutScene_frame.SetActive(false);
            if (_index == MyEndings.EndingIndex.third_2)
                GlitchScreenManager.instance.GlitchOn(1.0f);
            while (curAlpha > 0)
            {
                if (EndCutSceneDataManager.instance.prop_fadeOut_max_alpha != 0)
                    curAlpha -= (Time.deltaTime / EndCutSceneDataManager.instance.prop_fadeOut_max_second) * EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;
                else
                    curAlpha = 0;

                cutScene_BG_img.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
                yield return null;
            }
            if (MySceneManager.instance.GetCurrentSceneName() == MySceneName.SceneName.EndingListPage)
            {
                SoundManager.instance.SetBG(MySound.MyBGs.Lobby);
                ListSceneBoxMain.instance.OnListSceneReset(); // OffCutScene을 포함한 이벤트.
                EndingListPageScene.instance.EndingRepeatWindowSetActive(false);
            }
            else
                OffCutScene();
        }
    }

    private IEnumerator DialoguesOn(MyEndings.EndingIndex _index)
    {
        if(isMagnifierSet)
        {
            isMagnifierSet = false;
            MagnifierManager.instance.StopInfoCoroutine();
        }
        isEndingOn = true;
        if (_index == MyEndings.EndingIndex.second || _index == MyEndings.EndingIndex.third_1 || _index == MyEndings.EndingIndex.third_2 || _index == MyEndings.EndingIndex.fourth || _index == MyEndings.EndingIndex.fifth)
        {
            MyDialogue.DialogueStruct[] array_dialogue = null;
            switch(_index)
            {
                case MyEndings.EndingIndex.second:
                    array_dialogue = EndCutSceneDataManager.instance.prop_dialogue_end02;
                    break;
                case MyEndings.EndingIndex.third_1:
                    array_dialogue = EndCutSceneDataManager.instance.prop_dialogue_end03_1;
                    break;
                case MyEndings.EndingIndex.third_2:
                    array_dialogue = EndCutSceneDataManager.instance.prop_dialogue_end03_2;
                    break;
                case MyEndings.EndingIndex.fourth:
                    array_dialogue = EndCutSceneDataManager.instance.prop_dialogue_end04;
                    break;
                case MyEndings.EndingIndex.fifth:
                    array_dialogue = EndCutSceneDataManager.instance.prop_dialogue_end05;
                    break;
            }
            if(array_dialogue == null)
            {
                Debug.LogError("(EndCutSceneManager Coroutine)대화문을 가져오는 데 실패했습니다.");
                yield break;
            }
            p_dialogues.SetActive(true);
            dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            curAlpha = 0.0f;
            while (curAlpha < EndCutSceneDataManager.instance.prop_fadeOut_max_alpha)
            {
                if (EndCutSceneDataManager.instance.prop_fadeOut_max_second > 0)
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
                if (cnt >= array_dialogue.Length)
                {
                    yield return new WaitForSeconds(1.0f);
                    isEnd = true;
                    for (int i = 0; i < obj_dialogues.Length; i++)
                    {
                        text_dialogues[i].text = null;
                        text_head[i].text = null;
                        obj_dialogues[i].SetActive(false);
                    }
                    //while (curAlpha > 0)
                    //{
                    //    if (EndCutSceneDataManager.instance.prop_fadeOut_max_alpha != 0)
                    //        curAlpha -= (Time.deltaTime / EndCutSceneDataManager.instance.prop_fadeOut_max_second) * EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;
                    //    else
                    //        curAlpha = 0;

                    //    dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
                    //    yield return null;
                    //}
                    p_dialogues.SetActive(false);
                    //StartCoroutine(DelayedCutSceneOpen(0.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end02));

                    switch(_index)
                    {
                        case MyEndings.EndingIndex.second:
                            SoundManager.instance.SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.OpenDoor);
                            StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end02));
                            break;
                        case MyEndings.EndingIndex.third_1:
                            GlitchScreenManager.instance.GlitchOn(1.0f);
                            StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end03_1));
                            break;
                        case MyEndings.EndingIndex.third_2:
                            GlitchScreenManager.instance.GlitchOn(1.0f);
                            StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end03_2));
                            break;
                        case MyEndings.EndingIndex.fourth:
                            StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end04));
                            break;
                        case MyEndings.EndingIndex.fifth:
                            StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end05));
                            break;
                    }


                    break;
                }
                else if (cnt == 0)
                {
                    //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
                    text_head[0].text = array_dialogue[cnt].speaker;
                    text_dialogues[0].text = array_dialogue[cnt].dialogue;
                }
                else if (cnt == 1)
                {
                    //img_dialogues[1].sprite = img_dialogues[0].sprite;
                    text_head[1].text = text_head[0].text;
                    text_dialogues[1].text = text_dialogues[0].text;
                    //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
                    text_head[0].text = array_dialogue[cnt].speaker;
                    text_dialogues[0].text = array_dialogue[cnt].dialogue;
                }
                else if (cnt >= 2)
                {
                    //img_dialogues[2].sprite = img_dialogues[1].sprite;
                    text_head[2].text = text_head[1].text;
                    text_dialogues[2].text = text_dialogues[1].text;
                    //img_dialogues[1].sprite = img_dialogues[0].sprite;
                    text_head[1].text = text_head[0].text;
                    text_dialogues[1].text = text_dialogues[0].text;
                    //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
                    text_head[0].text = array_dialogue[cnt].speaker;
                    text_dialogues[0].text = array_dialogue[cnt].dialogue;
                }
                cnt++;
                yield return new WaitForSeconds(EndCutSceneDataManager.instance.prop_dialogueDelay);
            }
        }
        yield break;
    }

    public void MagnifierDialogueSet(string _specker,string _text)
    {
        if (m_isMagnifierSet == false)
            isMagnifierSet = true;

        text_head[0].text = _specker;
        text_dialogues[0].text = _text;
    }
}




//switch (_index)
//{
//    case MyEndings.EndingIndex.second:
//        if (cnt >= EndCutSceneDataManager.instance.prop_dialogue_end02.Length)
//        {
//            yield return new WaitForSeconds(1.0f);
//            isEnd = true;
//            for (int i = 0; i < obj_dialogues.Length; i++)
//            {
//                text_dialogues[i].text = null;
//                text_head[i].text = null;
//                obj_dialogues[i].SetActive(false);
//            }
//            //while (curAlpha > 0)
//            //{
//            //    if (EndCutSceneDataManager.instance.prop_fadeOut_max_alpha != 0)
//            //        curAlpha -= (Time.deltaTime / EndCutSceneDataManager.instance.prop_fadeOut_max_second) * EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;
//            //    else
//            //        curAlpha = 0;

//            //    dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
//            //    yield return null;
//            //}
//            p_dialogues.SetActive(false);
//            //StartCoroutine(DelayedCutSceneOpen(0.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end02));
//            SoundManager.instance.SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.OpenDoor);
//            StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end02));
//            break;
//        }
//        else if (cnt == 0)
//        {
//            //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
//            text_head[0].text = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].speaker;
//            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].dialogue;
//        }
//        else if (cnt == 1)
//        {
//            //img_dialogues[1].sprite = img_dialogues[0].sprite;
//            text_head[1].text = text_head[0].text;
//            text_dialogues[1].text = text_dialogues[0].text;
//            //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
//            text_head[0].text = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].speaker;
//            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].dialogue;
//        }
//        else if (cnt >= 2)
//        {
//            //img_dialogues[2].sprite = img_dialogues[1].sprite;
//            text_head[2].text = text_head[1].text;
//            text_dialogues[2].text = text_dialogues[1].text;
//            //img_dialogues[1].sprite = img_dialogues[0].sprite;
//            text_head[1].text = text_head[0].text;
//            text_dialogues[1].text = text_dialogues[0].text;
//            //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
//            text_head[0].text = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].speaker;
//            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end02[cnt].dialogue;
//        }
//        break;
//    case MyEndings.EndingIndex.third_1:
//        if (cnt >= EndCutSceneDataManager.instance.prop_dialogue_end03_1.Length)
//        {
//            yield return new WaitForSeconds(1.0f);
//            isEnd = true;
//            for (int i = 0; i < obj_dialogues.Length; i++)
//            {
//                text_dialogues[i].text = null;
//                text_head[i].text = null;
//                obj_dialogues[i].SetActive(false);
//            }
//            //while (curAlpha > 0)
//            //{
//            //    if (EndCutSceneDataManager.instance.prop_fadeOut_max_alpha != 0)
//            //        curAlpha -= (Time.deltaTime / EndCutSceneDataManager.instance.prop_fadeOut_max_second) * EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;
//            //    else
//            //        curAlpha = 0;

//            //    dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
//            //    yield return null;
//            //}
//            p_dialogues.SetActive(false);
//            //StartCoroutine(DelayedCutSceneOpen(0.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end03));
//            GlitchScreenManager.instance.GlitchOn(1.0f);
//            StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end03_1));
//            break;
//        }
//        else if (cnt == 0)
//        {
//            //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
//            text_head[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03_1[cnt].speaker;
//            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03_1[cnt].dialogue;
//        }
//        else if (cnt == 1)
//        {
//            //img_dialogues[1].sprite = img_dialogues[0].sprite;
//            text_head[1].text = text_head[0].text;
//            text_dialogues[1].text = text_dialogues[0].text;
//            //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
//            text_head[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03_1[cnt].speaker;
//            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03_1[cnt].dialogue;
//        }
//        else if (cnt >= 2)
//        {
//            //img_dialogues[2].sprite = img_dialogues[1].sprite;
//            text_head[2].text = text_head[1].text;
//            text_dialogues[2].text = text_dialogues[1].text;
//            //img_dialogues[1].sprite = img_dialogues[0].sprite;
//            text_head[1].text = text_head[0].text;
//            text_dialogues[1].text = text_dialogues[0].text;
//            //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
//            text_head[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03_1[cnt].speaker;
//            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03_1[cnt].dialogue;
//        }
//        break;
//    case MyEndings.EndingIndex.third_2:
//        if (cnt >= EndCutSceneDataManager.instance.prop_dialogue_end03_1.Length)
//        {
//            yield return new WaitForSeconds(1.0f);
//            isEnd = true;
//            for (int i = 0; i < obj_dialogues.Length; i++)
//            {
//                text_dialogues[i].text = null;
//                text_head[i].text = null;
//                obj_dialogues[i].SetActive(false);
//            }
//            //while (curAlpha > 0)
//            //{
//            //    if (EndCutSceneDataManager.instance.prop_fadeOut_max_alpha != 0)
//            //        curAlpha -= (Time.deltaTime / EndCutSceneDataManager.instance.prop_fadeOut_max_second) * EndCutSceneDataManager.instance.prop_fadeOut_max_alpha;
//            //    else
//            //        curAlpha = 0;

//            //    dialogues_BG.color = new Color(1.0f, 1.0f, 1.0f, curAlpha);
//            //    yield return null;
//            //}
//            p_dialogues.SetActive(false);
//            //StartCoroutine(DelayedCutSceneOpen(0.0f, 5.0f, EndCutSceneDataManager.instance.prop_cs_spr_end03));
//            GlitchScreenManager.instance.GlitchOn(1.0f);
//            StartCoroutine(CutSceneOpen(5.0f, _index, EndCutSceneDataManager.instance.prop_cs_spr_end03));
//            break;
//        }
//        else if (cnt == 0)
//        {
//            //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
//            text_head[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].speaker;
//            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].dialogue;
//        }
//        else if (cnt == 1)
//        {
//            //img_dialogues[1].sprite = img_dialogues[0].sprite;
//            text_head[1].text = text_head[0].text;
//            text_dialogues[1].text = text_dialogues[0].text;
//            //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
//            text_head[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].speaker;
//            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].dialogue;
//        }
//        else if (cnt >= 2)
//        {
//            //img_dialogues[2].sprite = img_dialogues[1].sprite;
//            text_head[2].text = text_head[1].text;
//            text_dialogues[2].text = text_dialogues[1].text;
//            //img_dialogues[1].sprite = img_dialogues[0].sprite;
//            text_head[1].text = text_head[0].text;
//            text_dialogues[1].text = text_dialogues[0].text;
//            //img_dialogues[0].sprite = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].isLeft ? EndCutSceneDataManager.instance.prop_dialogue_spr_left : EndCutSceneDataManager.instance.prop_dialogue_spr_right;
//            text_head[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].speaker;
//            text_dialogues[0].text = EndCutSceneDataManager.instance.prop_dialogue_end03[cnt].dialogue;
//        }
//        break;
//}