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
    public Action<FlowConnection> OnClickRemoveConnection;

    public FlowConnection(FlowConnector _inPoint, FlowConnector _outPoint, Action<FlowConnection> _OnClickRemoveConnection)
    {
        inPoint = _inPoint;
        outPoint = _outPoint;
        OnClickRemoveConnection = _OnClickRemoveConnection;
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
            2f
        );

        //if (Handles.Button((inPoint.position.center + outPoint.position.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleCap))
        //{
        //    if (OnClickRemoveConnection != null)
        //    {
        //        OnClickRemoveConnection(this);
        //    }
        //}
    }
}