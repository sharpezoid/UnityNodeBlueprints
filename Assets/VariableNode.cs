using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableNode : Node
{
    public new Color color = Color.red;

    public VariableNode(Vector2 _pos) : base(_pos)
    {
        //position = new Rect(_pos, sizeDelta);
    }
}
