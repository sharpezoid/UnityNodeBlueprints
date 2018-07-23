﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

public class CommentNode : Node
{
    bool editing = false;

    public string comment = "Enter comment...";

    public CommentNode(Vector2 _pos) : base(_pos)
    {
        position = new Rect(_pos, new Vector2(100, 30));
        color = Color.gray;
        nodeType = NodeData.NodeType.Comment;
    }

    public override void Draw()
    {
        base.Draw();

        if (editing)
        {
            comment = EditorGUI.TextField(position, comment); 
        }
        else
        {
            // -- DRAW THE REST OF THE NODE
            EditorGUI.HelpBox(position, comment, MessageType.Info);
        }

        if (Event.current.clickCount > 1)
        {
            if (position.Contains(Event.current.mousePosition))
            {
                Debug.Log("GOT DOUBLE CLICK? " + Event.current.clickCount.ToString() );
                editing = true;
            }
        }
        else if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if (position.Contains(Event.current.mousePosition))
            {
                editing = false;
            }
        }
    }


    public override string SaveNode()
    {
        StringBuilder sb = new StringBuilder();

        string save = base.SaveNode();
        sb.Append(save);

        // -- Comment
        sb.AppendLine("comment:" + comment);

        return sb.ToString();
    }


}
