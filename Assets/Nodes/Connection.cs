using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// The connection between 2 connection points
/// </summary>
[Serializable]
public class Connection 
{
    public ConnectionData A;
    public ConnectionData B;

    public Connection(ConnectionData _in, ConnectionData _out)
    {
        Debug.Log("NEW CONNECTION");
        A = _in;
        B = _out;
    }

    public void Draw()
    {
        //Draw curve...
        Handles.DrawBezier(
            A.GetPosition().center,
            B.GetPosition().center,
            A.GetPosition().center + Vector2.left * 50f,
            B.GetPosition().center - Vector2.left * 50f,
            Color.green,
            null,
            5.0f
        );
    }
}

[Serializable]
public class ConnectionData
{
    public Node node;
    public int index;
    public Connector.FlowType flowType;

    public ConnectionData(Node _node, int _index, Connector.FlowType _flowType)
    {
        node = _node;
        index = _index;
        flowType = _flowType;
    }

    public Rect GetPosition()
    {
        Rect rect = new Rect(node.position.x, node.position.y + index * 20, 20, 20);
        if (flowType == Connector.FlowType.Output)
        {
            rect.x = node.position.x + node.sizeDelta.x - 20;
        }

        return rect;
    }
}
