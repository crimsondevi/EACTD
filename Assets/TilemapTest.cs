using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapTest : MonoBehaviour
{
    public Tile tb;
    public TileBase test;
    
    // Start is called before the first frame update
    void Start()
    {
         tb = ScriptableObject.CreateInstance<Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mpInt = new Vector3Int(Mathf.RoundToInt(mp.x), Mathf.RoundToInt(mp.y), 0);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.right);
            print(hit.transform.gameObject);
            Tilemap tilemap = hit.transform.gameObject.GetComponent<Tilemap>();
            test = tilemap.GetTile(mpInt);
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.right);
        //    Tilemap tilemap = hit.transform.gameObject.GetComponent<Tilemap>();
        //    print(tilemap.GetTile(mpInt));
        //    tb.sprite = tilemap.GetSprite(mpInt);
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.right);
        //    Tilemap tilemap = hit.transform.gameObject.GetComponent<Tilemap>();
        //    
        //    print("setTile");
        //    tilemap.SetTile(mpInt, tb);
        //}
    }
}
