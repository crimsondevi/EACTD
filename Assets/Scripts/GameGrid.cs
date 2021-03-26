using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Modular;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GameGrid : MonoBehaviour
{
    public MapDataVariable mapDataVariable;
    private MapData mapData;
    
    public MapNodeListVariable startNodeListVariable;
    
    public GameObject nodePrefab;

    public LineRenderer lr;
    
    private MapNode[,] _mapNodes;
    public MapNode selectedNode;
    public MapNodeVariable hoveredNode;
    public MapNode prevHoveredNode;

    public MapNodeVariable StartNode;
    public MapNodeVariable EndNode;
    
    public LayerMask layerMask;

    public TextAsset jsonFile;

    private IEnumerator tooltipRoutine;
    public GameObject ToolTip;
    public ToolTip toolTipScript;
    public float toolTipDelay = 0.4f;

    public bool toolTipOn = true;
    
    
    private void Start()
    {
        mapData = mapDataVariable.Value;

        toolTipScript = ToolTip.gameObject.GetComponent<ToolTip>();
        ToolTip.SetActive(false);
        
        GenerateFromJson();
        UpdateMap();

        transform.position = new Vector3(-mapData.Width / 2, -mapData.Height / 2, 0);
    }
    
    private void Update()
    {
        hoveredNode.SetValue(GetMapNode());

        if (hoveredNode.Value != null)
        {
            LineRendererHover(hoveredNode.Value.transform);
            
            if (toolTipOn)
                TooltipControl();

            prevHoveredNode = hoveredNode.Value;
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
        string name = "";
        switch ((int) hoveredNode.Value.affinity)
        {
            case 0:
                name = "Abyssal Tile";
                break;
            case 1:
                name = "Infernal Tile";
                break;
            case 2:
                name = "Primordial Tile";
                break;
        }

        string info = "";
        if (hoveredNode.Value.isOccupied)
        {
            info = "Is Occupied by " + hoveredNode.Value.occupant.GetComponent<Tower>().name;
        }
        else
        {
            info = "Tile is Unoccupied";
        }
        
        toolTipScript.name.text = name;
        toolTipScript.info.text = info;
        
        ToolTip.SetActive(true);
    }

    private void TooltipControl()
    {
        ToolTip.transform.position = Input.mousePosition;
        if (tooltipRoutine == null)
        {
            tooltipRoutine = TooltipDelay(toolTipDelay);
            StartCoroutine(tooltipRoutine);
        } 
        else if (prevHoveredNode != hoveredNode.Value)
        {
            if (tooltipRoutine != null)
            {
                StopCoroutine(tooltipRoutine);
                ToolTip.SetActive(false);
            }
            tooltipRoutine = TooltipDelay(toolTipDelay);
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
        lr.useWorldSpace = true;
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

        _mapNodes = new MapNode[mapData.Width, mapData.Height];
        for (int y = 0; y < mapData.Height; y++)
        {
            for (int x = 0; x < mapData.Width; x++)
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
            if (this.transform.childCount > 0)
            {
                SetMapNodes();
                ClearMap();
            }
        }
        
        int mapWidth = mapData.Width;
        int mapHeight = mapData.Height;
        
        _mapNodes = new MapNode[mapWidth, mapHeight];
        List<MapNode> _startNodes = new List<MapNode>();
        
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                MapNode mapNode = Instantiate(nodePrefab).GetComponent<MapNode>();
                mapNode.sr = mapNode.GetComponent<SpriteRenderer>();
                if (mapData.NodeTypes[y * mapWidth + x] == -1)
                {
                    mapNode.isPath = true;
                    mapNode.defaultColor = Color.white;
                    _startNodes.Add(mapNode);
                }
                if (mapData.NodeTypes[y * mapWidth + x] == -2)
                {
                    mapNode.isPath = true; 
                    mapNode.defaultColor = Color.black;
                    EndNode.SetValue(mapNode);
                }
                if (mapData.NodeTypes[y * mapWidth + x] == 1)
                {
                    mapNode.isPath = true;
                    mapNode.defaultColor = Color.gray;
                }
                mapNode.transform.SetParent(this.gameObject.transform);
                mapNode.Initialize(x, y); 
                _mapNodes[x, y] = mapNode;
            }
        }
        startNodeListVariable.SetValue(_startNodes);
        AssignNeighbours();

    }
    private void ClearMap()
    {
        for (int y = 0; y < _mapNodes.GetLength(1); y++)
        {
            for (int x = 0; x < _mapNodes.GetLength(0); x++)
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
        int mapWidth = mapData.Width;
        int mapHeight = mapData.Height;
        
        for (int y = 0; y < mapHeight; y++)
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
                } else if (y == mapHeight - 1 && x == 0)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x + 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y - 1]);
                } else if (y == mapHeight - 1 && x == mapWidth - 1)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x - 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y - 1]);
                } else if (y == 0)
                {
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x + 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x - 1, y]);
                    _mapNodes[x, y].neighbours.Add(_mapNodes[x, y + 1]);
                } else if (y == mapHeight - 1)
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
        //TODO: implement save map
    }

    public void LoadMap()
    {
        
    }
    
    public void SetMapNodes()
    {    
        _mapNodes = new MapNode[mapData.Width, mapData.Height];
        for (int y = 0; y < _mapNodes.GetLength(1); y++)
        {
            for (int x = 0; x < _mapNodes.GetLength(0); x++)
            {
                    _mapNodes[x, y] = transform.GetChild((y * _mapNodes.GetLength(0)) + x).GetComponent<MapNode>();
            }
        }
    }
    public void UpdateMap()
    {
        if (_mapNodes == null)
            SetMapNodes();
        for (int y = 0; y < _mapNodes.GetLength(1); y++)
        {
            for (int x = 0; x < _mapNodes.GetLength(0); x++)
            {
                /*
                if (_mapNodes[x, y].isPath)
                    _mapNodes[x, y].defaultColor = Color.grey;
                else
                    _mapNodes[x, y].defaultColor = Color.white;
                */
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
        RaycastHit2D hit  = Physics2D.Raycast(mp, Vector2.right,0.01f, layerMask);
        if (hit.collider != null)
        {
            return hit.transform.gameObject.GetComponent<MapNode>();
        }
        return null;
    }
}