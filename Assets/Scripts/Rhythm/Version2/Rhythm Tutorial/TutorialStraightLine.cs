using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStraightLine : MonoBehaviour
{
    public LineRenderer line;

    private Transform from;
    private Transform to;

    void Awake()
    {
        if (line == null)
            line = GetComponent<LineRenderer>();

        if (line != null)
        {
            line.enabled = false;
            line.positionCount = 2;
            line.useWorldSpace = true;
        }
    }

    void Update()
    {
        if (line == null)
            return;

        if (from == null || to == null)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;
        line.positionCount = 2;

        line.SetPosition(0, from.position);
        line.SetPosition(1, to.position);
    }

    public void SetCurrentAndNext(TutorialNoteObject current, TutorialNoteObject next)
    {
        if (current == null || next == null) return;

        from = current.hitCircle != null ? current.hitCircle : current.transform;
        to = next.hitCircle != null ? next.hitCircle : next.transform;
    }

    public void ClearLine()
    {
        from = null;
        to = null;

        if (line != null)
            line.enabled = false;
    }
}
