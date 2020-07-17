using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using EditorScripts;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DamageTower : Tower
{
    public List<GameObject> Targets = new List<GameObject>();

    public float Damage;
    public float firerate;
    
    public float cooldown; //Fire rate;
    
    void Start()
    {
        base.Start();
        cooldown = firerate;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Targets.Add(other.gameObject);
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "enemy")
        {
            Targets.Remove(other.gameObject);
        }
    }
}
