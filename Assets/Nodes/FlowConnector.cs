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
    }
    public FlowType flowType = FlowType.Output;

    public Node node;

    public Action<FlowConnector> OnDragConnector;

    public FlowConnector(Node _node, FlowType _type, Action<FlowConnector> _OnDragConnector)
    {
        node = _node;
        flowType = _type;
        OnDragConnector = _OnDragConnector;
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

        if (GUI.Button(position, ">"))//, style))
        {
            if (OnDragConnector != null)
            {
                Debug.Log("GETTING > AND DRAG CONNECTOR EXISTS");
                OnDragConnector(this);
            }
            else
            {
                Debug.Log("GETTING > AND DRAG CONNECTOR IS NULL");
            }
        }
    }
}