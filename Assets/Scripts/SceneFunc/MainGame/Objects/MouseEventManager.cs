using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventManager : MonoBehaviour
{
    private Camera myMainCam;
    private Vector2 mousePosition;
    private readonly float maxDepth = 15.0f;
    private GameObject hitTarget;
    // Start is called before the first frame update
    void Start()
    {
        myMainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            mousePosition = myMainCam.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDepth);

            if(hit)
            {
                if(hit.collider.tag == "EventObjectCol")
                {
                    hitTarget = hit.transform.gameObject;
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            mousePosition = Input.mousePosition;
            mousePosition = myMainCam.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, maxDepth);

            if(hit)
            {
                if(hit.collider.tag == "EventObjectCol")
                {
                    if (ReferenceEquals(hitTarget, hit.transform.gameObject) && !ReferenceEquals(hitTarget, null))
                    {
                        hitTarget.GetComponent<IEventObject>().Execute();
                    }
                }
            }
        }
    }


}
