using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;

[System.Serializable]
public class EventNode : Node
{
    public override void InitNodeEditor(Vector2 pos, NodeData.NodeType _type, BlueprintEditor _m_Editor)
    {
        base.InitNodeEditor(pos, _type, _m_Editor);

        inputs.Add(new Connector(this, Connector.FlowType.Input));
        outputs.Add(new Connector(this, Connector.FlowType.Output));
    }

    public override void Draw()
    {
        base.Draw();

        for (int iLoop = 0; iLoop < inputs.Count; iLoop++)
        {
            inputs[iLoop].Draw(iLoop);
        }

        for (int oLoop = 0; oLoop < outputs.Count; oLoop++)
        {
            outputs[oLoop].Draw(oLoop);
        }
    }
}
    