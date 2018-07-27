using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BlueprintEditor : EditorWindow
{    
    // -- THIS EDITOR
    public static BlueprintEditor m_Instance = null;        
    
    // -- TOOL PARTS
    public static MenuBar m_MenuBar = null;                 // -- TOP BAR OF HIGH LEVEL CONTROLS
    public static DraftArea m_DraftArea = null;        // -- NODE WORKING AREA

    // -- OUR CURRENT BLUEPRINT
    private Blueprint currentBlueprint;
    public Blueprint CurrentBlueprint
    {
        get { return currentBlueprint; }
        set { currentBlueprint = value; }
    }


    /// <summary>
    /// Open Blueprint Editor window from inside unity
    /// </summary>
    [MenuItem("Window/Open Blueprint Editor")]
    static void OpenBlueprintEditor()
    {
        // -- Get existing open window or if none, make a new one:
        m_Instance = (BlueprintEditor)EditorWindow.GetWindow(typeof(BlueprintEditor));
        m_Instance.Show();

        // -- We need editor to be playing to see changes at run time
        if (EditorApplication.isPlaying == false)
        {
            EditorApplication.isPlaying = true;
        } 
    }


    private void OnEnable()
    {
        // -- INIT ALL TOOL PARTS
        m_MenuBar = new MenuBar();
        m_MenuBar.Init(this);

        m_DraftArea = new DraftArea();
        m_DraftArea.Init(this);

        // -- Check for a selected object
        CheckSelectionForHolder();        
        
        // -- Instantiate a player if one is needed
        BlueprintPlayer.Instance.InitEditor(this);
    }

    /// <summary>
    /// Handle selection of gameobjects in scene
    /// </summary>
    public bool hasHolder = false;
    void OnSelectionChange()
    {
        CheckSelectionForHolder();
    }

    void CheckSelectionForHolder()
    { 
        // -- We only check if we have one gameobject. #TODO check for non-modifiable gameobjects and prefabs
        if (Selection.gameObjects.GetLength(0) == 1)
        {
            hasHolder = (Selection.gameObjects[0].GetComponent<BlueprintHolder>() != null);
        }
    }


    void OnGUI()
    {
        // -- make sure we are init
        if( !m_MenuBar || !m_DraftArea)
        {
            return;
        }

        // -- DRAW ALL OF THE TOOL PARTS IN CONSTITUENT WINDOWS
        BeginWindows();

        // -- DRAW THE MENU BAR
        m_MenuBar.Draw();

        // -- DRAW THE DRAFT AREA
        m_DraftArea.Draw();

        EndWindows();

        // Repaint the window as wantsMouseMove doesnt trigger a repaint automatically
        if (Event.current != null)
        {
            Repaint();
        }
        
        // bolt you
        Resources.UnloadUnusedAssets();
    }
}
