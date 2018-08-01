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

    public const string bpLocation = "Assets/Blueprints";


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
        menu.AddItem(new GUIContent("Open Blueprint"), false, OpenBlueprint);
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

    #region callbacks
    void CreateNewBlueprint()
    {
        // -- Open new blueprint window
        BeginWindows();
        NewBlueprintPopup nameWindow = GetWindow<NewBlueprintPopup>();
        nameWindow.SetEditor(m_Editor);
        EndWindows();
    }

    void OpenBlueprint()
    {
        BeginWindows();
        OpenBlueprintPopup openWindow = GetWindow<OpenBlueprintPopup>();
        openWindow.SetEditor(m_Editor);
        EndWindows();
    }

    private void Save()
    {
        m_Editor.SaveCurrentBlueprint();
    }

    void ShowPreferencesWindow()
    {
        // -- #TODO create a preferences window, change colours, options etc
    }

    void ShowInfoWindow()
    {
        // -- #TODO Show info window about the tool and licensing etc...
    }
    #endregion

    /*
    private void XMLSave()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Blueprint), extraTypes);
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Blueprints/" + m_Editor.CurrentBlueprint.str_Name + ".blueprint");
        serializer.Serialize(writer.BaseStream, m_Editor.CurrentBlueprint);
        writer.Close();
    }

    private void XMLOpen()
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
    */
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
            if (System.IO.File.Exists("Assets/blueprints/" + newBlueprintName + "/" + newBlueprintName + ".asset"))
            {
                errorMsg = "BLUEPRINT ALREADY EXISTS!";
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
                Blueprint asset = ScriptableObject.CreateInstance<Blueprint>();

                AssetDatabase.CreateAsset(asset, "Assets/Blueprints/" + newBlueprintName + ".asset");
                AssetDatabase.SaveAssets();
                m_Editor.CurrentBlueprint = asset;

                this.Close();
            }
        }
        if (GUILayout.Button("Cancel"))
        {
            this.Close();
        }
    }
}


public class OpenBlueprintPopup : EditorWindow
{
    string newBlueprintName = "";
    BlueprintEditor m_Editor;
    string errorMsg = "";

    List<Blueprint> knownBlueprints = new List<Blueprint>();

    public void SetEditor(BlueprintEditor _in)
    {
        m_Editor = _in;

        knownBlueprints.Clear();

        string[] paths = Directory.GetDirectories("Assets/Blueprints");
        for (int i = 0; i < paths.GetLength(0); i++)
        {
            string bpName = paths[i].Remove(0, 18); // ew #TODO make this not rank, const string paths out and use their length

            Debug.Log("Checking Path : " + paths[i] + "   name : " + bpName );

            Blueprint bp = AssetDatabase.LoadAssetAtPath("Assets/Blueprints/" + bpName + "/" + bpName + ".asset", typeof(Blueprint)) as Blueprint;

            if (bp != null)
            {
                knownBlueprints.Add(bp);

                Debug.Log("Adding BP " + bp);
                Debug.Log("Adding BP name : " + bp.str_Name);
            }
            else
            {
                Debug.Log("Blueprint was null @ " + i);
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("SELECT BLUEPRINT", GUILayout.Height(30), GUILayout.ExpandWidth(true));
        GUILayout.Space(10);

        for (int i = 0; i < knownBlueprints.Count; i++)
        {
            if (GUILayout.Button(knownBlueprints[i].str_Name))
            {
                m_Editor.SetupBlueprint(knownBlueprints[i]);
                this.Close();
            }
        }

        if (GUILayout.Button("Cancel"))
        {
            this.Close();
        }
    }
}
