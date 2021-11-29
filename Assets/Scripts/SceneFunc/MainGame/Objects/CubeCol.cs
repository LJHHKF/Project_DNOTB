using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCol : MonoBehaviour, IEventObject
{
    [SerializeField] private Transform resetPoint;
    [SerializeField] private Transform firstWayPoint;
    [SerializeField] private Transform secondWayPoint;

    private int cubeActiveCount = 0;
    private Collider2D m_Collider;
    private Transform m_Transform;
    private bool isMoving;
    private readonly float moveSpeed = 0.1f;

    private void Start()
    {
        m_Collider = GetComponent<Collider2D>();
        m_Transform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        MyReset();
        MainEventManager.instance.ev_Reset += MyReset;
    }

    private void OnDisable()
    {
        if(MainEventManager.instance != null)
            MainEventManager.instance.ev_Reset -= MyReset;
    }

    private void MyReset()
    {
        transform.position = resetPoint.position;
        cubeActiveCount = 0;
        isMoving = false;
    }

    public void Execute()
    {
        if (!isMoving)
        {
            switch (cubeActiveCount)
            {
                case 0:
                    BoxMain.instance.isCubeMoved = true;
                    StartCoroutine(CubeMoving(1));
                    cubeActiveCount++;
                    break;
                case 1:
                    if (BoxMain.instance.isUntaped)
                    {
                        StartCoroutine(CubeMoving(2));
                        cubeActiveCount++;
                    }
                    break;
                case 2:
                    BoxMain.instance.DoCubePassPortal();
                    cubeActiveCount++;
                    break;
                default:
                    return;
            }
        }
    }

    IEnumerator CubeMoving(int _index)
    {
        isMoving = true;
        Vector2 _target = Vector2.zero;
        m_Collider.enabled = false;
        switch (_index)
        {
            case 1:
                _target = firstWayPoint.position;
                    break;
            case 2:
                _target = secondWayPoint.position;
                break;
            default:
                m_Collider.enabled = true;
                isMoving = false;
                yield break;
        }
        while (isMoving)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, _target, moveSpeed);

            if ((((Vector2)m_Transform.position) - _target).sqrMagnitude < 0.0000001f)
                isMoving = false;
            else
                yield return new WaitForFixedUpdate();
        }
        m_Collider.enabled = true;
        yield break;
    }
}
