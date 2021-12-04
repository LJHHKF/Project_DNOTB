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
        Invoice,
        Table,
        PortalBlue,
        PortalOrenge,
        Cube_Init,
        Cube_Second,
        Cube_Final,
        TableButtonCover_Init,
        TableButtonCover_Final,
        TableButton_Init,
        TableButton_Final
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
    [SerializeField] private MyDialogue.DialogueStruct info_Table;
    [SerializeField] private MyDialogue.DialogueStruct info_PortalBlue;
    [SerializeField] private MyDialogue.DialogueStruct info_PortalOrenge;
    [SerializeField] private MyDialogue.DialogueStruct info_Cube_Init;
    [SerializeField] private MyDialogue.DialogueStruct info_Cube_Second;
    [SerializeField] private MyDialogue.DialogueStruct info_Cube_Final;
    [SerializeField] private MyDialogue.DialogueStruct info_TableButtonCover_Init;
    [SerializeField] private MyDialogue.DialogueStruct info_TableButtonCover_Final;
    [SerializeField] private MyDialogue.DialogueStruct info_TableButton_Init;
    [SerializeField] private MyDialogue.DialogueStruct info_TableButton_Final;

    private Coroutine endCorutine;

    private bool m_isCubeInvest = false;
    public bool isCubeInvest { get { return m_isCubeInvest; } }

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
        SoundManager.instance.SetSoundEffect_Overlap(MySound.MySoundEffects_Overlap.Notification);
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
            case MyInfoText.Types.Table:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_Table.speaker, info_Table.dialogue);
                break;
            case MyInfoText.Types.PortalBlue:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_PortalBlue.speaker, info_PortalBlue.dialogue);
                break;
            case MyInfoText.Types.PortalOrenge:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_PortalOrenge.speaker, info_PortalOrenge.dialogue);
                break;
            case MyInfoText.Types.Cube_Init:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_Cube_Init.speaker, info_Cube_Init.dialogue);
                m_isCubeInvest = true;
                break;
            case MyInfoText.Types.Cube_Second:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_Cube_Second.speaker, info_Cube_Second.dialogue);
                break;
            case MyInfoText.Types.Cube_Final:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_Cube_Final.speaker, info_Cube_Final.dialogue);
                break;
            case MyInfoText.Types.TableButtonCover_Init:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_TableButtonCover_Init.speaker, info_TableButtonCover_Init.dialogue);
                break;
            case MyInfoText.Types.TableButtonCover_Final:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_TableButtonCover_Final.speaker, info_TableButtonCover_Final.dialogue);
                break;
            case MyInfoText.Types.TableButton_Init:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_TableButton_Init.speaker, info_TableButton_Init.dialogue);
                break;
            case MyInfoText.Types.TableButton_Final:
                EndCutSceneManager.instance.MagnifierDialogueSet(info_TableButton_Final.speaker, info_TableButton_Final.dialogue);
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

    public void ResetInfo()
    {
        m_isCubeInvest = false;
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
