using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPuzzleSpace : MonoBehaviour
{
    [SerializeField] private int spaceIndex;
    public int prop_SpaceIndex { get { return spaceIndex; } }
    [SerializeField] private Transform t_up;
    [SerializeField] private Transform t_down;
    [SerializeField] private Transform t_left;
    [SerializeField] private Transform t_right;
    [SerializeField] private Vector2 endLocalPos;

    private bool m_isHad = false;
    public bool isHad
    {
        get { return m_isHad; }
        set { m_isHad = value; }
    }
    private SlidingPuzzleSpace moveSpace;

    public Transform GetToMoveTransform(SlidingPuzzle.MoveTo _move)
    {
        switch(_move)
        {
            case SlidingPuzzle.MoveTo.Left:
                if (t_left != null)
                {
                    moveSpace = t_left.GetComponent<SlidingPuzzleSpace>();
                    if (moveSpace.isHad)
                        return null;
                    else
                    {
                        m_isHad = false;
                        moveSpace.isHad = true;
                        return t_left;
                    }
                }
                break;
            case SlidingPuzzle.MoveTo.Right:
                if (t_right != null)
                {
                    moveSpace = t_right.GetComponent<SlidingPuzzleSpace>();
                    if (moveSpace.isHad)
                        return null;
                    else
                    {
                        m_isHad = false;
                        moveSpace.isHad = true;
                        return t_right;
                    }
                }
                break;
            case SlidingPuzzle.MoveTo.Up:
                if (t_up != null)
                {
                    moveSpace = t_up.GetComponent<SlidingPuzzleSpace>();
                    if (moveSpace.isHad)
                        return null;
                    else
                    {
                        m_isHad = false;
                        moveSpace.isHad = true;
                        return t_up;
                    }
                }
                break;
            case SlidingPuzzle.MoveTo.Down:
                if (t_down != null)
                {
                    moveSpace = t_down.GetComponent<SlidingPuzzleSpace>();
                    if (moveSpace.isHad)
                        return null;
                    else
                    {
                        m_isHad = false;
                        moveSpace.isHad = true;
                        return t_down;
                    }
                }
                break;
        }
        return null;
    }

    public Vector2 GetEndLocalPos()
    {
        return endLocalPos;
    }
}
