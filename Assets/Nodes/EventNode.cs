using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;

[System.Serializable]
public class EventNode : Node
{
    [NonSerialized]
    FlowConnector inPoint = null;
    [NonSerialized]
    FlowConnector outPoint = null;

    public override void Draw()
    {
        base.Draw();

        if (outPoint != null && inPoint != null)
        {
            outPoint.Draw();
            inPoint.Draw();
        }
        else
        {
            if (m_Editor != null && m_Editor.DraftArea != null)
            {
                inPoint = new FlowConnector(this, FlowConnector.FlowType.Input, m_Editor.DraftArea.OnDragConnector, m_Editor.DraftArea.OnStopDragConnector);
                outPoint = new FlowConnector(this, FlowConnector.FlowType.Output, m_Editor.DraftArea.OnDragConnector, m_Editor.DraftArea.OnStopDragConnector);
            }
        }
    }
}
    