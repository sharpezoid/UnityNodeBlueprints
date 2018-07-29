using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// quick test for changing back to scriptable objects...
public class MakeScriptableBlueprint
{
    [MenuItem("Assets/Create/My Scriptable Object")]
    public static void CreateMyAsset()
    {
        Blueprint asset = ScriptableObject.CreateInstance<Blueprint>();

        AssetDatabase.CreateAsset(asset, "Assets/Blueprints/NewScripableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/My Scriptable Node")]
    public static void CreateNode()
    {
        Node asset = ScriptableObject.CreateInstance<Node>();

        AssetDatabase.CreateAsset(asset, "Assets/Blueprints/Node.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/My Scriptable Event Node")]
    public static void CreateENode()
    {
        CommentNode asset = ScriptableObject.CreateInstance<CommentNode>();

        AssetDatabase.CreateAsset(asset, "Assets/Blueprints/Node.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Make Node Editor GUI Styles")]
    public static void CreateGUIStyle()
    {
        CommentNode asset = ScriptableObject.CreateInstance<CommentNode>();

        AssetDatabase.CreateAsset(asset, "Assets/Blueprints/Node.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}