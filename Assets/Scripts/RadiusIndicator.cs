using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusIndicator : MonoBehaviour
{
    public float range = 5;
    public int segments = 50;
    public float width = 0.1f;
    private LineRenderer _lineRenderer;

    public void SetRange(float range)
    {
        this.range = range;
    }

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        InitRenderer();
    }

    public void InitRenderer()
    {
        Color c1 = Color.cyan;
        _lineRenderer.startColor = _lineRenderer.endColor = c1;
        _lineRenderer.startWidth = _lineRenderer.endWidth = width;
        _lineRenderer.positionCount = segments + 1;
        _lineRenderer.useWorldSpace = false;
    }

    public void DoRenderer()
    {
        _lineRenderer.startWidth = _lineRenderer.endWidth = width;
        _lineRenderer.positionCount = segments + 1;

        float deltaTheta = (float) (2.0 * Mathf.PI) / segments;
        float theta = 0f;

        for (int i = 0; i < segments + 1; i++)
        {
            float x = range * Mathf.Cos(theta);
            float y = range * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, 0);
            _lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DoRenderer();
    }
}