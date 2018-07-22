using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BlueprintEditor : EditorWindow
{
    // -- OUR CURRENT BLUEPRINT
    private Blueprint currentBlueprint;
    public Blueprint CurrentBlueprint
    {
        get { return currentBlueprint; }
        set { currentBlueprint = value; }
    }

    public static BlueprintEditor m_Instance = null;        // -- THIS EDITOR
    

    // -- TOOL PARTS
    public static MenuBar m_MenuBar = null;                 // -- TOP BAR OF HIGH LEVEL CONTROLS
    public static DraftArea m_DraftArea = null;        // -- NODE WORKING AREA

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Open Blueprint Editor")]
    static void OpenBlueprintEditor()
    {
        // Get existing open window or if none, make a new one:
        m_Instance = (BlueprintEditor)EditorWindow.GetWindow(typeof(BlueprintEditor));
        m_Instance.Show();
    }

    private void OnEnable()
    {
        m_MenuBar = new MenuBar();
        m_MenuBar.Init(this);

        m_DraftArea = new DraftArea();
        m_DraftArea.Init(this);
    }


    void OpenBlueprint()
    {
        string path = EditorUtility.OpenFilePanel("Open Blueprint", "Assets/blueprints", "asset");

        if (path.Contains("Assets/Blueprints/"))
        {
            // -- cut the end off the path so we are left with a file name...
            int cut = 0;
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '/')
                {
                    cut = i + 1;
                    break;
                }
            }
            path = path.Remove(0, cut);

            if (path.Length != 0)
            {
                currentBlueprint = (Blueprint)AssetDatabase.LoadAssetAtPath("Assets/blueprints/" + path, typeof(Blueprint));
            }
        }
        else
        {
            EditorUtility.DisplayDialog("BAD LOCATION", "File does not appear to be in the Assets/Blueprints/ folder!", "Ok");
        }
    }


    void OnGUI()
    {
        // -- make sure we are init
        if( !m_MenuBar || !m_DraftArea)
        {
            return;
        }

        BeginWindows();

        // -- DRAW THE MENU BAR
        m_MenuBar.Draw();

        // -- DRAW THE DRAFT AREA
        m_DraftArea.Draw();

        EndWindows();


        if (Event.current != null)
        {
            // Repaint the window as wantsMouseMove doesnt trigger a repaint automatically
            Repaint();
        }
        

        // bolt you
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

}
