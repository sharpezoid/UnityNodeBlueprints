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

    private void OnEnable()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);

        style = new GUIStyle();
        style.normal.background = texture;
       // style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        style.border = new RectOffset(6, 6, 6, 6);
    }

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
