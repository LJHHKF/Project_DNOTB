using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetectOnMouseClick : MonoBehaviour
{
    [Header("For Detect On Mouse Click Setting")]
    [SerializeField] protected Transform leftUp;
    [SerializeField] protected Transform rightDown;
    protected Vector2 v_leftUp;
    protected Vector2 v_rightDown;
    protected Vector2 mousePosition;
    protected Camera myCam;
    protected bool isDowned;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myCam = Camera.main;

        v_leftUp = leftUp.position;
        v_rightDown = rightDown.position;
        isDowned = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            mousePosition = myCam.ScreenToWorldPoint(mousePosition);

            Debug.Log($"Down, v_leftUp: {v_leftUp}, v_rightDown: {v_rightDown}, mousePosition: {mousePosition}");

            if (mousePosition.x > v_leftUp.x
            && mousePosition.x < v_rightDown.x
            && mousePosition.y > v_rightDown.y
            && mousePosition.y < v_leftUp.y)
                isDowned = true;
            else
                isDowned = false;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (isDowned)
            {
                mousePosition = Input.mousePosition;
                mousePosition = myCam.ScreenToWorldPoint(mousePosition);

                Debug.Log($"Up, v_leftUp: {v_leftUp}, v_rightDown: {v_rightDown}, mousePosition: {mousePosition}");

                if (mousePosition.x > v_leftUp.x
                && mousePosition.x < v_rightDown.x
                && mousePosition.y > v_rightDown.y
                && mousePosition.y < v_leftUp.y)
                    Execute();
            }
        }
    }

    abstract protected void Execute();
}
