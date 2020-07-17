using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GathererTower : Tower
{

    public GameController gameController;
    public GameGrid gameGrid;

    public float gatherValue;
    public float gatherTime;

    private float cooldown;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameGrid = GameObject.Find("Map").GetComponent<GameGrid>();
    }

    private void Start()
    {
        base.Start();
        cooldown = gatherTime;
    }

    void Update()
    {
        _circleCol.radius = Range;
        _radiusIndicator.SetRange(Range);
        GatherMana();
    }

    public void GatherMana()
    {
        if (cooldown >= 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            gameController.mana += gatherValue;
            cooldown = gatherTime;
        }
        
    }
}
