using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

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
        position = new Rect(0, node.position.y + 2.0f, 15f, 20f);
    }

    public void Draw()
    {
        position.y = node.position.y + 2.0f;

        switch (flowType)
        {
            case FlowType.Input:
                position.x = node.position.x - position.width + 10f;
                break;

            case FlowType.Output:
                position.x = node.position.x + node.position.width - 10f;
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

                    // -- Consume Event
                    e.Use();
                }
                if (e.button == 1 && position.Contains(e.mousePosition))
                {
                    Debug.Log("----------------------------------Showing Connection Removal Generic Menu --------------------");
                    ShowConnectorGenericMenu();

                    // -- Consume Event
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                if (e.button == 0 && position.Contains(e.mousePosition))
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


    void ShowConnectorGenericMenu()
    {
        GenericMenu menu = new GenericMenu();
        for (int cLoop = 0; cLoop < node.m_Editor.CurrentBlueprint.connections.Count; cLoop++)
        {
            menu.AddItem(new GUIContent("Remove "+ cLoop +" - #TODO NAME NODES "), false, RemoveConnection, node.m_Editor.CurrentBlueprint.connections[cLoop]);
        }
        menu.ShowAsContext();
    }

    void RemoveConnection(object userData)
    {
        FlowConnection removeMe = (FlowConnection)userData;

        // -- find the opposite side and remove it too.
        FlowConnector otherSide;
        if (flowType == FlowType.Input)
        {
            otherSide = removeMe.outPoint;
        }
        else
        {
            otherSide = removeMe.inPoint;
        }

        otherSide.node.m_Editor.CurrentBlueprint.connections.Remove(removeMe);
        node.m_Editor.CurrentBlueprint.connections.Remove(removeMe);
    }
}