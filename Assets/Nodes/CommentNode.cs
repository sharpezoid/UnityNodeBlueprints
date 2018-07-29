using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

[System.Serializable]
public class CommentNode : Node
{
    bool editing = false;

    [SerializeField]
    public string comment = "Enter comment...";

    public override bool ProcessEvents(Event e)
    {
        base.ProcessEvents(e);

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (position.Contains(e.mousePosition) && e.clickCount > 1)
                    {
                        editing = true;
                        GUI.changed = true;
                    }
                    else
                    {
                        editing = false;
                        GUI.changed = true;
                    }
                }
                break;
        }

        return false;
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
    }
}
