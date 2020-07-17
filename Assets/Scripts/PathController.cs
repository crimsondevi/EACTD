using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{

    public List<MapNode> Positions;
    public MapNode start;
    public MapNode end;

    public MapNode nextMapNode(int index)
    {
        if (index == Positions.Count - 1)
            return null;
        else
        {
            return Positions[index + 1];
        }
    }

    public void Start()
    {
        start = Positions[0];
        end = Positions[Positions.Count - 1];
    }
}