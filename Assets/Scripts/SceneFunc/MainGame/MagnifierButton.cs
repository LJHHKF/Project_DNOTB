using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnifierButton : MonoBehaviour
{
    private Image m_img;
    [SerializeField] private Color color_Selected;
    private Color color_UnSelected = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private void Start()
    {
        m_img = GetComponent<Image>();
        m_img.color = color_UnSelected;
    }

    private void OnEnable()
    {
        CursorManager.instnace.ev_UnsetMagnifire += UnsetEvent;
    }

    private void OnDisable()
    {
        if(CursorManager.instnace != null)
            CursorManager.instnace.ev_UnsetMagnifire -= UnsetEvent;
    }

    public void Execute()
    {
        MyCursor.CursorType _type = CursorManager.instnace.GetCurrentCursorType();

        if (_type == MyCursor.CursorType.Magnifier)
            CursorManager.instnace.MySetCursor(MyCursor.CursorType.Normal);
        else
        {
            CursorManager.instnace.MySetCursor(MyCursor.CursorType.Magnifier);
            m_img.color = color_Selected;
        }
    }

    private void UnsetEvent()
    {
        m_img.color = color_UnSelected;
    }
}
