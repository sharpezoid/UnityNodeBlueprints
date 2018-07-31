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
    public Action<FlowConnector> OnStopDragConnector;

    public FlowConnector(Node _node, FlowType _type, Action<FlowConnector> _OnDragConnector, Action<FlowConnector> _OnStopDragConnector)
    {
        node = _node;
        flowType = _type;
        OnDragConnector = _OnDragConnector;
        OnStopDragConnector = _OnStopDragConnector;
        position = new Rect(0, 0, 10f, 20f);
    }


    public void InitConnector(Node _node, FlowType _type, Action<FlowConnector> _OnDragConnector, Action<FlowConnector> _OnStopDragConnector)
    {
        node = _node;
        flowType = _type;
        OnDragConnector = _OnDragConnector;
        OnStopDragConnector = _OnStopDragConnector;
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

        GUI.Box(position, new GUIContent(">"));

        ProcessEvents(Event.current);
    }


    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0 && position.Contains(e.mousePosition))
                {
                    if (OnDragConnector != null)
                    {
                        Debug.Log("GETTING > AND DRAG CONNECTOR EXISTS");
                        OnDragConnector(this);
                    }

                    // -- Consume event
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                if (e.button == 0)
                {
                    if (OnStopDragConnector != null)
                    {
                        Debug.Log("STOP DRAG OF CONNECTOR");
                        OnStopDragConnector(this);
                    }
                }
                break;
        }
    }
}