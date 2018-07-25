using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeData
{
    public enum NodeType
    {
        Comment,
        Event,
        Variable,
        Function,
        Math,
        Macro,
        Debug,

        COUNT
    }

    public enum EventType
    {
        // -- General / Actor
        OnAwake,
        OnUpdate,
        OnFixedUpdate,
        OnLateUpdate,
        OnDestroy,

        // -- Input
        OnInput,
        OnMouseEnter,
        OnMouseExit,
        
        // -- Collision and Trigger
        OnCollision,
        OnEnterTrigger,

        // -- #TODO go through documentation and plan each of these, main triggers will be in BlueprintHolder (which i might make controller but it isnt? :S)
    }
}
