using System.Collections;
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

    private FlowConnector selectedOutPoint;
    private FlowConnector selectedInPoint;

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
        if (m_Editor.CurrentBlueprint == null) { return; }

        DrawNodes();

        ProcessEvents(Event.current);
        ProcessNodeEvents(Event.current);

        DrawConnectionLine(Event.current);
    }


    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    mousePos = e.mousePosition;
                    GenericMenu menu = new GenericMenu();
                    for (int i = 0; i < (int)NodeData.NodeType.COUNT; i++)
                    {
                        menu.AddItem(new GUIContent(((NodeData.NodeType)i).ToString()), false, AddNodeCallback, (NodeData.NodeType)i);
                    }
                    menu.ShowAsContext();
                }
                break;
        }
    }


    private void ProcessNodeEvents(Event e)
    {
        if (m_Editor.CurrentBlueprint.nodes != null)
        {
            for (int nLoop = 0; nLoop < m_Editor.CurrentBlueprint.nodes.Count; nLoop++)
            {
                if (m_Editor.CurrentBlueprint.nodes[nLoop].ProcessEvents(e))
                {
                    GUI.changed = true;
                }
            }
        }
    }

    public void OnDragConnector(FlowConnector outPoint)
    {
        Debug.Log("DRAG CONNECTOR!");
        selectedOutPoint = outPoint;

        // -- Deselect any nodes so as to not drag selected nodes...
        DeselectAllNodes();

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
               CreateConnection();
            }
            else
            {

            }
        }
    }

    public void OnStopDragConnector(FlowConnector outPoint)
    {
        Debug.Log("STOP DRAG CONNECTOR!");
        selectedOutPoint = null;
    }


    private void CreateConnection()
    {
        if (m_Editor.CurrentBlueprint.connections == null)
        {
            m_Editor.CurrentBlueprint.connections = new List<FlowConnection>();
        }

        m_Editor.CurrentBlueprint.connections.Add(new FlowConnection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }


    private void OnClickRemoveConnection(FlowConnection connection)
    {
        m_Editor.CurrentBlueprint.connections.Remove(connection);
    }


    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.position.center,
                e.mousePosition,
                selectedInPoint.position.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.position.center,
                e.mousePosition,
                selectedOutPoint.position.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }


    void AddNodeCallback(object userData)
    {
        NodeData.NodeType _type = (NodeData.NodeType)userData;

        int count = 0;
        foreach (Node _n in m_Editor.CurrentBlueprint.nodes)
        {
            if (_n.nodeType == _type) { count++; }
        }

        Node n = null;

        switch (_type)
        {
            case NodeData.NodeType.Comment:
                n = ScriptableObject.CreateInstance<CommentNode>(); 
                break;

            case NodeData.NodeType.Debug:
                //n = ScriptableObject.CreateInstance<CommentNode>();
                break;

            case NodeData.NodeType.Event:
                n = ScriptableObject.CreateInstance<EventNode>();
                break;

            case NodeData.NodeType.Function:
               // n = ScriptableObject.CreateInstance<CommentNode>();
                break;

            case NodeData.NodeType.Variable:
                //n = ScriptableObject.CreateInstance<CommentNode>();
                break;
        }

        n.InitNodeEditor(mousePos, _type, m_Editor);
        AssetDatabase.CreateAsset(n, "Assets/Blueprints/" + m_Editor.CurrentBlueprint.str_Name + "/nodes/" + _type.ToString() + "Node_" + count.ToString() + ".asset");
        AssetDatabase.SaveAssets();

        m_Editor.CurrentBlueprint.nodes.Add(n);

        m_Editor.SaveCurrentBlueprint();

        draftState = DraftState.Default;
    }

    
    void DeselectAllNodes()
    {
        for (int nLoop = 0; nLoop < m_Editor.CurrentBlueprint.nodes.Count; nLoop++)
        {
            m_Editor.CurrentBlueprint.nodes[nLoop].IsSelected = false;
        }
    }

    void DrawNodes()
    {
        for (int nLoop = 0; nLoop < m_Editor.CurrentBlueprint.nodes.Count; nLoop++)
        {
            // -- there could be a null entry so check for it
            if (m_Editor.CurrentBlueprint.nodes[nLoop])
            {
                m_Editor.CurrentBlueprint.nodes[nLoop].Draw();
            }
        }
    }
}
