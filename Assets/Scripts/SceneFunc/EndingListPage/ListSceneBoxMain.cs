using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ListSceneBoxMain : BoxMain
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

    //[Header("Resource Setting")]
    //[SerializeField] protected Sprite boxingSprite;
    //[SerializeField] protected Sprite untapingSprite;
    //[SerializeField] protected Sprite unboxingSprite;
    //[SerializeField] protected Sprite end01_ObjectSprite;

    //[Header("Objects linking")]
    //[SerializeField] private GameObject object_Collider_Tape; // 직렬화를 해둬서 public 처럼 보임
    //[SerializeField] private GameObject object_BoxCol; // 직렬화를 해둬서 public 처럼 보임
    //[SerializeField] private GameObject object_Collider_underInvoiceTape; // 직렬화를 해둬서 public 처럼 보임
    //[SerializeField] protected GameObject object_InBoxObject;
    //protected SpriteRenderer inBoxObjectSpriteR;
    //[SerializeField] protected GameObject object_Invoice_Cover;
    //[SerializeField] protected GameObject object_Invoice;
    //[SerializeField] protected GameObject object_Cube;
    //[SerializeField] private GameObject object_Portal;  // 직렬화를 해둬서 public 처럼 보임
    //[SerializeField] protected GameObject object_CubeSticker;

    //protected SpriteRenderer m_sprR;

    [Header("Other Object linking(origin: Sub Event Manager)")]
    [SerializeField] private GameObject[] activeObjects_least_1;
    [SerializeField] private GameObject knifeObject;

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

    protected override void Start()
    {
        //이하 2줄 base
        //m_sprR = GetComponent<SpriteRenderer>();
        //inBoxObjectSpriteR = object_InBoxObject.GetComponent<SpriteRenderer>();

        base.Start();
        cnt_end = EventManager.instance.CheckDoneEndingCount_all();
        OnReset();
    }

    protected override void OnEnable()
    {
        ev_endingListReset += OnReset;
    }

    protected override void OnDisable()
    {
        ev_endingListReset -= OnReset;
    }

    private void OnReset()
    {
        object_InBoxObject.SetActive(false);
        m_sprR.sprite = boxingSprite;
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
                inBoxObjectSpriteR.sprite = null; // 아직 리소스 없음.
                break;
            case MyEndings.EndingIndex.second:
                UnSetCube();
                //m_sprR.sprite = boxingSprite;
                break;
            case MyEndings.EndingIndex.third:
                UnBoxing();
                UnSetCube();
                knifeObject.SetActive(false);
                inBoxObjectSpriteR.sprite = end03_ObjectSprite;
                break;
            case MyEndings.EndingIndex.fourth:
                UnBoxing();
                inBoxObjectSpriteR.sprite = null; // 아직 리소스 없음.
                break;
        }

        void UnBoxing()
        {
            m_sprR.sprite = unboxingSprite;
            object_InBoxObject.SetActive(true);
            object_Invoice_Cover.SetActive(false);
            object_Invoice.SetActive(false);
        }

        void UnSetCube()
        {
            object_Cube.SetActive(false);
            object_CubeSticker.SetActive(false);
            object_Portal.SetActive(false);
        }
    }

    public void OnListSceneReset()
    {
        ev_endingListReset?.Invoke();
    }
}
