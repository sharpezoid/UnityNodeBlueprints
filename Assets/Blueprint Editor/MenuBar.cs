using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System;

/// <summary>
/// TOP MENU BAR, File Edit Etc.
/// </summary>
public class MenuBar : EditorWindow
{
    BlueprintEditor m_Editor;

    // -- Extra types for blueprint serialisation
    Type[] extraTypes = { typeof(Node),
                          typeof(CommentNode),
                          typeof(VariableNode) };
     
    public void Init(BlueprintEditor _m_Editor)
    {
        m_Editor = _m_Editor;
    }

    public void Draw()
    {
        // slight gap at very top to seperate from the window bar
        GUILayout.Space(2);

        // -- DRAW A ROW OF MENU OPTIONS
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("File", EditorStyles.miniButton, GUILayout.Width(100)))
        {
            BuildFileContextMenu();
        }


        if (GUILayout.Button("Edit", EditorStyles.miniButton, GUILayout.Width(100)))
        {
            BuildEditContextMenu();
        }


        if (GUILayout.Button("Help", EditorStyles.miniButton, GUILayout.Width(100)))
        {
            BuildHelpContextMenu();
        }

        GUILayout.EndHorizontal(); 
    }

    public void BuildFileContextMenu()
    {
        GenericMenu menu = new GenericMenu();
        //menu.DropDown(GUILayoutUtility.GetLastRect());
        menu.AddItem(new GUIContent("New Blueprint"), false, CreateNewBlueprint);
        menu.AddItem(new GUIContent("Open Blueprint"), false, Open);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Save"), false, Save);
        menu.ShowAsContext();
    }

    public void BuildEditContextMenu()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Preferences"), false, ShowPreferencesWindow);
        menu.ShowAsContext();
    }

    public void BuildHelpContextMenu()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("About"), false, ShowInfoWindow);
        menu.ShowAsContext();
    }


    void CreateNewBlueprint()
    {
        // -- check for "assets/blueprints" folder
        if (Directory.Exists("assets/blueprints"))      // -- can also do AssetDatabase.IsValidFolder
        {
            Debug.Log("GOT BLUEPRINTS FOLDER");
        }
        else
        {
            Directory.CreateDirectory("assets/blueprints");
            Debug.Log("CREATING BLUEPRINTS FOLDER");
        }

        // -- Open new blueprint window
        BeginWindows();
        NewBlueprintPopup nameWindow = GetWindow<NewBlueprintPopup>();
        nameWindow.SetEditor(m_Editor);
        EndWindows();
    }

    void ShowPreferencesWindow()
    {
        // -- #TODO create a preferences window, change colours, options etc
    }

    void ShowInfoWindow()
    {
        // -- #TODO Show info window about the tool and licensing etc...
    }


    private void Save()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Blueprint), extraTypes);
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Blueprints/" + m_Editor.CurrentBlueprint.str_Name + ".blueprint");
        serializer.Serialize(writer.BaseStream, m_Editor.CurrentBlueprint);
        writer.Close();
    }

    private void Open()
    {
        string path = EditorUtility.OpenFilePanel("Open Blueprint", "Assets/blueprints", "blueprint");

        if (path != "")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Blueprint), extraTypes);
            StreamReader reader = new StreamReader(path);
            Blueprint deserializedBP = (Blueprint)serializer.Deserialize(reader.BaseStream);
            reader.Close();

            if (deserializedBP != null)
            {
                Debug.Log("LOADED!" + path);
                m_Editor.CurrentBlueprint = deserializedBP;
            }
        }
    }
}



public class NewBlueprintPopup : EditorWindow
{
    string newBlueprintName = "";
    BlueprintEditor m_Editor;
    string errorMsg = "";

    public void SetEditor(BlueprintEditor _in)
    {
        m_Editor = _in;
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        GUILayout.Label("ENTER A NAME FOR THE NEW BLUEPRINT", GUILayout.Height(30), GUILayout.ExpandWidth(true));

        if (errorMsg != "")
        {
            Color c = GUI.color;
            GUI.color = Color.red;
            GUILayout.Box(errorMsg, GUILayout.ExpandWidth(true));
            GUI.color = c;
        }
        else
        {
            GUILayout.Space(25);
        }

        GUILayout.Space(10);

        newBlueprintName = EditorGUILayout.TextField(newBlueprintName, EditorStyles.textArea);
        GUILayout.Space(5);

        if (GUILayout.Button("OK"))
        {
            // -- Check name validity
            if (System.IO.File.Exists("Assets/blueprints/" + newBlueprintName + ".blueprint"))
            {
                errorMsg = "FILE EXISTS!";
            }
            else if (newBlueprintName == "" || newBlueprintName == string.Empty)
            {
                errorMsg = "EMPTY STRING";
            }
            else if (newBlueprintName.Length < 1)
            {
                errorMsg = "NAME TOO SHORT";
            }
            else
            {
                // -- create the asset and save it
                Blueprint newBlueprint = new Blueprint{str_Name = newBlueprintName};
                m_Editor.CurrentBlueprint = newBlueprint;

                this.Close();
            }
        }
        if (GUILayout.Button("Cancel"))
        {
            this.Close();
        }
    }
}
