using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

[System.Serializable]
public class EventNode : Node
{
    #region Constructors
    public EventNode() : base()
    {

    }

    public EventNode(Vector2 _pos) : base(_pos)
    {
        position = new Rect(_pos, new Vector2(100, 30));
        color = Color.red;
        nodeType = NodeData.NodeType.Comment;
    }
    #endregion

    public override void Draw()
    {
        base.Draw();

        if (Event.current.clickCount > 1)
        {
            if (position.Contains(Event.current.mousePosition))
            {
                Debug.Log("GOT DOUBLE CLICK? " + Event.current.clickCount.ToString() );
            }
        }
        else if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if (position.Contains(Event.current.mousePosition))
            {
                Debug.Log("Clicking Event Node");
            }
        }
    }
}
