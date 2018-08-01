using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;

[System.Serializable]
public class EventNode : Node
{
    FlowConnector inPoint = null;
    FlowConnector outPoint = null;

    public override void InitNodeEditor(Vector2 pos, NodeData.NodeType _type, BlueprintEditor _m_Editor)
    {
        base.InitNodeEditor(pos, _type, _m_Editor);

        inPoint = new FlowConnector(this, FlowConnector.FlowType.Input, m_Editor.DraftArea.OnDragConnector, m_Editor.DraftArea.OnStopDragConnector);
        outPoint = new FlowConnector(this, FlowConnector.FlowType.Output, m_Editor.DraftArea.OnDragConnector, m_Editor.DraftArea.OnStopDragConnector);
    }

    public override void Draw()
    {
        base.Draw();

        if (inPoint != null)
        {
            inPoint.Draw();
        }
        if (outPoint != null)
        {
            outPoint.Draw();
        }
    }
}
    