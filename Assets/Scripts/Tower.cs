using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class  Tower : MonoBehaviour
{
    public MapNode mapNode;

    public string name;
    
    public float Range;
    public CircleCollider2D _circleCol;
    public RadiusIndicator _radiusIndicator;

    public void Initialize(MapNode mapNode)
    {
        this.mapNode = mapNode;
    }

    public void Start()
    {
        _circleCol = GetComponent<CircleCollider2D>();
        _radiusIndicator = gameObject.GetComponentInChildren<RadiusIndicator>();
        _circleCol.radius = Range;
        _radiusIndicator.SetRange(Range);
    }
}