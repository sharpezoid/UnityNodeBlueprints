using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blueprint represents the collection of nodes that we have.
/// </summary>
public class Blueprint : ScriptableObject
{
    public List<Node> nodes = new List<Node>();
}
