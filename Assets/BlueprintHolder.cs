using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blueprint Holder component, attach to gameobject and attach blueprints to it, these will then be controlled by the player.
/// </summary>
[System.Serializable]
public class BlueprintHolder : MonoBehaviour
{
    //[SerializeField]
    //public List<Object> objects = new List<Object>();
    [SerializeField]
    public List<Blueprint> blueprints = new List<Blueprint>();

    private void Awake()
    {
        Debug.Log("Adding " + blueprints.Count + " blueprints from " + gameObject.name);

        BlueprintPlayer.Instance.AddBlueprints(gameObject, blueprints);
    }
}