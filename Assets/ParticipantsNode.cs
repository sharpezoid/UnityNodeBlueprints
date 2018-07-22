using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class ParticipantsNode : Node
{
    const float NODE_UNSCALED_WIDTH = 60f;
    
    public ParticipantsNode(ToolWindow _toolWindow, Vector2 spawnPos) : base(_toolWindow)
    {
        toolWindow = _toolWindow;

        NodeUnscaledPosition = new Rect(60, 100, nodeWidth, nodeHeight);
    }

    public override void Draw()
    {
        UpdateNodeSize();

        // -- create texture for use in drawing this node...
        Texture2D texture = new Texture2D(1, 1);

        // -- DRAW BACKGROUND SELECTION IF SELECTED (YELLOW BOX BEHIND THE NODE)
        if (IsSelected)
        {
            texture.SetPixel(0, 0, Color.yellow);
            texture.Apply();

            GUI.DrawTexture(new Rect(DrawPosition.x - 2, DrawPosition.y - 2, DrawPosition.width + 4, DrawPosition.height + 4), texture);
        }

        // -- DRAW THE MAIN BODY OF THE NODE
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.DrawTexture(NodeUnscaledPosition, texture);

        // -- TITLE THE WINDOW
        GUI.Box(new Rect(DrawPosition.x, DrawPosition.y, nodeWidth, 20 * toolWindow.scale), "Wrestlers");

        // -- DRAW THE BUTTONS
        for (int pLoop = 0; pLoop < toolWindow.move.participants.Count; pLoop++)
        {
            if (GUI.Button(new Rect(DrawPosition.x + 10 * toolWindow.scale, 
                                    DrawPosition.y + 10 * toolWindow.scale + pLoop * 25 * toolWindow.scale + 25, 
                                    20 * toolWindow.scale, 
                                    20 * toolWindow.scale), 
                                    pLoop.ToString()))
            {
                // Start to drag participant stream.
                Debug.Log("Selected participant " + pLoop);
            }
        }

        // -- ADD ANOTHER PARTICIPANT
        if (GUI.Button(new Rect(DrawPosition.x + 10 * toolWindow.scale,
                                (DrawPosition.y + 10 + (toolWindow.move.participants.Count * 25)) * toolWindow.scale,

                                //NodeDrawPosition.y * toolWindow.scale + 10 * toolWindow.scale + toolWindow.move.participants.Count * 25 * toolWindow.scale + 25 * toolWindow.scale,
                                20 * toolWindow.scale,
                                20 * toolWindow.scale), 
                                "+"))
        {
            toolWindow.move.participants.Add(new MoveParticipant(toolWindow.move.participants.Count));
        }
    }

    // -- UPDATE THE SIZE OF THE NODE
    void UpdateNodeSize()
    {
        // -- WIDTH
        nodeWidth = NODE_UNSCALED_WIDTH;

        // -- HEIGHT
        nodeHeight = 25 + toolWindow.move.participants.Count * 25 + 50;
    }

    //public override void UpdatePositionAndScale()
    //{
    //    base.UpdatePositionAndScale();
    //    UpdateNodeSize();
    //}

    //void UpdateDrawPosition()
    //{
    //    DrawPosition = new Rect(NodeUnscaledPosition.x * toolWindow.scale + toolWindow.PinBoardDrawRect.x,
    //    NodeUnscaledPosition.y * toolWindow.scale + toolWindow.PinBoardDrawRect.y,
    //    nodeWidth * toolWindow.scale,
    //    nodeHeight * toolWindow.scale);
    //}
}
*/