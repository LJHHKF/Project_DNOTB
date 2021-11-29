using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MySound
{
    public enum MyBGs
    {
        Null,
        Lobby,
        MainGame,
        End01
    }

    public enum MySoundEffects_NonOverlap
    {
        Alarm01 = 0,
        Alarm02,
        PaperKnife,
        Taping,
        TapeTearing,
        Bell,
        OpenDoor,
        SpaceShip,

        _max
    }

    public enum MySoundEffects_Overlap
    {
        Click = 0,
        Notification,
        CardFlip,

        _max
    }
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager m_instance;
    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<SoundManager>();
            return m_instance;
        }
    }

    [Header("BG Setting")]
    [SerializeField] private AudioClip gameLobby;
    [SerializeField] private AudioClip mainGame;
    [SerializeField] private AudioClip end01;
    private AudioSource m_audio;
    private MySound.MyBGs curBG;

    [Serializable] private struct SE_Clip_NonOverlap
    {
        public AudioClip clip;
        public float delay;
    }

    [Header("Sound Effect Setting_NonOverlap")]
    [SerializeField] private SE_Clip_NonOverlap se_Alarm01; //0
    [SerializeField] private SE_Clip_NonOverlap se_Alarm02; //1
    [SerializeField] private SE_Clip_NonOverlap se_PaperKnife; //2
    [SerializeField] private SE_Clip_NonOverlap se_Taping; //3
    [SerializeField] private SE_Clip_NonOverlap se_TapeTearing; //4
    [SerializeField] private SE_Clip_NonOverlap se_Bell; //5
    [SerializeField] private SE_Clip_NonOverlap se_OpenDoor; //6
    [SerializeField] private SE_Clip_NonOverlap se_SpaceShip; //7
    //[SerializeField] private float se_Alarm01_Delay = 3.5f;

    [Header("Sound Effect Setting_Overlap")]
    [SerializeField] private AudioClip se_Click;
    [SerializeField] private AudioClip se_Notification;
    [SerializeField] private AudioClip se_CardFlip;

    private Coroutine corutine_Alarm1_Recall;
    private Queue<Action>[] evQueues_NonOverlapSE = new Queue<Action>[(int)MySound.MySoundEffects_NonOverlap._max];
    private float[] delays_NonOverlapSE = new float[(int)MySound.MySoundEffects_NonOverlap._max];

    //private float _delayTime_alram01 = 0.0f;

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
        m_audio = gameObject.GetComponent<AudioSource>();
        m_audio.clip = null;
        curBG = MySound.MyBGs.Null;

        //InitSESet();
        for (int i = 0; i < delays_NonOverlapSE.Length; i++)
        {
            evQueues_NonOverlapSE[i] = new Queue<Action>();
            delays_NonOverlapSE[i] = 0.0f;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SetSoundEffect_Overlap(MySound.MySoundEffects_Overlap.Click);
        for(int i = 0; i < evQueues_NonOverlapSE.Length; i++)
        {
            if (delays_NonOverlapSE[i] > 0.0f)
                delays_NonOverlapSE[i] -= Time.deltaTime;
            else if (evQueues_NonOverlapSE[i].Count > 0)
                evQueues_NonOverlapSE[i].Dequeue().Invoke();
        }
    }

    public void SetBG(MySound.MyBGs _bg)
    {
        if(curBG != _bg)
        {
            m_audio.Stop();
            curBG = _bg;
            switch(_bg)
            {
                case MySound.MyBGs.Lobby:
                    m_audio.clip = gameLobby;
                    m_audio.Play();
                    break;
                case MySound.MyBGs.MainGame:
                    m_audio.clip = mainGame;
                    m_audio.Play();
                    break;
                case MySound.MyBGs.End01:
                    m_audio.clip = end01;
                    m_audio.Play();
                    break;
            }
        }
    }

    public void SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap _se)
    {
        //se_List_NonOverlap[(int)_se].evQueue.Enqueue(execute);
        evQueues_NonOverlapSE[(int)_se].Enqueue(execute);
        if (_se == MySound.MySoundEffects_NonOverlap.Alarm01)
            corutine_Alarm1_Recall = StartCoroutine(DelayedReExecute_Alarm01());
        void execute()
        {

            AudioClip _clip = null;
            switch(_se)
            {
                case MySound.MySoundEffects_NonOverlap.Alarm01:
                    _clip = se_Alarm01.clip;
                    delays_NonOverlapSE[(int)_se] = se_Alarm01.delay;
                    break;
                case MySound.MySoundEffects_NonOverlap.Alarm02:
                    _clip = se_Alarm02.clip;
                    delays_NonOverlapSE[(int)_se] = se_Alarm02.delay;
                    break;
                case MySound.MySoundEffects_NonOverlap.PaperKnife:
                    _clip = se_PaperKnife.clip;
                    delays_NonOverlapSE[(int)_se] = se_PaperKnife.delay;
                    break;
                case MySound.MySoundEffects_NonOverlap.Taping:
                    _clip = se_Taping.clip;
                    delays_NonOverlapSE[(int)_se] = se_Taping.delay;
                    break;
                case MySound.MySoundEffects_NonOverlap.TapeTearing:
                    _clip = se_TapeTearing.clip;
                    delays_NonOverlapSE[(int)_se] = se_TapeTearing.delay;
                    break;
                case MySound.MySoundEffects_NonOverlap.Bell:
                    _clip = se_Bell.clip;
                    delays_NonOverlapSE[(int)_se] = se_Bell.delay;
                    break;
                case MySound.MySoundEffects_NonOverlap.OpenDoor:
                    _clip = se_OpenDoor.clip;
                    delays_NonOverlapSE[(int)_se] = se_OpenDoor.delay;
                    break;
                case MySound.MySoundEffects_NonOverlap.SpaceShip:
                    _clip = se_SpaceShip.clip;
                    delays_NonOverlapSE[(int)_se] = se_SpaceShip.delay;
                    break;
            }

            if(_clip != null)
                m_audio.PlayOneShot(_clip);
        }
    }

    private IEnumerator DelayedObjectUnActive(GameObject _go, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        _go.SetActive(false);
        yield break;
    }


    private IEnumerator DelayedReExecute_Alarm01()
    {
        yield return new WaitForSeconds(se_Alarm01.delay);
        SetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap.Alarm01);
        yield break;
    }

    public void UnSetSoundEffect_NonOverlap(MySound.MySoundEffects_NonOverlap _se)
    {
        evQueues_NonOverlapSE[(int)_se].Clear();
        if (_se == MySound.MySoundEffects_NonOverlap.Alarm01 && corutine_Alarm1_Recall != null)
            StopCoroutine(corutine_Alarm1_Recall);
    }


    public void SetSoundEffect_Overlap(MySound.MySoundEffects_Overlap _se)
    {
        AudioClip _clip = null;
        switch(_se)
        {
            case MySound.MySoundEffects_Overlap.Click:
                _clip = se_Click;
                break;
            case MySound.MySoundEffects_Overlap.Notification:
                _clip = se_Notification;
                break;
            case MySound.MySoundEffects_Overlap.CardFlip:
                _clip = se_CardFlip;
                break;
        }
        if (_clip != null)
            m_audio.PlayOneShot(_clip);
    }
}
