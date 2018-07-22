using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// -- REPRESENTATION OF A NODE IN THE TOOL
[System.Serializable]
public class Node
{
    const float NODE_HEIGHT_PER_VARIABLE = 20.0f;       // -- how much distance for each variable 

    public Color color = Color.grey;

    // -- create texture for use in drawing this node...
    Texture2D texture = new Texture2D(1, 1);

    [HideInInspector]
    public Rect position = new Rect(100, 100, 100, 100);

    public Node(){}

    // -- editable comment on the node
    public string nodeComment = "Write Node Comment Here...";
    private bool isDisplayingComment = false;
    public bool IsDisplayingComment
    {
        get { return isDisplayingComment; }
    }
    public void DisplayComment()
    {
        isDisplayingComment = true;
    }
    public void HideComment()
    {
        isDisplayingComment = false;
    }

    // -- node selection toggle
    private bool isSelected = false;
    public bool IsSelected
    {
        get { return isSelected; }
    }
    public void Select(Blueprint _blueprint, bool deselectOtherNodes = false)
    {
        if (deselectOtherNodes)
        {
            foreach (Node n in _blueprint.nodes)
            {
                n.Deselect();
            }
        }
        isSelected = true;
    }
    public void Deselect()
    {
        isSelected = false;
        // -- hide comment if open
        isDisplayingComment = false;
    }

    public List<NodeConnection> connections = new List<NodeConnection>();

    public class NodeConnection
    {
        public int targetNodeIndex = 0;
        public float m_fCurrentNodeExitTime = 0.0f;
        public float m_fNewNodeEnterTime = 0.5f;
        public float m_fTransitionDuration = 0.4f;
        
        public NodeConnection(int newIndex)
        {
            targetNodeIndex = newIndex;
        }
    }

    public virtual void Draw()
    {
        // -- DRAW DROP SHADOW
        texture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 0.15f));
        texture.Apply();
        GUI.DrawTexture(new Rect(position.x + 3, position.y + 3, position.width, position.height), texture);

        // -- DRAW THE OUTLINE  /W or WO selection
        if (IsSelected)
        {
            texture.SetPixel(0, 0, Color.yellow);
            texture.Apply();
            GUI.DrawTexture(position, texture);
        }
        else
        {
            texture.SetPixel(0, 0, Color.white); //#TODO Tool Option for colours of nodes 
            texture.Apply();
            GUI.DrawTexture(position, texture);
        }

        // -- DRAW THE MAIN PANEL OF THE NODE
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.DrawTexture(new Rect(position.x + 1, position.y + 1, position.width -2, position.height -2), texture);

        // -- OVERRIDE THIS CLASS AND CALL THE BASE FIRST THEN RENDER OWN THINGS ON THE PANEL

    }
}
