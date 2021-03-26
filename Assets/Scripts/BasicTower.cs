using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BasicTower : DamageTower, IDealDamage
{
    void Update()
    {
        _circleCol.radius = Range;
        _radiusIndicator.SetRange(Range);
        if (Targets.Count > 0)
        {
            if (cooldown >= 0)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                DealDamage();
                cooldown = firerate;
            }
        }
    }

    public void Start()
    {
        base.Start();
        name = "Basic Tower";
    }
    
    public void DealDamage()
    {
        IDamageable damageable = (IDamageable)Targets[0].GetComponent(typeof(IDamageable));
        damageable.TakeDamage(Damage);
        
    }
}
