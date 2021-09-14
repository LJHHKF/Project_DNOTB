using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCursor;

namespace MyCursor
{
    public enum CursorType
    {
        None,
        Normal,
        Knife
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

    private CursorType current_type = CursorType.None;

    [SerializeField] private Texture2D normalCursorImg;
    [SerializeField] private Vector2 normalCImagePivot;
    private Vector2 m_normalPivot;
    [SerializeField] private Texture2D knifeCursorImg;
    [SerializeField] private Vector2 knifeCImagePivot;
    private Vector2 m_knifePivot;



    // Start is called before the first frame update
    void Start()
    {
        m_normalPivot = new Vector2(normalCursorImg.width * normalCImagePivot.x, normalCursorImg.height * normalCImagePivot.y);
        m_knifePivot = new Vector2(knifeCursorImg.width * knifeCImagePivot.x, knifeCursorImg.height * knifeCImagePivot.y);

        MySetCursor(CursorType.Normal);
    }

    public void MySetCursor(MyCursor.CursorType _type)
    {
        if (current_type != _type)
        {
            switch (_type)
            {
                case CursorType.Normal:
                    Cursor.SetCursor(normalCursorImg, m_normalPivot, CursorMode.Auto);
                    break;
                case CursorType.Knife:
                    Cursor.SetCursor(knifeCursorImg, m_knifePivot, CursorMode.Auto);
                    break;
            }
        }
        else
            return;
    }
}
