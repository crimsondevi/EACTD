using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MapNode : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    public bool goal;
    public MapNode child;
    public List<MapNode> neighbours = new List<MapNode>();
    
    public SpriteRenderer sr;
    public GameObject occupant;

    public bool isPath;

    public bool hovered = false;

    public Color defaultColor = Color.white;
        
    public void Initialize(int x, int y)
    {
        xIndex = x;
        yIndex = y;
        this.transform.position = new Vector3(xIndex + 0.5f, yIndex + 0.5f, 0);
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (isPath)
        {
            defaultColor = Color.grey;
        }
    }

    void Update()
    {
        //if (hovered)
        //    Hover();
        //else UnHover();
    }

    public void setOccupant(GameObject tower)
    {
        occupant = tower;
        occupant.transform.position = this.transform.position;
    }

    public void Hover()
    {
        sr.color = Color.magenta;
    }

    public void UnHover()
    {
        sr.color = defaultColor;
    }

}