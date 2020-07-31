using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float health;
    public float speed;
    
    private PathController _pathController;
    private int index = 0;
    
    public MapNode destination;
    
    private Rigidbody2D rb;

    public void Initialize(int health, float speed, MapNode start)
    {
        this.health = health;
        this.speed = speed;
        destination = start;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PathMovement();
    }

    public void PathMovement()
    {
        if (destination != null)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, destination.transform.position, speed * Time.deltaTime);
            Vector2 direction = (destination.transform.position - this.transform.position);
            if (direction.magnitude <= 0.1)
            {
                destination = destination.child;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            //TODO: Handle reached destination
        }
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}