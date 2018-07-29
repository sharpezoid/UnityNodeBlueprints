using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;

[System.Serializable]
public class EventNode : Node
{
    public FlowConnector outPoint;

    public override void Draw()
    {
        base.Draw();

        if (outPoint != null)
        {
            outPoint.Draw();
        }
        else
        {
            if (m_Editor != null && m_Editor.DraftArea != null)
            {
                outPoint = new FlowConnector(this, FlowConnector.FlowType.Output, m_Editor.DraftArea.OnDragConnector);
            }
        }
    }
}
