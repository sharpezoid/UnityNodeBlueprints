using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public List<MoveParticipant> participants = new List<MoveParticipant>();

    public List<Node> nodes = new List<Node>();

    public Move(ToolWindow _toolWindow)
    {
        //participants.Add(new MoveParticipant(0));
        //nodes.Add(new ParticipantsNode(_toolWindow, new Vector2(10, 10)));
    }
}


public class MoveParticipant
{
    public int ID = -1;
    public MoveParticipant(int _ID)
    {
        ID = _ID;
    }
}