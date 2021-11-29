using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyInfoText
{
    public enum Types
    {
        Box,
        Knife,
        Tape,
        InvoiceCover,
        Invoice
    }
}

public class MagnifierManager : MonoBehaviour
{
    private static MagnifierManager m_instance;
    public static MagnifierManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<MagnifierManager>();
            return m_instance;
        }
    }


    [SerializeField] private float infoDelayTime = 1.0f;
    [SerializeField] private MyDialogue.DialogueStruct info_Box;
    [SerializeField] private MyDialogue.DialogueStruct info_Knife;
    [SerializeField] private MyDialogue.DialogueStruct info_Tape;
    [SerializeField] private MyDialogue.DialogueStruct info_InvoiceCover;
    [SerializeField] private MyDialogue.DialogueStruct info_Invoice;

    private Coroutine endCorutine;

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

    public void SetInfoText(MyInfoText.Types _type)
    {
        EndCutSceneManager.instance.isMagnifierSet = true;
        StopInfoCoroutine();
        switch(_type)
        {
            case MyInfoText.Types.Box:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_Box.speaker, info_Box.dialogue);
                break;
            case MyInfoText.Types.Knife:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_Knife.speaker, info_Knife.dialogue);
                break;
            case MyInfoText.Types.Tape:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_Tape.speaker, info_Tape.dialogue);
                break;
            case MyInfoText.Types.InvoiceCover:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_InvoiceCover.speaker, info_InvoiceCover.dialogue);
                break;
            case MyInfoText.Types.Invoice:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_Invoice.speaker, info_Invoice.dialogue);
                endCorutine = StartCoroutine(DelayedOnConcentraition());
                break;
        }
        if(endCorutine == null)
            endCorutine = StartCoroutine(DelayedSetFalse());
    }

    public void StopInfoCoroutine()
    {
        if (endCorutine != null)
        {
            StopCoroutine(endCorutine);
            endCorutine = null; // 바로 위에 걸로 충분할건데 안전 용도.
        }
    }

    private IEnumerator DelayedSetFalse()
    {
        yield return new WaitForSeconds(infoDelayTime);
        EndCutSceneManager.instance.isMagnifierSet = false;
        yield break;
    }

    private IEnumerator DelayedOnConcentraition()
    {
        yield return new WaitForSeconds(infoDelayTime);
        EndCutSceneManager.instance.isMagnifierSet = false;
        SubPuzzleManager.instance.ActiveConcentration();
        CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
        yield break;
    }
}
