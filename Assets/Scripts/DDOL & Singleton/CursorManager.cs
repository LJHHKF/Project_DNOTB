using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyCursor
{
    public enum CursorType
    {
        None,
        Normal,
        Knife,
        Magnifier
    }
}

public class CursorManager : MonoBehaviour
{
    private static CursorManager m_instance;
    public static CursorManager instnace
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<CursorManager>();
            return m_instance;
        }
    }

    private MyCursor.CursorType current_type = MyCursor.CursorType.None;

    [Serializable] private struct CursorSet
    {
        public Texture2D cursorImg;
        public Vector2 cImagePivot;
    }

    [SerializeField] private CursorSet noraml;
    private Vector2 m_normalPivot = Vector2.zero;
    [SerializeField] private CursorSet knife;
    private Vector2 m_knifePivot = Vector2.zero;
    [SerializeField] private CursorSet magnifier;
    private Vector2 m_magnifierPivot = Vector2.zero;

    public event Action ev_UnsetKnife;
    public event Action ev_UnsetMagnifire;

    private void Awake()
    {
        if (instnace != this)
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
        m_normalPivot = new Vector2(noraml.cursorImg.width * noraml.cImagePivot.x, noraml.cursorImg.height * noraml.cImagePivot.y);
        m_knifePivot = new Vector2(knife.cursorImg.width * knife.cImagePivot.x, knife.cursorImg.height * knife.cImagePivot.y);
        m_magnifierPivot = new Vector2(magnifier.cursorImg.width * magnifier.cImagePivot.x, magnifier.cursorImg.height * magnifier.cImagePivot.y);
    }

    public void MySetCursor(MyCursor.CursorType _type)
    {
        if (current_type != _type)
        {
            if (current_type == MyCursor.CursorType.Knife)
                ev_UnsetKnife?.Invoke();
            else if (current_type == MyCursor.CursorType.Magnifier)
                ev_UnsetMagnifire?.Invoke();
            switch (_type)
            {
                case MyCursor.CursorType.Normal:
                    Cursor.SetCursor(noraml.cursorImg, m_normalPivot, CursorMode.Auto);
                    break;
                case MyCursor.CursorType.Knife:
                    Cursor.SetCursor(knife.cursorImg, m_knifePivot, CursorMode.Auto);
                    break;
                case MyCursor.CursorType.Magnifier:
                    Cursor.SetCursor(magnifier.cursorImg, m_magnifierPivot, CursorMode.Auto);
                    break;
            }
            if (_type != MyCursor.CursorType.None)
                current_type = _type;
        }
        else
            return;
    }

    public MyCursor.CursorType GetCurrentCursorType()
    {
        return current_type;
    }
}
