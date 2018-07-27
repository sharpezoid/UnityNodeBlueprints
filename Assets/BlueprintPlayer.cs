using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -- Script that lives in scene and plays blueprints at run-time
public class BlueprintPlayer : MonoSingleton<BlueprintPlayer>
{
    public Dictionary<GameObject, List<Blueprint>> blueprintReferences = new Dictionary<GameObject, List<Blueprint>>();
    
    // -- Allow us to connect to editor when needed
    private BlueprintEditor m_Editor;
    public void InitEditor(BlueprintEditor _editor)
    {
        m_Editor = _editor;
    }

    // -- Add blueprints to our collection
    public void AddBlueprints(GameObject go, List<Blueprint> bps)
    {
        blueprintReferences.Add(go, bps);

        // -- #TODO Perform Any Awake Blueprint Event Calls
    }
}
