using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The plugin for flow connection between nodes, 
/// lines are drawn from these to other nodes
/// </summary>
public class FlowConnector
{
    public enum FlowType
    {
        Input,
        Output
        // Both?  we should have 2 flowconnectors instead
    }
    public FlowType flow = FlowType.Output;


    public void Draw()
    {
        
    }
}
