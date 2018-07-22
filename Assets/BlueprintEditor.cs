using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class BlueprintEditor : EditorWindow
{
    // -- OUR COLLECTION OF BLUEPRINTS
    static Dictionary<string, Blueprint> blueprints = new Dictionary<string, Blueprint>();
    Blueprint currentBlueprint;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Open Blueprint Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        BlueprintEditor window = (BlueprintEditor)EditorWindow.GetWindow(typeof(BlueprintEditor));
        window.Show();

        // -- Load our scriptable object blueprints
        LoadBlueprints();
    }

    static void LoadBlueprints()
    {
        blueprints.Clear();

        object[] assets = AssetDatabase.LoadAllAssetsAtPath("assets/blueprints");

        foreach (object o in assets)
        {
            blueprints.Add(((Blueprint)o).name, (Blueprint)o);
        }
    }


    void OnGUI()
    {
        int selected = 0;
        string[] options = new string[]
        {
            "Option1", "Option2", "Option3",
        };
        selected = EditorGUILayout.Popup("Label", selected, options);

        if (GUILayout.Button("NEW BLUEPRINT"))
        {
            if (blueprints.ContainsKey(name))
            {
                if (EditorUtility.DisplayDialog("Blueprint of this name already exists!", "There is already a blueprint with this name, replace?", "Yes", "No"))
                {
                    CreateNewBlueprint();
                }
            }
            else
            {
                CreateNewBlueprint();
            }
        }

        if (currentBlueprint)
        {
            if (GUILayout.Button("CREATE NODE"))
            {
                CreateNewNode();
            }
            if (GUILayout.Button("CREATE VARIABLE NODE"))
            {
                currentBlueprint.nodes.Add(new VariableNode());
            }
        }

        if (!currentBlueprint){ return; }

        // -- Draw nodes
        DrawNodes();

        //HandleNodeClickAndDrag();

        HandleNodeDrag();


        if (Event.current != null)
        {
            // Repaint the window as wantsMouseMove doesnt trigger a repaint automatically
            Repaint();
        }

        Resources.UnloadUnusedAssets();
    }

    bool draggingSelectionRect = false;
    Vector2 mouseDownPos;
    Rect selectionRect;
    /// <summary>
    /// Node Selection and Drag handler
    /// </summary>
    void HandleNodeDrag()
    {
        // -- TRY TO SELECT A NODE
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log("MOUSE DOWN");

            bool clickedOnANode = false;
            // -- GOT A CLICK
            for (int nLoop = 0; nLoop < currentBlueprint.nodes.Count; nLoop++)
            {
                Node n = currentBlueprint.nodes[nLoop];

                if (n.IsDisplayingComment)
                {
                    n.HideComment();
                }

                if ( n.position.Contains(Event.current.mousePosition))
                {
                    clickedOnANode = true;
                    if (!n.IsSelected)
                    {
                        n.Select(currentBlueprint,true);
                    }
                }
            }
            if (!clickedOnANode)
            {
                foreach(Node n in currentBlueprint.nodes)
                {
                    n.Deselect();
                }
            }

            // -- SET SELECTION RECT INITIAL X,Y
            mouseDownPos = Event.current.mousePosition;
        }
        // -- TRY TO DRAG A NODE...
        else if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            // -- ATTEMPT TO MOVE ALL NODES IN A SELECTION IF WE ARE NOT ALREADY
            bool haveASelectedNode = false;

            if (!draggingSelectionRect)
            {
                foreach (Node n in currentBlueprint.nodes)
                {
                    if (n.IsSelected)
                    {
                        haveASelectedNode = true;

                        n.position.x = /*mouseDownPos.x + */Event.current.mousePosition.x;
                        n.position.y = /*mouseDownPos.y + */Event.current.mousePosition.y;
                    }
                }
            }

            // -- OTHERWISE DRAG A SELECTION WINDOW AROUND NODES
            if (!haveASelectedNode)
            {
                draggingSelectionRect = true;

                // -- SET SELECTION RECT INITIAL X,Y
                if (mouseDownPos.x > Event.current.mousePosition.x)
                {
                    selectionRect.width = mouseDownPos.x - Event.current.mousePosition.x;
                    selectionRect.x = Event.current.mousePosition.x;
                }
                else
                {
                    selectionRect.width = Event.current.mousePosition.x - mouseDownPos.x;
                    selectionRect.x = mouseDownPos.x;
                }

                if (mouseDownPos.y > Event.current.mousePosition.y)
                {
                    selectionRect.height = mouseDownPos.y - Event.current.mousePosition.y;
                    selectionRect.y = Event.current.mousePosition.y;
                }
                else
                {
                    selectionRect.height = Event.current.mousePosition.y - mouseDownPos.y;
                    selectionRect.y = mouseDownPos.y;
                }

                Debug.Log("SELECTION RECT : " + selectionRect);

                // -- IF INSIDE THE RECTANGLE, SELECT THE NODE
                foreach(Node n in currentBlueprint.nodes)
                {
                    if (selectionRect.Overlaps(n.position))
                    {
                        n.Select(currentBlueprint);
                    }
                    else
                    {
                        n.Deselect();
                    }
                }
            }
        }
        else if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            draggingSelectionRect = false;
            //selectionRect = new Rect(0, 0, 0, 0);
        }
        
    }

    /// <summary>
    /// CONTROLS RIGHT CLICK DRAG OF THE PINBOARD BACKGROUND
    /// </summary>
    //List<Node> draggedNodes = new List<Node>();
    //Vector2 startDragPos = new Vector2();
    //void HandleNodeClickAndDrag()
    //{
    //    // -- MOVE BOARD WITH RIGHT MOUSE BUTTON
    //    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
    //    {
    //        Debug.Log("Left Click");
    //        startDragPos = Event.current.mousePosition;

    //        for (int nLoop = 0; nLoop < currentBlueprint.nodes.Count; nLoop++)
    //        {
    //            if (currentBlueprint.nodes[nLoop].position.Contains(Event.current.mousePosition))
    //            {
    //                draggedNodes.Add(currentBlueprint.nodes[nLoop]);
    //            }
    //        }
    //    }
    //    if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && draggedNodes.Count > 0)
    //    {
    //        Debug.Log("Left Drag");

    //        for (int nLoop = 0; nLoop < draggedNodes.Count; nLoop++)
    //        {
    //            draggedNodes[nLoop].position = new Rect(startDragPos + Event.current.mousePosition, new Vector2(100, 100));
    //        }
    //    }
    //    if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && draggedNodes.Count > 0)
    //    {
    //        Debug.Log("End Left Drag");
    //        draggedNodes.Clear();
    //    }
    //}

    ///// <summary>
    ///// CONTROLS RIGHT CLICK DRAG OF THE PINBOARD BACKGROUND
    ///// </summary>
    //void HandleRightButtonDrag()
    //{
    //    // -- MOVE BOARD WITH RIGHT MOUSE BUTTON
    //    if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
    //    {
    //        Debug.Log("Right Click");
    //    }
    //    if (Event.current.type == EventType.MouseDrag && Event.current.button == 1)
    //    {
    //        Debug.Log("Right Drag");
    //        position = new Rect(Event.current.mousePosition, new Vector2(100, 100));
    //    }
    //    if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
    //    {
    //        Debug.Log("End Drag");
    //        dragging = false;
    //    }
    //}



    Blueprint CreateNewBlueprint()
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

        // -- create a new folder using the string entry

        // -- create the asset and save it
        Blueprint asset = CreateInstance<Blueprint>();
        AssetDatabase.CreateAsset(asset, "Assets/blueprints/New Blueprint.asset");
        AssetDatabase.SaveAssets();

        // -- add or replace to loaded blueprints 
        if (blueprints.ContainsKey(name))
        {
            blueprints[name] = asset;
        }
        else
        {
            blueprints.Add(name, asset);
        }

        // -- set current blueprint
        currentBlueprint = asset;

        return asset;
    }


    void CreateNewNode()
    {
        currentBlueprint.nodes.Add(new Node());
    }


    void DrawNodes()
    {
        for (int nLoop = 0; nLoop < currentBlueprint.nodes.Count; nLoop++)
        {
            Node n = currentBlueprint.nodes[nLoop];
            n.Draw();
        }
    }
}
