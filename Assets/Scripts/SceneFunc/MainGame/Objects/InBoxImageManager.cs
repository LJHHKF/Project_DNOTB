using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBoxImageManager : MonoBehaviour
{
    [Header("SpriteSet")]
    [SerializeField] private Sprite spr_end03;
    [SerializeField] private Sprite spr_end04;

    [Header("Object Link")]
    [SerializeField] private GameObject obj_end04;

    private SpriteRenderer m_sprR; 

    private void Awake()
    { 
        ResetEvent();
        EventManager.instance.ev_Reset += ResetEvent;
    }

    private void ResetEvent()
    {
        GetComponent<SpriteRenderer>().sprite = null;
        obj_end04.SetActive(false);
    }

    private void ChkAndFind_SprR() // Start나 Awake에서 찾는 방식에서 왠지 오류나서 그냥 쓸 때 마다 찾아 쓰도록 만듦.
    {
        if (m_sprR == null)
            m_sprR = GetComponent<SpriteRenderer>();
    }

    public void SetEnding(MyEndings.UnboxingType _end)
    {
        ChkAndFind_SprR();
        switch(_end)
        {
            case MyEndings.UnboxingType.first:
                m_sprR.sprite = null;
                obj_end04.SetActive(false);
                break;
            case MyEndings.UnboxingType.third:
                m_sprR.sprite = spr_end03;
                obj_end04.SetActive(false);
                break;
            case MyEndings.UnboxingType.fourth:
                m_sprR.sprite = spr_end04;
                obj_end04.SetActive(true);
                break;
        }
    }
}
