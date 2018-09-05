using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Area where we work with nodes.
/// </summary>
public class DraftArea : EditorWindow
{
    static BlueprintEditor m_Editor;

    Vector2 mousePos;   // -- Mouse postion update by Event.current.mousePosition so we can have it during other event types...

    static Connector dragOrigin;

    public void Init(BlueprintEditor _m_Editor)
    {
        m_Editor = _m_Editor;
    }

    public void Draw()
    {
        if (!m_Editor){ return; }
        if (m_Editor.CurrentBlueprint == null) { return; }

        DrawNodes();
        DrawConnections();

        DrawCurrentDragConnection(Event.current);

        ProcessDraftEvents(Event.current);
        ProcessNodeEvents(Event.current);
    }


    private void ProcessDraftEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)      // -- RIGHT CLICK TO SHOW NODE SELECTION
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


    public static void OnDragConnector(Connector outPoint)
    {
        Debug.Log("DRAG CONNECTOR!");
        dragOrigin = outPoint;

        // -- Deselect any nodes so as to not drag selected nodes...
        DeselectAllNodes();
    }

    public static void OnStopDragConnector(Connector overPoint)
    {
        //dragEnd = inPoint;
        if (dragOrigin != null && dragOrigin.Node != overPoint.Node && overPoint.flowType != dragOrigin.flowType)
        {
            CreateConnection(overPoint, dragOrigin);
        }

        dragOrigin = null;
    }


    static void CreateConnection(Connector _out, Connector _in)
    {
        ConnectionData A = new ConnectionData(_out.Node, _out.index, _out.flowType);
        ConnectionData B = new ConnectionData(_in.Node, _in.index, _in.flowType);

        // -- Check it doesn't already exist.
        bool exists = false;
        foreach ( Connection c in m_Editor.CurrentBlueprint.connections)
        {
            if (c.A.node == A.node && c.A.index == A.index && c.B.node == B.node && c.B.index == B.index)
            {
                exists = true;
                continue;
            }
        }

        if (!exists)
        {
            m_Editor.CurrentBlueprint.connections.Add(new Connection(A, B));
        }
    }

    private void ClearConnection()
    {
        //dragOrigin = null;
        //dragEnd = null;
    }


    private void DrawCurrentDragConnection(Event e)
    {
        if (dragOrigin != null)
        {
            Handles.DrawBezier(
                dragOrigin.GetPosition(),
                e.mousePosition,
                dragOrigin.GetPosition() - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                5f
            );
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

        m_Editor.Repaint();
    }

    
    static void DeselectAllNodes()
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

    void DrawConnections()
    {
        if (m_Editor.CurrentBlueprint.connections != null)
        {
            foreach (Connection c in m_Editor.CurrentBlueprint.connections)
            {
                c.Draw();
            }
        }
    }
}
