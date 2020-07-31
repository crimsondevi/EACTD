using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Modular;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Queue<MapNode> mapNodesQueue;
    private bool[,] visited = new bool[20,20];
    
    public MapNodeVariable StartNode;

    private void Start()
    {
        BFS(StartNode.Value);
    }

    public MapNode BFS(MapNode startNode)
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                visited[i, j] = false;
            }
        }
        
        mapNodesQueue = new Queue<MapNode>();
        visited[startNode.xIndex, startNode.yIndex] = true;
        mapNodesQueue.Enqueue(startNode);
        while (mapNodesQueue.Any())
        {
            MapNode mapNode = mapNodesQueue.Dequeue();
            if (mapNode.goal)
            {
                return mapNode;
            }
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
        return null;
    }

}
