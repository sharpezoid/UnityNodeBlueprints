using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToolWindow : EditorWindow
{
    static float WINDOW_WIDTH = 1920;
    static float WINDOW_HEIGHT = 1000;

    static float PINBOARD_WIDTH = 1920;
    static float PINBOARD_HEIGHT = 1000;
     
    public Rect PinBoardDrawRect = new Rect(-PINBOARD_WIDTH / 2, -PINBOARD_HEIGHT / 2, PINBOARD_WIDTH, PINBOARD_HEIGHT);
    public Vector2 PinBoardOffset = new Vector2(0, 0);

    // -- SELF REFERENCE
    private static ToolWindow m_Instance = null;

    public Move move;

    // -- SCALE OF NODES
    public float scale = 1.0f;

    // -- TEXTURE FOR LINES
    static Texture2D lineTex;

    // -- HAVE WE STARTED UP OR NOT?
    bool setup = false;

    // -- SELECTION BOX RECT
    Vector2 selectionStartPosition;
    Rect selectionRect;

    float mouseDownTime = 0.0f;
    float dragWait = 0.1f; // -- how long before a click becomes a drag?

    bool draggingPinBoard = false;
    Vector2 mousePositionLastFrame;

    #region Init and OpenWindow
    [MenuItem("Tool/ToolTest")]
    static void Init()
    {
        Debug.ClearDeveloperConsole();
        OpenWindow();
    }

    public static ToolWindow OpenWindow()
    {
        ToolWindow window = (ToolWindow)EditorWindow.GetWindow(typeof(ToolWindow));
        window.Show();
        window.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);

        m_Instance = window;

        return window;
    }
    #endregion


    void Update()
    {
        #region Setup When Ready
        if (!setup)
        {
            lineTex = new Texture2D(1, 1);
            lineTex.SetPixel(0, 0, Color.gray);
            lineTex.Apply();

            if (Event.current != null)
            {
                PinBoardOffset.x -= (mousePositionLastFrame.x - Event.current.mousePosition.x);
                PinBoardOffset.y -= (mousePositionLastFrame.y - Event.current.mousePosition.y);
            }

            PinBoardDrawRect.width = PINBOARD_WIDTH * scale;
            PinBoardDrawRect.height = PINBOARD_HEIGHT * scale;

            PinBoardDrawRect.x = WINDOW_WIDTH * 0.5f - PinBoardDrawRect.width * 0.5f + PinBoardOffset.x * scale;
            PinBoardDrawRect.y = WINDOW_HEIGHT * 0.5f - PinBoardDrawRect.height * 0.5f + PinBoardOffset.y * scale;

            move = new Move(this);

            setup = true;
        }
        #endregion
    }

    void OnGUI()
    {
        if (move == null)
        {
            return;
        }

        GUI.Box(new Rect(10, 10, 200, 25), "Scale: " + scale);

        // -- draw pinboard grid
        DrawBackgroundGrid();

        // -- update position on right click drag
        HandlePinBoardDragAndZoom();

        // -- handle input for dragging of nodes.
       // HandleNodeDrag();

        // -- Draw Selection Box
        GUI.Box(selectionRect, "");

        // -- Handle all the input we can expect.
        HandleInput();

        if (Event.current != null)
        {
            // Repaint the window as wantsMouseMove doesnt trigger a repaint automatically
            Repaint();
        }

        // -- Draw all our nodes
        DrawNodes();
    }



    enum InputContext
    {
        None,
        DragNode,
        MoveBackground,
        DragParticipantStream,
        COUNT
    }
    InputContext inputContext;

    bool leftDragging = false;
    bool leftDownLastFrame = false;

    bool rightDragging = false;
    bool rightDownLastFrame = false;

    void HandleInput()
    {
        if (Event.current == null)
        {
            return;
        }

        // -- left click
        if (Event.current.button == 0)
        {
            if (Event.current.type == EventType.MouseDown && !leftDragging)
            {
                Debug.Log("Left Click");
                leftDownLastFrame = true;
            }
            else if (Event.current.type == EventType.MouseUp && !leftDragging)
            {
                Debug.Log("Left Click Up");
                leftDownLastFrame = false;

                if (leftDragging)
                {
                    leftDragging = false;
                }
                else
                {
                    // -- Do an actual click
                    AttemptToSelectNode();
                }
            }
            // -- left drag
            else if (Event.current.type == EventType.MouseDrag)
            {
                Debug.Log("Left Drag");
                leftDragging = true;
            }
        }


        // -- right click
        if (Event.current.button == 1)
        {
            if (Event.current.type == EventType.MouseDown && !rightDragging)
            {
                Debug.Log("Right Click");
                rightDownLastFrame = true;
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                Debug.Log("Right Click Up");
                rightDownLastFrame = false;

                if (rightDragging)
                {
                    rightDragging = false;
                }
                else
                {
                    // -- SHOW THE SPAWN MENU
                    ShowSpawnNodeMenu();
                }
            }
            // -- left drag
            else if (Event.current.type == EventType.MouseDrag)
            {
                Debug.Log("Right Drag");
                rightDragging = true;
            }
        }
    }


    void AttemptToSelectNode()
    {

    }


    bool draggingSelectionRect = false;
    /// <summary>
    /// Node Selection and Drag handler
    /// </summary>
    void HandleNodeDrag()
    {
        /*
        // -- TRY TO SELECT A NODE
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log("MOUSE DOWN");

            bool clickedOnANode = false;
            // -- GOT A CLICK
            for (int nLoop = 0; nLoop < move.nodes.Count; nLoop++)
            {
                Node n = move.nodes[nLoop];

                if (n.IsDisplayingComment)
                {
                    n.HideComment();
                }

                if ( n.NodeDrawPosition.Contains(Event.current.mousePosition) || n.NodeCommentPosition.Contains(Event.current.mousePosition) )
                {
                    clickedOnANode = true;
                    if (!n.IsSelected)
                    {
                        n.Select(true);
                    }
                }
            }
            if (!clickedOnANode)
            {
                foreach(Node n in move.nodes)
                {
                    n.Deselect();
                }
            }

            // -- SET SELECTION RECT INITIAL X,Y
            selectionStartPosition.x = Event.current.mousePosition.x;
            selectionStartPosition.y = Event.current.mousePosition.y;
        }
        // -- TRY TO DRAG A NODE...
        else if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {
            // -- ATTEMPT TO MOVE ALL NODES IN A SELECTION IF WE ARE NOT ALREADY
            bool haveASelectedNode = false;

            if (!draggingSelectionRect)
            {
                foreach (Node n in move.nodes)
                {
                    if (n.IsSelected)
                    {
                        haveASelectedNode = true;

                        n.NodeAbsolutePosition.x += Event.current.delta.x / scale;
                        n.NodeAbsolutePosition.y += Event.current.delta.y / scale;
                    }
                    n.UpdatePositionAndScale();
                }
            }

            // -- OTHERWISE DRAG A SELECTION WINDOW AROUND NODES
            if (!haveASelectedNode)
            {
                draggingSelectionRect = true;

                // -- SET SELECTION RECT INITIAL X,Y
                if (selectionStartPosition.x > Event.current.mousePosition.x)
                {
                    selectionRect.width = selectionStartPosition.x - Event.current.mousePosition.x;
                    selectionRect.x = Event.current.mousePosition.x;
                }
                else
                {
                    selectionRect.width = Event.current.mousePosition.x - selectionStartPosition.x;
                    selectionRect.x = selectionStartPosition.x;
                }

                if (selectionStartPosition.y > Event.current.mousePosition.y)
                {
                    selectionRect.height = selectionStartPosition.y - Event.current.mousePosition.y;
                    selectionRect.y = Event.current.mousePosition.y;
                }
                else
                {
                    selectionRect.height = Event.current.mousePosition.y - selectionStartPosition.y;
                    selectionRect.y = selectionStartPosition.y;
                }

                Debug.Log("SELECTION RECT : " + selectionRect);

                // -- IF INSIDE THE RECTANGLE, SELECT THE NODE
                foreach(Node n in move.nodes)
                {
                    if (selectionRect.Overlaps(n.NodeDrawPosition))
                    {
                        n.Select();
                    }
                    else
                    {
                        n.Deselect();
                    }
                }
            }
        }
        else if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            draggingSelectionRect = false;
            selectionRect = new Rect(0, 0, 0, 0);
        }
        */
    }


    void ShowSpawnNodeMenu()
    {
        GenericMenu menu = new GenericMenu();

        for (int i = 1; i < (int)NodeData.NodeType.COUNT; i++)
        {
            menu.AddItem( new GUIContent(((NodeData.NodeType)i).ToString()), true, AddNodeCallback, (NodeData.NodeType)i);
        }

        menu.ShowAsContext();
    }

    void AddNodeCallback(object userData)
    {
        NodeData.NodeType _type = (NodeData.NodeType)userData;

        switch (_type)
        {
            case NodeData.NodeType.None:
                // wut? shouldnt be possible
                break;

            case NodeData.NodeType.Physics:
             //   move.nodes.Add(new PhysicsNode(this, Event.current.mousePosition));
                break;

            case NodeData.NodeType.Position:
             //   move.nodes.Add(new PositionNode(this, Event.current.mousePosition));
                break;

            case NodeData.NodeType.Animation:
             //   move.nodes.Add(new AnimationNode(this, Event.current.mousePosition));
                break;
        }
    }

    /// <summary>
    /// CONTROLS RIGHT CLICK DRAG OF THE PINBOARD BACKGROUND
    /// </summary>
    void HandlePinBoardDragAndZoom()
    {
        // -- MOVE BOARD WITH RIGHT MOUSE BUTTON
        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            draggingPinBoard = true;

            mousePositionLastFrame = Event.current.mousePosition;
        }
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 1)
        {
            PinBoardOffset.x -= (mousePositionLastFrame.x - Event.current.mousePosition.x);
            PinBoardOffset.y -= (mousePositionLastFrame.y - Event.current.mousePosition.y);

            PinBoardDrawRect.width = PINBOARD_WIDTH * scale;
            PinBoardDrawRect.height = PINBOARD_HEIGHT * scale;

            PinBoardDrawRect.x = WINDOW_WIDTH * 0.5f - PinBoardDrawRect.width * 0.5f + PinBoardOffset.x * scale;
            PinBoardDrawRect.y = WINDOW_HEIGHT * 0.5f - PinBoardDrawRect.height * 0.5f + PinBoardOffset.y * scale;

            mousePositionLastFrame = Event.current.mousePosition;
        }
        if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
        {
            draggingPinBoard = false;
        }
    }


    /// <summary>
    /// UPDATES ALL DRAWN ELEMENTS ON THE PINBOARD TO ENSURE CORRECT POSITIONING AND SCALING
    /// </summary>
    //void UpdateElementDrawPositions()
    //{
    //    // -- RESIZE NODES
    //    for ( int nLoop = 0; nLoop < move.nodes.Count; nLoop++)
    //    {
    //    //    move.nodes[nLoop].UpdatePositionAndScale();
    //        //Node n = nodes[nLoop];
    //        ////n.NodeDrawPosition = new Rect(n.NodeAbsolutePosition.x * scale + PinBoardDrawRect.x,
    //        ////    n.NodeAbsolutePosition.y * scale + PinBoardDrawRect.y,
    //        ////    NODE_WIDTH * scale,
    //        ////    NODE_HEIGHT * scale);
    //    }

    //}


    /// <summary>
    /// DRAWS A GRID ON THE PIN BOARD
    /// </summary>
    public void DrawBackgroundGrid()
    {
        // -- DRAW A BOUNDS OF THE PINBOARD
        GUI.Box(PinBoardDrawRect, "");

        if (lineTex != null)
        {
            // -- DRAW LINES TO SHOW SCALE
            for (int x = 0; x < 11; x++)
            {
                GUI.DrawTexture(new Rect(PinBoardDrawRect.x + x * (PinBoardDrawRect.width / 10) - 1, PinBoardDrawRect.y, 2, PinBoardDrawRect.height), lineTex);
            }
            for (int y = 0; y < 11; y++)
            {
                GUI.DrawTexture(new Rect(PinBoardDrawRect.x, PinBoardDrawRect.y + y * (PinBoardDrawRect.height / 10) - 1, PinBoardDrawRect.width, 2), lineTex);
            }
        }
    }


    /// <summary>
    /// DRAWS THE NODES ON THE PINBOARD 
    /// </summary>
    public void DrawNodes()
    {
        for (int nLoop = 0; nLoop < move.nodes.Count; nLoop++)
        {
            Node n = move.nodes[nLoop];

            n.Draw();

            //// -- create texture for use in drawing this node...
            //Texture2D texture = new Texture2D(1, 1);

            //// -- DRAW BACKGROUND SELECTION IF SELECTED (YELLOW BOX BEHIND THE NODE)
            //if (n.IsSelected)
            //{
            //    texture.SetPixel(0, 0, Color.yellow);
            //    texture.Apply();
            //    GUI.DrawTexture(new Rect(n.NodeDrawPosition.x - 2, n.NodeDrawPosition.y - 2, n.NodeDrawPosition.width + 4, n.NodeDrawPosition.height + 4), texture);
            //}

            //// -- DRAW THE MAIN BODY OF THE NODE
            //texture.SetPixel(0, 0, n.color);
            //texture.Apply();
            //GUI.DrawTexture(n.NodeDrawPosition, texture);
            
            //// -- X BUTTON
            //if (GUI.Button(new Rect(n.NodeDrawPosition.x + n.NodeDrawPosition.width * 0.95f, n.NodeDrawPosition.y, n.NodeDrawPosition.width * 0.05f, n.NodeDrawPosition.height * 0.333f), "X"))
            //{
            //    move.nodes.Remove(n);
            //}
            
            //// -- SHOW THE NODES COMMENT BOX AREA
            //HandleCommentBox(n);

            //// -- CURVES
            //for (int cLoop = 0; cLoop < n.connections.Count; cLoop++)
            //{
            //    n.DrawFilledCurve(n.NodeDrawPosition, move.nodes[n.connections[cLoop].targetNodeIndex].NodeDrawPosition, n.connections[cLoop].m_fCurrentNodeExitTime, n.connections[cLoop].m_fNewNodeEnterTime, n.connections[cLoop].m_fTransitionDuration, Color.red);
            //}
        }
    }


    void HandleCommentBox(Node n)
    {
        if (n.IsSelected)
        {
            // -- little box to click that will open up the comment box proper, comments are displayed in absolute position and size
            if (!n.IsDisplayingComment)
            {
              //  if (GUI.Button( n.NodeCommentPosition, "..?"))
              //  {
              //      n.DisplayComment();
              //  }
            }
            else
            {
                // -- SHOW THE TEXT FIELD
              //  n.nodeComment = GUI.TextArea(n.NodeCommentPosition, n.nodeComment);

                // -- and a cross to close it
           //     if (GUI.Button( new Rect(n.NodeDrawPosition.x , n.NodeDrawPosition.y + n.NodeDrawPosition.height * 5.0f, n.NodeDrawPosition.width * 0.1f, n.NodeDrawPosition.height * 0.33f ), "X"))
           //     {
           //         n.HideComment();
           //     }
            }
        }
    }


    /*
    /// <summary>
    /// THE NODE CLASS
    /// </summary>
    public class Node
    {
        public ToolWindow toolWindow;

        public Node()
        {
            NodeAbsolutePosition = new Rect(100 * scale, 100 * scale, NODE_WIDTH * scale, NODE_HEIGHT * scale);
            NodeDrawPosition = new Rect(100 * scale, 100 * scale, NODE_WIDTH * scale, NODE_HEIGHT * scale);
        }

        public Rect NodeAbsolutePosition;
        public Rect NodeDrawPosition;
        public Rect NodeCommentPosition ;

        public Color color = Color.gray;

        public AnimationClip animClip;

        // -- editable comment on the node
        public string nodeComment = "Write Node Comment Here...";
        private bool isDisplayingComment = false;
        public bool IsDisplayingComment
        {
            get { return isDisplayingComment; }
        }
        public void DisplayComment()
        {
            isDisplayingComment = true;
            // -- change the size for displaying the whole text field
            NodeCommentPosition = new Rect(NodeDrawPosition.x, NodeDrawPosition.y - 120, NodeAbsolutePosition.width, 100);
        }
        public void HideComment()
        {
            isDisplayingComment = false;
        }

        // -- node selection toggle
        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
        }
        public void Select(bool deselectOtherNodes = false)
        {
            if (deselectOtherNodes)
            {
                foreach(Node n in tool.Nodes)
                {

                }
            }
            isSelected = true;
        }
        public void Deselect()
        {
            isSelected = false;
            // -- hide comment if open
            isDisplayingComment = false;
        }

        public void UpdatePositionAndScale()
        {
            NodeDrawPosition = new Rect(NodeAbsolutePosition.x * scale + PinBoardDrawRect.x,
            NodeAbsolutePosition.y * scale + PinBoardDrawRect.y,
            NODE_WIDTH * scale,
            NODE_HEIGHT * scale);

            Debug.Log("Node Draw Position : " + NodeDrawPosition.ToString());

            if (!IsDisplayingComment)
            {
                NodeCommentPosition = new Rect(NodeDrawPosition.x, NodeDrawPosition.y - 25, 25, 15);
            }

            Debug.Log("Node Comment Position : " + NodeCommentPosition.ToString());

        }

        public List<NodeConnection> connections = new List<NodeConnection>();
    }

    public class NodeConnection
    {
        public float startPos = 0.0f;
        public float endPos = 0.5f;
        public float duration = 0.4f;
        public int targetNodeIndex = 0;

        public NodeConnection(int newIndex)
        {
            targetNodeIndex = newIndex;
        }
    }

    void DrawFilledCurve(Rect startNodeRect, Rect endNodeRect, float startX, float endX, float duration, Color _color, int thickness = 2, string label = "ERROR")
    {
        // -- Draw Label / Tag for curve
        Rect labelRect = new Rect(endNodeRect.x + (endX * endNodeRect.width) + (duration * endNodeRect.width) + 10, endNodeRect.y - 25, 100, 20);

        GUI.Box(labelRect, label);

        // -- DRAW CURVES
        for (int x = 0; x < (int)(duration * startNodeRect.width) + 1; x++)
        {
            float originPointX = startNodeRect.x + startX * startNodeRect.width;
            float destinationPointStartX = endNodeRect.x + endX * endNodeRect.width;

            Vector3 startPos = new Vector3(originPointX, startNodeRect.y + startNodeRect.height - 1, 0);
            Vector3 endPos = new Vector3(destinationPointStartX, endNodeRect.y + 1, 0);

            Vector3 startTan = startPos + Vector3.up * 25;
            Vector3 endTan = endPos + Vector3.down * 25;

            Handles.DrawBezier(startPos, endPos, startTan, endTan, _color, null, 1.5f);
        }
    }
    */
}
