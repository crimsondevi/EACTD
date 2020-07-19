using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{

    public List<MapNode> Positions;
    public MapNode start;
    public MapNode end;

    public MapNode nextMapNode(MapNode node)
    {
        return node.child;
    }

    public void Start()
    {
        start = Positions[0];
        end = Positions[Positions.Count - 1];
    }
}