using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeTile : Tile
{
    public List<NodeTile> neighbors = new List<NodeTile>();

    public GameObject occupant;

}
