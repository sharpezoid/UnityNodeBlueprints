using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

// -- REPRESENTATION OF A NODE IN THE TOOL
[System.Serializable]
public class Node : ScriptableObject
{
    // -- THIS NODE TYPE
    public NodeData.NodeType nodeType = NodeData.NodeType.Debug;

    // -- COLOR OF THIS NODE
    public Color color = Color.magenta;

    //[HideInInspector]
    public Vector2 sizeDelta = new Vector2(100, 100);

    //[HideInInspector]
    public Rect position;
    
    //// -- node selection toggle
    private bool isSelected = false;
    private bool isDragged = false;
    public bool IsSelected
    {
        get { return isSelected; }
    }

    // -- Editor Reference
    [HideInInspector]
    public BlueprintEditor m_Editor = null;

    public virtual void InitNodeEditor(Vector2 pos, NodeData.NodeType _type, BlueprintEditor _m_Editor)
    {
        position = new Rect(pos, sizeDelta);
        nodeType = _type;
        m_Editor = _m_Editor;
    }

    public virtual bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (position.Contains(e.mousePosition))
                    {
                        isSelected = true;
                        GUI.changed = true;
                    }
                    else
                    {
                        isSelected = false;
                        GUI.changed = true;
                    }
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && IsSelected)
                {
                    isDragged = true;
                    position.position += e.delta;
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }


    public virtual void Draw()
    {
        GUI.Box(position, "");

        // -- OVERRIDE THIS CLASS AND CALL THE BASE FIRST THEN RENDER OWN THINGS ON THE PANEL
    }
}
