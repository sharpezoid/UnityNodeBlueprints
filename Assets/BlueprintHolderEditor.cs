using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Custom editor display for blueprints so we can add from our built ones and show errors etc.
/// </summary>
[CustomEditor(typeof(BlueprintHolder))]
[CanEditMultipleObjects]
public class BlueprintHolderEditor : Editor
{
    /*
    SerializedProperty blueprints;

    void OnEnable()
    {
        blueprints = serializedObject.FindProperty("bps");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(blueprints, new GUIContent("  Blueprints("+blueprints.arraySize+")"), true);

        SerializedProperty sp = blueprints.Copy(); // copy so we don't iterate the original

        if (sp.isArray)
        {
            int arrayLength = 0;

            // Get the array size
           //arrayLength = sp.arraySize;

            sp.Next(true); // skip generic field
            sp.Next(true); // advance to array size field

            // Get the array size
            arrayLength = sp.intValue;

            sp.Next(true); // advance to first array index

            // Write values to list
            List<Object> values = new List<Object>(arrayLength);
            int lastIndex = arrayLength - 1;
            for (int i = 0; i < arrayLength; i++)
            {
                values.Add(sp.objectReferenceValue); // copy the value to the list
                if (i < lastIndex) sp.Next(false); // advance without drilling into children
            }

            // iterate over the list displaying the contents
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] != null)
                {
                    EditorGUILayout.LabelField(i + " = " + values[i]);
                }
                else
                {
                    EditorGUILayout.HelpBox("ERROR: NULL/EMPTY DATA", MessageType.Error);
                }
                
            }

            //

            //Debug.Log("Type : " + blueprints.type + "   prop type : " + blueprints.propertyType);

            //for (int i = 0; i < blueprints.arraySize; i++)
            //{
            //    EditorGUILayout.LabelField(" BSP : " + i.ToString() + "  " + blueprints.GetArrayElementAtIndex(i).ToString());
            //}

            //System.Object o = blueprints.objectReferenceValue as System.Object;
            //List<Blueprint> bps = new List<Blueprint>();

            //if (bps != null)
            //{
            // Debug.Log("Blueprints ref : " + blueprints.objectReferenceValue.ToString() );

            //   // List<Blueprint> bps = blueprints.objectReferenceValue as System.Object as List<Blueprint>; //m_achievement.objectReferenceValue as System.Object as Achievement;

            //    // -- Render BP names stored in holder
            //    for (int bLoop = 0; bLoop < bps.Count; bLoop++)
            //    {
            //        EditorGUILayout.LabelField(bps[bLoop].str_Name);
            //    }
            //}
        

            // -- Add / Remove BPs
            if (GUILayout.Button("Add Blueprint"))
            {
                AddBlueprint();
            }

            if (GUILayout.Button("Clear"))
            {
                blueprints.ClearArray();
            }
        }

        // -- Display Errors / Warnings / Conflicts & Notes

        serializedObject.ApplyModifiedProperties();

    }
    
    void AddBlueprint()
    {
        Debug.Log("Add Blueprint");

        string path = EditorUtility.OpenFilePanel("Open Blueprint", "Assets/blueprints", "blueprint");

        if (path != "")
        {
            Object o = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

            SerializedProperty sp = blueprints.Copy(); // copy so we don't iterate the original

            if (sp.isArray)
            {
                int arrayLength = 0;

                // Get the array size
                //arrayLength = sp.arraySize;

                sp.Next(true); // skip generic field
                sp.Next(true); // advance to array size field

                // Get the array size
                arrayLength = sp.intValue;//.arraySize;

                sp.Next(true); // advance to first array index

                // Write values to list
                List<Object> values = new List<Object>(arrayLength);
                int lastIndex = arrayLength - 1;
                for (int i = 0; i < arrayLength; i++)
                {
                    values.Add(sp.objectReferenceValue); // copy the value to the list
                    if (i < lastIndex) sp.Next(false); // advance without drilling into children
                }

                // Add our new object
                values.Add(o);

                //blueprints = values;

                // iterate over the list displaying the contents
                //for (int i = 0; i < values.Count; i++)
                //{
                //    EditorGUILayout.LabelField(i + " = " + values[i]);
                //}



                blueprints.arraySize++;

                //blueprints.InsertArrayElementAtIndex(0);


                //XmlSerializer serializer = new XmlSerializer(typeof(Blueprint), extraTypes);
                //StreamReader reader = new StreamReader(path);
                //Blueprint deserializedBP = (Blueprint)serializer.Deserialize(reader.BaseStream);
                //reader.Close();

                //if (deserializedBP != null)
                //{
                //    Debug.Log("LOADED!" + path);
                //    m_Editor.CurrentBlueprint = deserializedBP;
                //}
            }
        }
    }
    */
}