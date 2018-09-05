using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

/// <summary>
/// Point on a node that can have a connection of data or flow attached to it.
/// types should inherit this to manage any cases
/// </summary>
public class Connector 
{
    protected Action<Connector> OnDrag;
    protected Action<Connector> EndDrag;
    public int index;
    Rect rect;

    public enum FlowType
    {
        Input,
        Output
    }
    public FlowType flowType = FlowType.Output;

    Node node;
    public Node Node
    {
        get { return node; }
    }

    public Connector(Node _node, FlowType _flowType)
    {
        OnDrag = DraftArea.OnDragConnector;
        EndDrag = DraftArea.OnStopDragConnector;
        node = _node;
        flowType = _flowType; 
    }

    public void Draw(int _index)
    {
        index = _index;
        rect = new Rect(node.position.x, node.position.y + index * 20, 20, 20);
        if (flowType == FlowType.Output)
        {
            rect.x = node.position.x + node.sizeDelta.x - 20;
        }
        GUI.Box(rect, new GUIContent(">"));


        ProcessEvents(Event.current);
    }

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0 && rect.Contains(e.mousePosition))
                {
                    if (OnDrag != null)
                    {
                        OnDrag(this);
                    }

                    // -- Consume Event
                    e.Use();
                }
                if (e.button == 1 && rect.Contains(e.mousePosition))
                {
                    Debug.Log("----------------------------------Showing Connection Removal Generic Menu --------------------");
                    ShowConnectorGenericMenu();

                    // -- Consume Event
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                if (e.button == 0 && rect.Contains(e.mousePosition))
                {
                    if (EndDrag != null)
                    {
                        EndDrag(this);
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
            menu.AddItem(new GUIContent("Remove " + cLoop + " - #TODO NAME NODES "), false, RemoveConnection, node.m_Editor.CurrentBlueprint.connections[cLoop]);
        }
        menu.ShowAsContext();
    }

    void RemoveConnection(object userData)
    {
        Connection removeMe = (Connection)userData;

        // -- find the opposite side and remove it too.
        Connector otherSide;
        if (flowType == FlowType.Input)
        {
            //otherSide = removeMe.outPoint;
        }
        else
        {
            //otherSide = removeMe.inPoint;
        }

        //otherSide.node.m_Editor.CurrentBlueprint.connections.Remove(removeMe);
        //node.m_Editor.CurrentBlueprint.connections.Remove(removeMe);
    }
}
