using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelinePiece : MonoBehaviour
{
    public enum CorrectRots
    {
        _0 = 0,
        _90 = 1,
        _180 = 2,
        _270 = 3,
        None
    }

    private bool m_isSet;
    private bool isSet
    {
        get { return m_isSet; }
        set
        {
            m_isSet = value;
            highLightObject.SetActive(m_isSet);
        }
    }

    [SerializeField] private GameObject highLightObject;
    [SerializeField] private CorrectRots correct_rot_index;
    [SerializeField] private bool isStraight = false;
    private int m_rot_index = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(isStraight && correct_rot_index != CorrectRots.None)
        {
            if ((int)correct_rot_index >= 2)
                correct_rot_index -= 2;
        }

        ResetEvent();
        MainEventManager.instance.ev_Reset += ResetEvent;
    }

    private void OnDestroy()
    {
        if (MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= ResetEvent;
    }

    private void ResetEvent()
    {
        UnsetSelect();
        if (correct_rot_index == CorrectRots.None)
            m_rot_index = 0;
        else if (isStraight)
            m_rot_index = UnityEngine.Random.Range(0, 1);
        else
            m_rot_index = UnityEngine.Random.Range(0, 3);
        RotateSelf();
    }

    public void BTNClicked()
    {
        if(!isSet)
        {
            isSet = true;
            PipelinePuzzleEventManager.instance.SetSelectedPiece(this);
        }
        else
        {
            UnsetSelect();
            PipelinePuzzleEventManager.instance.UnSetSelectedPiece();
        }
    }

    public void UnsetSelect()
    {
        isSet = false;
    }

    public void Rotate(PipelinePuzzle.ClockwiseRotate _rot)
    {
        if (correct_rot_index != CorrectRots.None)
        {
            if (isStraight)
            {
                switch(_rot)
                {
                    case PipelinePuzzle.ClockwiseRotate.Clockwise:
                        if (m_rot_index >= 2)
                            m_rot_index = 0;
                        else
                            m_rot_index++;
                        break;
                    case PipelinePuzzle.ClockwiseRotate.ConterClockwise:
                        if (m_rot_index <= 0)
                            m_rot_index = 1;
                        else
                            m_rot_index--;
                        break;
                }
            }
            else
            {
                switch (_rot)
                {
                    case PipelinePuzzle.ClockwiseRotate.Clockwise:
                        if (m_rot_index >= 3)
                            m_rot_index = 0;
                        else
                            m_rot_index++;
                        break;
                    case PipelinePuzzle.ClockwiseRotate.ConterClockwise:
                        if (m_rot_index <= 0)
                            m_rot_index = 3;
                        else
                            m_rot_index--;
                        break;
                }
            }
            RotateSelf();
        }
    }

    private void RotateSelf()
    {
        Quaternion quaternion = Quaternion.Euler(new Vector3(0, 0, -m_rot_index * 90.0f));
        transform.localRotation = quaternion;
        PipelinePuzzleEventManager.instance.CheckEnd();
    }

    public bool CheckRotCorrect()
    {
        if (correct_rot_index == CorrectRots.None)
            return true;
        else
        {
            if ((int)correct_rot_index == m_rot_index)
                return true;
            else
                return false;
        }
    }
}
