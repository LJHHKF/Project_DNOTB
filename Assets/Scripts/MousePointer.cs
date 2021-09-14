using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    //protected Canvas m_canvas;
    //private float width_max;
    //private float height_max;

    public Texture2D cursorTexture;
    private Vector2 cursorImagePoint;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //m_canvas = gameObject.GetComponentInParent<Canvas>();
        //width_max = m_canvas.GetComponent<RectTransform>().rect.width * 0.5f;
        //height_max = m_canvas.GetComponent<RectTransform>().rect.height * 0.5f;

        StartCoroutine(MouseCursorMove());
    }

    protected IEnumerator MouseCursorMove()
    {
        yield return new WaitForEndOfFrame();
        cursorImagePoint.x = cursorTexture.width / 2;
        cursorImagePoint.y = cursorTexture.height / 2;
        Cursor.SetCursor(cursorTexture, cursorImagePoint, CursorMode.ForceSoftware);
    }
}
