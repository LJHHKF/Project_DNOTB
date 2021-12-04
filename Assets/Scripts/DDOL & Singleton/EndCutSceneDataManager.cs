using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyDialogue
{
    [Serializable] public struct DialogueStruct
    {
        //public bool isLeft;
        public string speaker;
        [TextArea] public string dialogue;
    }
}


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
    [SerializeField] private Sprite cs_spr_end03_1;
    public Sprite prop_cs_spr_end03_1 { get { return cs_spr_end03_1; } }
    [SerializeField] private Sprite cs_spr_end03_2;
    public Sprite prop_cs_spr_end03_2 { get { return cs_spr_end03_2; } }
    [SerializeField] private Sprite cs_spr_end04;
    public Sprite prop_cs_spr_end04 { get { return cs_spr_end04; } }
    [SerializeField] private Sprite cs_spr_end05;
    public Sprite prop_cs_spr_end05 { get { return cs_spr_end05; } }

    [Header("Dialouge Setting")]
    //[SerializeField] private Sprite dialogue_spr_left;
    //public Sprite prop_dialogue_spr_left { get { return dialogue_spr_left; } }
    //[SerializeField] private Sprite dialogue_spr;
    //public Sprite prop_dialogue_spr_right { get { return dialogue_spr; } }
    [SerializeField] private float dialogueDelay;
    public float prop_dialogueDelay { get { return dialogueDelay; } } 
    [SerializeField] private float fadeOut_max_alpha;
    public float prop_fadeOut_max_alpha { get { return fadeOut_max_alpha; } }
    [SerializeField] private float fadeOut_max_second;
    public float prop_fadeOut_max_second { get { return fadeOut_max_second; } }
    [SerializeField] private MyDialogue.DialogueStruct[] dialogue_end02;
    public MyDialogue.DialogueStruct[] prop_dialogue_end02 { get { return dialogue_end02; } }
    [SerializeField] private MyDialogue.DialogueStruct[] dialogue_end03_1;
    public MyDialogue.DialogueStruct[] prop_dialogue_end03_1 { get { return dialogue_end03_1; } }
    [SerializeField] private MyDialogue.DialogueStruct[] dialogues_end03_2;
    public MyDialogue.DialogueStruct[] prop_dialogue_end03_2 { get { return dialogues_end03_2; } }
    [SerializeField] private MyDialogue.DialogueStruct[] dialogues_end04;
    public MyDialogue.DialogueStruct[] prop_dialogue_end04 { get { return dialogues_end04; } }
    [SerializeField] private MyDialogue.DialogueStruct[] dialogues_end05;
    public MyDialogue.DialogueStruct[] prop_dialogue_end05 { get { return dialogues_end05; } }

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
