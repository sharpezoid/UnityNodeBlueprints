using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

/// <summary>
/// Any node that displays a flow input or output
/// </summary>
[System.Serializable]
public class FlowConnection
{
    public FlowConnector inPoint;
    public FlowConnector outPoint;

    public FlowConnection(FlowConnector _inPoint, FlowConnector _outPoint)
    {
        inPoint = _inPoint;
        outPoint = _outPoint;
    }

    public void Draw()
    {
        Handles.DrawBezier(
            inPoint.position.center,
            outPoint.position.center,
            inPoint.position.center + Vector2.left * 50f,
            outPoint.position.center - Vector2.left * 50f,
            Color.white,
            null,
            5.0f
        );
    }
}