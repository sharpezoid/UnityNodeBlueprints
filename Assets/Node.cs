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

    // -- TEXTURE FOR RENDERING NODE
    Texture2D texture;

    //[HideInInspector]
    public Vector2 sizeDelta = new Vector2(100, 100);

    //[HideInInspector]
    public Rect position;

    public void InitNode(Vector2 pos, NodeData.NodeType _type)
    {
        position = new Rect(pos, sizeDelta);
        nodeType = _type;
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
    }


    public virtual void Draw()
    {
        if (!texture)
        {
            texture = new Texture2D(1, 1);
        }

        // -- DRAW DROP SHADOW
        texture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f, 0.15f));
        texture.Apply();
        GUI.DrawTexture(new Rect(position.x + 3, position.y + 3, position.width, position.height), texture);

        // -- DRAW THE OUTLINE  W/WO selection
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
