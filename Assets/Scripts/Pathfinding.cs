using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Modular;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public MapDataVariable mapDataVariable;
    private MapData mapData;

    public MapNodeListVariable startNodeListVariable;
    
    private Queue<MapNode> mapNodesQueue;
    private bool[,] visited;
    
    public MapNodeVariable StartNode;

    private void Start()
    {
        mapData = mapDataVariable.Value;
        visited = new bool[mapData.Width, mapData.Height];
        BFS(StartNode.Value);
        
    }

    public void BFS(MapNode startNode)
    {
        for (int y = 0; y < mapData.Height; y++)
        {
            for (int x = 0; x < mapData.Width; x++)
            {
                visited[x, y] = false;
            }
        }
        
        mapNodesQueue = new Queue<MapNode>();
        visited[startNode.xIndex, startNode.yIndex] = true;
        mapNodesQueue.Enqueue(startNode);
        while (mapNodesQueue.Any())
        {
            MapNode mapNode = mapNodesQueue.Dequeue();
            foreach (MapNode node in mapNode.neighbours)
            {
                if (!visited[node.xIndex, node.yIndex] && node.isPath)
                {
                    visited[node.xIndex, node.yIndex] = true;
                    node.child = mapNode;
                    mapNodesQueue.Enqueue(node);
                }
            }
        }
    }

}
