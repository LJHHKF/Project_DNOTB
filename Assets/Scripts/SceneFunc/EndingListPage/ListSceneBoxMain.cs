using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ListSceneBoxMain : MonoBehaviour
{
    private static ListSceneBoxMain m_instance;
    public static ListSceneBoxMain child_instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<ListSceneBoxMain>();
            return m_instance;
        }
    }

    [Header("Resource Setting")]
    [SerializeField] private Sprite boxingSprite;
    [SerializeField] private Sprite untapingSprite;
    [SerializeField] private Sprite unboxingSprite;

    [Header("Objects linking")]
    [SerializeField] private GameObject object_ShadowLight;
    [SerializeField] private GameObject object_InBoxObject;
    private InBoxImageManager inBoxObjectImageManager;
    [SerializeField] private GameObject object_Invoice_Cover;
    [SerializeField] private GameObject object_Invoice;
    [SerializeField] private GameObject object_CubeButton;

    private SpriteRenderer m_sprR;

    [Header("Other Object linking(origin: Sub Event Manager)")]
    [SerializeField] private GameObject[] activeObjects_least_1;
    [SerializeField] private GameObject knifeObject;
    [SerializeField] private int sOrder_default;
    [SerializeField] private int sOrder_open;

    private int cnt_end;

    public event Action ev_endingListReset;

    private void Awake()
    {
        if (child_instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
    }

    private void Start()
    {
        //이하 2줄 base
        m_sprR = GetComponent<SpriteRenderer>();
        inBoxObjectImageManager = object_InBoxObject.GetComponent<InBoxImageManager>();

        cnt_end = EventManager.instance.CheckDoneEndingCount_all();
        OnReset();
    }

    private void OnEnable()
    {
        ev_endingListReset += OnReset;
    }

    private void OnDisable()
    {
        ev_endingListReset -= OnReset;
    }

    private void OnReset()
    {
        object_InBoxObject.SetActive(false);
        object_ShadowLight.SetActive(true);
        //object_ShadowLight_knife_full.SetActive(true);
        //object_ShadowLight_knife_down.SetActive(false);
        m_sprR.sprite = boxingSprite;
        m_sprR.sortingOrder = sOrder_default;
        if(cnt_end > 0)
        {
            for (int i = 0; i < activeObjects_least_1.Length; i++)
                activeObjects_least_1[i].SetActive(true);
        }
    }

    //이 함수는 각 엔딩 체크 버튼 오브젝트에서 참조하도록 해야함.
    public void SetEndingState(MyEndings.EndingIndex _index)
    {
        OnListSceneReset();
        EndCutSceneManager.instance.OnCutScene(_index);
        switch (_index)
        {
            case MyEndings.EndingIndex.first:
                UnBoxing();
                UnSetCube();
                inBoxObjectImageManager.SetEnding(MyEndings.UnboxingType.first);
                break;
            case MyEndings.EndingIndex.second:
                UnSetCube();
                //m_sprR.sprite = boxingSprite;
                break;
            case MyEndings.EndingIndex.third:
                UnBoxing();
                UnSetCube();
                knifeObject.SetActive(false);
                inBoxObjectImageManager.SetEnding(MyEndings.UnboxingType.third);
                break;
            case MyEndings.EndingIndex.fourth:
                UnBoxing();
                inBoxObjectImageManager.SetEnding(MyEndings.UnboxingType.fourth);
                break;
        }

        void UnBoxing()
        {
            m_sprR.sprite = unboxingSprite;
            m_sprR.sortingOrder = sOrder_open;
            object_ShadowLight.SetActive(false);
            //object_ShadowLight_knife_full.SetActive(false);
            //object_ShadowLight_knife_down.SetActive(true);
            object_InBoxObject.SetActive(true);
            object_Invoice_Cover.SetActive(false);
            object_Invoice.SetActive(false);
        }

        void UnSetCube()
        {
            object_CubeButton.SetActive(false);
        }
    }

    public void OnListSceneReset()
    {
        ev_endingListReset?.Invoke();
    }
}
