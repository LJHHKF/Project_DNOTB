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
    //[SerializeField] private GameObject object_Collider_Tape; // ����ȭ�� �صּ� public ó�� ����
    //[SerializeField] private GameObject object_BoxCol; // ����ȭ�� �صּ� public ó�� ����
    //[SerializeField] private GameObject object_Collider_underInvoiceTape; // ����ȭ�� �صּ� public ó�� ����
    //[SerializeField] protected GameObject object_InBoxObject;
    //protected SpriteRenderer inBoxObjectSpriteR;
    //[SerializeField] protected GameObject object_Invoice_Cover;
    //[SerializeField] protected GameObject object_Invoice;
    //[SerializeField] protected GameObject object_Cube;
    //[SerializeField] private GameObject object_Portal;  // ����ȭ�� �صּ� public ó�� ����
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
        //���� 2�� base
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

    //�� �Լ��� �� ���� üũ ��ư ������Ʈ���� �����ϵ��� �ؾ���.
    public void SetEndingState(MyEndings.EndingIndex _index)
    {
        OnListSceneReset();
        EndCutSceneManager.instance.OnCutScene(_index);
        switch (_index)
        {
            case MyEndings.EndingIndex.first:
                UnBoxing();
                UnSetCube();
                inBoxObjectSpriteR.sprite = null; // ���� ���ҽ� ����.
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
                inBoxObjectSpriteR.sprite = null; // ���� ���ҽ� ����.
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
