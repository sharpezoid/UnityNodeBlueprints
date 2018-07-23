﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// TOP MENU BAR, File Edit Etc.
/// </summary>
public class MenuBar : EditorWindow
{
    BlueprintEditor m_Editor;

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
            Debug.Log("EDIT CLICKED");
            // #TODO Edit Menu functionality
        }

        if (GUILayout.Button("Help", EditorStyles.miniButton, GUILayout.Width(100)))
        {
            Debug.Log("HALP CLICKED");
            // #TODO Readme + Info
        }

        GUILayout.EndHorizontal(); 
    }

    public void BuildFileContextMenu()
    {
        GenericMenu menu = new GenericMenu();
        menu.DropDown(GUILayoutUtility.GetLastRect());
        menu.AddItem(new GUIContent("New Blueprint"), false, CreateNewBlueprint);
        menu.AddItem(new GUIContent("Open Blueprint"), false, CustomLoad);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Save"), false, CustomSave);
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


    //void OpenExistingBlueprint()
    //{
    //    Debug.Log("Open Existing");

    //    string path = EditorUtility.OpenFilePanel("Open Blueprint", "Assets/blueprints", "asset");

    //    if (path.Contains("Assets/Blueprints/"))
    //    {
    //        // -- cut the end off the path so we are left with a file name...
    //        int cut = 0;
    //        for (int i = path.Length - 1; i >= 0; i--)
    //        {
    //            if (path[i] == '/')
    //            {
    //                cut = i + 1;
    //                break;
    //            }
    //        }
    //        path = path.Remove(0, cut);

    //        if (path.Length != 0)
    //        {
    //            //m_Editor.CurrentBlueprint = (Blueprint)AssetDatabase.LoadAssetAtPath("Assets/blueprints/" + path, typeof(Blueprint));
    //        }
    //    }
    //    else
    //    {
    //        EditorUtility.DisplayDialog("BAD LOCATION", "File does not appear to be in the Assets/Blueprints/ folder!", "Ok");
    //    }
    //}

    private void CustomSave()
    {
        string blueprintSaveString = "Error - no string found";
        foreach (Node n in m_Editor.CurrentBlueprint.nodes)
        {
            blueprintSaveString = n.SaveNode();
        }

        string filePath = Application.dataPath + "/Blueprints/" + m_Editor.CurrentBlueprint.str_Name + ".blueprint";
        File.WriteAllText(filePath, blueprintSaveString);
    }

    private void CustomLoad()
    {
        string path = EditorUtility.OpenFilePanel("Open Blueprint", "Assets/blueprints", "blueprint");

        //if (path.Contains("Assets/Blueprints/"))
        //{
        //    // -- cut the end off the path so we are left with a file name...
        //    int cut = 0;
        //    for (int i = path.Length - 1; i >= 0; i--)
        //    {
        //        if (path[i] == '/')
        //        {
        //            cut = i + 1;
        //            break;
        //        }
        //    }
        //    path = path.Remove(0, cut);

        //    if (path.Length != 0)
        //    {
        //        m_Editor.CurrentBlueprint = (Blueprint)AssetDatabase.LoadAssetAtPath("Assets/blueprints/" + path, typeof(Blueprint));
        //    }
        //}
        //else
        //{
        //    EditorUtility.DisplayDialog("BAD LOCATION", "File does not appear to be in the Assets/Blueprints/ folder!", "Ok");
        //}


        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

    private void SaveBlueprintData()
    {
        string dataAsJson = JsonUtility.ToJson(m_Editor.CurrentBlueprint);

        string filePath = Application.dataPath + "/Blueprints/asset.txt";
        File.WriteAllText(filePath, dataAsJson);

    }

    static void SaveBlueprint(bool shite)
    {
        //string path = "Assets/Resources/test.txt";
        string path = EditorUtility.OpenFilePanel("Open Blueprint", "Assets/blueprints", "asset");

        // -- Reverse parse the blueprint to create a neat file, we could JSON or something?


        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        
        writer.WriteLine("Test");
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("test");

        //Print the text from the file
        Debug.Log(asset.text);
    }

    static void OpenBlueprint()
    {
        string path = "Assets/Resources/test.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
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
