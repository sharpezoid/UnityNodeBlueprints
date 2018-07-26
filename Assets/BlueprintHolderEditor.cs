using UnityEngine;
using UnityEditor;


/// <summary>
/// Custom editor display for blueprints so we can add from our built ones and show errors etc.
/// </summary>
[CustomEditor(typeof(BlueprintHolder))]
[CanEditMultipleObjects]
public class BlueprintHolderEditor : Editor
{
    SerializedProperty blueprints;

    void OnEnable()
    {
        blueprints = serializedObject.FindProperty("bps");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(blueprints);

        // -- Render BP names stored in holder

        // -- Add / Remove BPs
        if (GUILayout.Button("Add Blueprint"))
        {
            AddBlueprint();
        }

        // -- Display Errors / Warnings / Conflicts & Notes

        serializedObject.ApplyModifiedProperties();

    }
    
    void AddBlueprint()
    {
        Debug.Log("Add Blueprint");
    }
}