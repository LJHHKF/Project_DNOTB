using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPuzzleManager : MonoBehaviour
{
    private static SubPuzzleManager m_instance;
    public static SubPuzzleManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<SubPuzzleManager>();
            return m_instance;
        }
    }

    [SerializeField] private Vector2 initPos_xy; // �۾� ���ǿ뵵. �۾� �� ��Ȱ��ȭ�صθ� ��ư�� ó���� �ι� ������ Ȱ��ȭ��.
    private bool m_isSubPuzzleOn = false;
    public bool isSubPuzzleOn
    {
        get { return m_isSubPuzzleOn; }
        private set { m_isSubPuzzleOn = value; }
    }

    [Header("Screen Border Setting")]
    [SerializeField] private Transform pos_LeftTop;
    private Vector2 m_pos_LeftTop;
    [SerializeField] private Transform pos_RightBottom;
    private Vector2 m_pos_RightBottom;
    private Camera cam_main;

    [Header("Puzzle Registration")]
    [SerializeField] private GameObject dialLockObjects;
    private bool m_isDialClear = false;
    public bool isDialClear
    {
        get { return m_isDialClear; }
        set {
            m_isDialClear = value;
            if (m_isDialClear == true)
                UnActiveDialLock();
            } 
    }
    [SerializeField] private GameObject concentrationObjects;
    private bool m_isConcentrationClear = false;
    public bool isConcentrationClear
    {
        get { return m_isConcentrationClear; }
        set
        {
            m_isConcentrationClear = value;
            if (m_isConcentrationClear == true)
                UnActiveConcentration();
        }
    }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (m_instance == this)
        {
            m_instance = null;
            EventManager.instance.ev_Reset -= ResetEvent;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = initPos_xy;

        cam_main = Camera.main;
        m_pos_LeftTop = cam_main.WorldToScreenPoint(pos_LeftTop.position);
        m_pos_RightBottom = cam_main.WorldToScreenPoint(pos_RightBottom.position);

        ResetEvent();
        EventManager.instance.ev_Reset += ResetEvent;
    }

    private void OnEnable()
    {
        isSubPuzzleOn = true;
    }

    private void OnDisable()
    {
        isSubPuzzleOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            Debug.Log($"mousePos:{mousePos}");
            Debug.Log($"LeftTopPos:{m_pos_LeftTop}");
            Debug.Log($"RightTopPos:{m_pos_RightBottom}");

            //���콺 �������� ��ũ�� ���� ���̶��
            if(mousePos.x < m_pos_LeftTop.x ||
               mousePos.x > m_pos_RightBottom.x ||
               mousePos.y < m_pos_RightBottom.y ||
               mousePos.y > m_pos_LeftTop.y)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void ResetEvent()
    {
        gameObject.SetActive(false);
        dialLockObjects.SetActive(false);
        concentrationObjects.SetActive(false);
        
        m_isDialClear = false;
        m_isConcentrationClear = false;
        m_isSubPuzzleOn = false;
    }

    private void OnWindow()
    {
        gameObject.SetActive(true);
    }

    private void OffWindow()
    {
        gameObject.SetActive(false);
    }

    public void ActiveDialLock()
    {
        if (!isDialClear)
        {
            OnWindow();
            dialLockObjects.SetActive(true);
        }
    }

    private void UnActiveDialLock()
    {
        dialLockObjects.SetActive(false);
        OffWindow();
    }

    public void ActiveConcentration()
    {
        if (!isConcentrationClear)
        {
            OnWindow();
            concentrationObjects.SetActive(true);
        }
    }

    private void UnActiveConcentration()
    {
        concentrationObjects.SetActive(false);
        OffWindow();
    }
}
