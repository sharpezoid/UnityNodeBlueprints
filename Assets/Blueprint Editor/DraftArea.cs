﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Area where we work with nodes.
/// </summary>
public class DraftArea : EditorWindow
{
    BlueprintEditor m_Editor;

    Vector2 mousePos;   // -- Mouse postion update by Event.current.mousePosition so we can have it during other event types...

    public void Init(BlueprintEditor _m_Editor)
    {
        m_Editor = _m_Editor;
    }

    public enum DraftState
    {
        Default,
        AddingNode,
        SelectingArea,
        DragginNodes,
        COUNT
    }
    public DraftState draftState = DraftState.Default; 

    public void Draw()
    {
        if (!m_Editor){ return; }
        if (!m_Editor.CurrentBlueprint) { return; }

        DrawNodes();

        HandleNodeDrag();

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            draftState = DraftState.AddingNode;

            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < (int)NodeData.NodeType.COUNT; i++)
            {
                menu.AddItem(new GUIContent(((NodeData.NodeType)i).ToString()), false, AddNodeCallback, (NodeData.NodeType)i);
            }
            menu.ShowAsContext();
        }

        if (Event.current.mousePosition != null)
        {
            mousePos = Event.current.mousePosition;
        }
    }


    bool draggingSelectionRect = false;
    Vector2 mouseDownPos;
    Rect selectionRect;
    /// <summary>
    /// Node Selection and Drag handler
    /// </summary>
    void HandleNodeDrag()
    {
        // -- TRY TO SELECT A NODE
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log("LEFT MOUSE DOWN");

            bool clickedOnANode = false;
            // -- GOT A CLICK
            for (int nLoop = 0; nLoop < m_Editor.CurrentBlueprint.nodes.Count; nLoop++)
            {
                Node n = m_Editor.CurrentBlueprint.nodes[nLoop];

                if (n.IsDisplayingComment)
                {
                    n.HideComment();
                }

                if (n.position.Contains(Event.current.mousePosition))
                {
                    clickedOnANode = true;
                    if (!n.IsSelected)
                    {
                        n.Select(m_Editor.CurrentBlueprint, true);
                    }
                }
            }
            if (!clickedOnANode)
            {
                foreach (Node n in m_Editor.CurrentBlueprint.nodes)
                {
                    n.Deselect();
                }
            }

            // -- SET SELECTION RECT INITIAL X,Y
            mouseDownPos = Event.current.mousePosition;
        }
        // -- TRY TO DRAG A NODE...
        else if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            // -- ATTEMPT TO MOVE ALL NODES IN A SELECTION IF WE ARE NOT ALREADY
            bool haveASelectedNode = false;

            if (!draggingSelectionRect)
            {
                foreach (Node n in m_Editor.CurrentBlueprint.nodes)
                {
                    if (n.IsSelected)
                    {
                        haveASelectedNode = true;

                        n.position.x += /*mouseDownPos.x + */Event.current.delta.x;
                        n.position.y += /*mouseDownPos.y + */Event.current.delta.y;
                    }
                }
            }

            // -- OTHERWISE DRAG A SELECTION WINDOW AROUND NODES
            if (!haveASelectedNode)
            {
                draggingSelectionRect = true;

                // -- SET SELECTION RECT INITIAL X,Y
                if (mouseDownPos.x > Event.current.mousePosition.x)
                {
                    selectionRect.width = mouseDownPos.x - Event.current.mousePosition.x;
                    selectionRect.x = Event.current.mousePosition.x;
                }
                else
                {
                    selectionRect.width = Event.current.mousePosition.x - mouseDownPos.x;
                    selectionRect.x = mouseDownPos.x;
                }

                if (mouseDownPos.y > Event.current.mousePosition.y)
                {
                    selectionRect.height = mouseDownPos.y - Event.current.mousePosition.y;
                    selectionRect.y = Event.current.mousePosition.y;
                }
                else
                {
                    selectionRect.height = Event.current.mousePosition.y - mouseDownPos.y;
                    selectionRect.y = mouseDownPos.y;
                }

                Debug.Log("SELECTION RECT : " + selectionRect);

                // -- IF INSIDE THE RECTANGLE, SELECT THE NODE
                foreach (Node n in m_Editor.CurrentBlueprint.nodes)
                {
                    if (selectionRect.Overlaps(n.position))
                    {
                        n.Select(m_Editor.CurrentBlueprint);
                    }
                    else
                    {
                        n.Deselect();
                    }
                }
            }
        }
        else if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            draggingSelectionRect = false;
            //selectionRect = new Rect(0, 0, 0, 0);
        }

    }


    void AddNodeCallback(object userData)
    {
        NodeData.NodeType _type = (NodeData.NodeType)userData;

        switch (_type)
        {
            case NodeData.NodeType.Comment:
                m_Editor.CurrentBlueprint.nodes.Add(new CommentNode(mousePos));
                break;

            case NodeData.NodeType.Debug:
                m_Editor.CurrentBlueprint.nodes.Add(new Node(mousePos));
                break;

            case NodeData.NodeType.Event:
                m_Editor.CurrentBlueprint.nodes.Add(new Node(mousePos));
                break;

            case NodeData.NodeType.Function:
                m_Editor.CurrentBlueprint.nodes.Add(new Node(mousePos));
                break;

            case NodeData.NodeType.Variable:
                m_Editor.CurrentBlueprint.nodes.Add(new VariableNode(mousePos));
                break;
        }

        draftState = DraftState.Default;
    }


    void DrawNodes()
    {
        for (int nLoop = 0; nLoop < m_Editor.CurrentBlueprint.nodes.Count; nLoop++)
        {
            m_Editor.CurrentBlueprint.nodes[nLoop].Draw();
        }
    }
}
