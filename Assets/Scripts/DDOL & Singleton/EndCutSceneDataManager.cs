using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutSceneDataManager : MonoBehaviour
{
    private static EndCutSceneDataManager m_instance;
    public static EndCutSceneDataManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<EndCutSceneDataManager>();
            return m_instance;
        }
    }

    [Header("Ending Sprite")]
    [SerializeField] private Sprite cs_spr_end01;
    public Sprite prop_cs_spr_end01 { get { return cs_spr_end01; } }
    [SerializeField] private Sprite cs_spr_end02;
    public Sprite prop_cs_spr_end02 { get { return cs_spr_end02; } }
    [SerializeField] private Sprite cs_spr_end03;
    public Sprite prop_cs_spr_end03 { get { return cs_spr_end03; } }

    [Header("Dialouge Setting")]
    [SerializeField] private float dialogueDelay;
    public float prop_dialogueDelay { get { return dialogueDelay; } } 
    [SerializeField] private float fadeOut_max_alpha;
    public float prop_fadeOut_max_alpha { get { return fadeOut_max_alpha; } }
    [SerializeField] private float fadeOut_max_second;
    public float prop_fadeOut_max_second { get { return fadeOut_max_second; } }
    [TextArea] [SerializeField] private string[] dialogue_end02;
    public string[] prop_dialogue_end02 { get { return dialogue_end02; } }

    private void Awake()
    {
        if (instance != this)
            Destroy(this);
    }

    private void OnDestroy()
    {
        if (m_instance == this)
            m_instance = null;
    }

}
