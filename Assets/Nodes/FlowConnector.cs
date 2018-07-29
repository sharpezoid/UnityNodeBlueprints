using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// The plugin for flow connection between nodes, 
/// lines are drawn from these to other nodes
/// </summary>
public class FlowConnector
{
    public Rect position;

    public enum FlowType
    {
        Input,
        Output
        // Both?  we should have 2 flowconnectors instead
    }
    public FlowType flowType = FlowType.Output;

    public Node node;

    public Action<FlowConnector> OnClickFlowConnector;

    public FlowConnector(Node _node, FlowType _type)//, Action<FlowConnector> _OnClickFlowConnector)
    {
        node = _node;
        flowType = _type;
        //OnClickFlowConnector = _OnClickFlowConnector;
        position = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
    {
        position.y = node.position.y + (node.position.height * 0.5f) - position.height * 0.5f;

        switch (flowType)
        {
            case FlowType.Input:
                position.x = node.position.x - position.width + 8f;
                break;

            case FlowType.Output:
                position.x = node.position.x + node.position.width - 8f;
                break;
        }

        if (GUI.Button(position, ""))//, style))
        {
            if (OnClickFlowConnector != null)
            {
                OnClickFlowConnector(this);
            }
        }
    }
}