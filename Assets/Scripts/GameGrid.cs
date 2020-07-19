using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Modular;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GameGrid : MonoBehaviour
{
    public int mapWidth;
    
    public GameObject nodePrefab;

    public LineRenderer lr;
    
    [SerializeField] public MapNode[,] _mapNodes;
    public MapNode selectedNode;
    public MapNode hoveredNode;
    public MapNode prevHoveredNode;

    public MapNodeVariable StartNode;
    public MapNodeVariable EndNode;
    
    public LayerMask layerMask;

    public TextAsset jsonFile;

    private IEnumerator tooltipRoutine;
    public GameObject ToolTip;


    void Start()
    {
        GenerateFromJson();
        UpdateMap();
    }
    void Update()
    {
        hoveredNode = GetMapNode();
        
        if (hoveredNode != null)
        {
            LineRendererHover(hoveredNode.transform);
            
            TooltipControl();

            prevHoveredNode = hoveredNode;
        }
        else
        {
            LineRenderEndHover();
            if (tooltipRoutine != null)
            {
                StopCoroutine(tooltipRoutine);
                ToolTip.SetActive(false);
                tooltipRoutine = null;
            }
          
        }
    }

    IEnumerator TooltipDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToolTip tooltip = ToolTip.gameObject.GetComponent<ToolTip>();
        tooltip.name.text = "Name of Occupant";
        tooltip.info.text = "Different info";
        ToolTip.SetActive(true);
        
    }

    private void TooltipControl()
    {
        ToolTip.transform.position = Input.mousePosition;
        if (tooltipRoutine == null)
        {
            tooltipRoutine = TooltipDelay(1f);
            StartCoroutine(tooltipRoutine);
        } 
        else if (prevHoveredNode != hoveredNode)
        {
            if (tooltipRoutine != null)
            {
                StopCoroutine(tooltipRoutine);
                ToolTip.SetActive(false);
            }
            tooltipRoutine = TooltipDelay(1f);
            StartCoroutine(tooltipRoutine);
        }
    }
    
    public void LineRendererHover(Transform mapNodeTransform)
    {
        InitRenderer();
        Vector3 position = mapNodeTransform.position - new Vector3(0.5f, 0.5f, 0);
        lr.SetPosition(0, position);
        lr.SetPosition(1, position + Vector3.right);
        lr.SetPosition(2, position + Vector3.right + Vector3.up);
        lr.SetPosition(3, position + Vector3.up);
        lr.SetPosition(4, position);
    }

    public void LineRenderEndHover()
    {
        lr.positionCount = 0;
    }
    
    public void InitRenderer()
    {
        Color c1 = Color.cyan;
        lr.startColor = lr.endColor = c1;
        lr.startWidth = lr.endWidth = 0.1f;
        lr.positionCount = 5;
        lr.useWorldSpace = false;
    }
    
    public void GenerateMap()
    {
        if (_mapNodes != null)
        {
            ClearMap();
        }
        else
        {
            SetMapNodes();
            ClearMap();
        }

        _mapNodes = new MapNode[mapWidth, mapWidth];
        for (int y = 0; y < mapWidth; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                MapNode mapNode = Instantiate(nodePrefab).GetComponent<MapNode>();
                mapNode.sr = mapNode.GetComponent<SpriteRenderer>();
                mapNode.defaultColor = mapNode.isPath ? Color.grey : Color.white;
                mapNode.transform.SetParent(this.gameObject.transform);
                mapNode.Initialize(x, y);
                _mapNodes[x, y] = mapNode;
            }
        }
        AssignNeighbours();
    }
    public void GenerateFromJson()
    {
        if (_mapNodes != null)
        {
            ClearMap();
        }
        else
        {
            SetMapNodes();
            ClearMap();
        }

        MapData mapData = JsonUtility.FromJson<MapData>(jsonFile.text);
        mapWidth = mapData.Dimension;
        
        _mapNodes = new MapNode[mapWidth, mapWidth];

        for (int y = 0; y < mapWidth; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                MapNode mapNode = Instantiate(nodePrefab).GetComponent<MapNode>();
                mapNode.sr = mapNode.GetComponent<SpriteRenderer>();
                if (mapData.NodeTypes[y * mapWidth + x] == -1)
                {
                    mapNode.isPath = true; 
                    StartNode.SetValue(mapNode);
                }
                if (mapData.NodeTypes[y * mapWidth + x] == -2)
                {
                    mapNode.isPath = true; 
                    EndNode.SetValue(mapNode);
                }
                if (mapData.NodeTypes[y * mapWidth + x] == 1)
                {
                    mapNode.isPath = true;
                }
                mapNode.defaultColor = mapNode.isPath ? Color.grey : Color.white;
                mapNode.transform.SetParent(this.gameObject.transform);
                mapNode.Initialize(x, y); 
                _mapNodes[x, y] = mapNode;
            }
        }
        
        AssignNeighbours();

    }
    private void ClearMap()
    {
        for (int y = 0; y < _mapNodes.GetLength(0); y++)
        {
            for (int x = 0; x < _mapNodes.GetLength(1); x++)
            {
                if (_mapNodes[x, y] != null)
                {
                    DestroyImmediate(_mapNodes[x, y].gameObject);
                    _mapNodes[x, y] = null;
                }
            }
        }
    }

    public void AssignNeighbours()
    {
        for (int y = 0; y < mapWidth; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (y == 0 && x == 0)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x + 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y + 1]);
                } else if (y == 0 && x == mapWidth - 1)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x - 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y + 1]);
                } else if (y == mapWidth - 1 && x == 0)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x + 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y - 1]);
                } else if (y == mapWidth - 1 && x == mapWidth - 1)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x - 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y - 1]);
                } else if (y == 0)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x + 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x - 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y + 1]);
                } else if (y == mapWidth - 1)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x + 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x - 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y - 1]);
                } else if (x == 0)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x + 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y + 1]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y - 1]);
                } else if (x == mapWidth - 1)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x - 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y + 1]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y - 1]);
                }
                else
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x + 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x - 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y + 1]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y - 1]);
                }
                
            }
        }
    }

    public void SaveMap()
    {
        
    }

    public void LoadMap()
    {
        
    }
    public void SetMapNodes()
    {
        _mapNodes = new MapNode[mapWidth, mapWidth];
        for (int y = 0; y < _mapNodes.GetLength(0); y++)
        {
            for (int x = 0; x < _mapNodes.GetLength(1); x++)
            {
                    _mapNodes[x, y] = transform.GetChild((y * _mapNodes.GetLength(0)) + x).GetComponent<MapNode>();
            }
        }
    }
    public void UpdateMap()
    {
        if (_mapNodes == null)
            SetMapNodes();
        for (int y = 0; y < _mapNodes.GetLength(0); y++)
        {
            for (int x = 0; x < _mapNodes.GetLength(1); x++)
            {
                if (_mapNodes[x, y].isPath)
                    _mapNodes[x, y].defaultColor = Color.grey;
                else
                    _mapNodes[x, y].defaultColor = Color.white;
                _mapNodes[x, y].sr.color = _mapNodes[x, y].defaultColor;
            }
        }
        
        /*
        for (int y = 0; y < _mapNodes.GetLength(0); y++)
        {
            for (int x = 0; x < _mapNodes.GetLength(1); x++)
            {
                if (_mapNodes[x, y].isPath)
                {
                    _mapNodes[x, y].defaultColor = Color.grey;
                    _mapNodes[x, y].sr.color = _mapNodes[x, y].defaultColor;
                }
            }
        }
        */
    }
    public MapNode GetMapNode()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit  = Physics2D.Raycast(mp, Vector2.right,1.5f, layerMask);
        if (hit.collider != null)
        {
            return hit.transform.gameObject.GetComponent<MapNode>();
        }
        return null;
    }
}
