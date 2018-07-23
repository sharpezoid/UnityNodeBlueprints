using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableNode : Node
{
    // -- Variable nodes have a single output of a type
    public NodeOutput output;

    public VariableNode(Vector2 _pos) : base(_pos)
    {
        position = new Rect(_pos, new Vector2(100, 20));
        color = Color.blue;
    }
}
