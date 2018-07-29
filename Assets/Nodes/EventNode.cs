using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;

[System.Serializable]
public class EventNode : Node
{
    public FlowConnector outPoint;

    public void OnEnable()
    {
        outPoint = new FlowConnector(this, FlowConnector.FlowType.Output);
    }

    public override void Draw()
    {
        base.Draw();

        if (outPoint != null)
        {
            outPoint.Draw();
        }

        if (Event.current.clickCount > 1)
        {
            if (position.Contains(Event.current.mousePosition))
            {
                Debug.Log("GOT DOUBLE CLICK? " + Event.current.clickCount.ToString() );
            }
        }
        else if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if (position.Contains(Event.current.mousePosition))
            {
                Debug.Log("Clicking Event Node");
            }
        }
    }
}
