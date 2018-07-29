using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blueprint represents the collection of nodes that we have.
/// </summary>
[System.Serializable]
public class Blueprint : ScriptableObject
{
    [SerializeField]
    public string str_Name = "Error";

    [SerializeField]
    public List<Node> nodes = new List<Node>();

    [SerializeField]
    public List<FlowConnection> connections = new List<FlowConnection>();

    public Blueprint()
    {

    }
}