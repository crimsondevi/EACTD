using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float mana;

    public List<GameObject> towers;

    public Text gameState;
    public Text buildOptions;
    public Text selectedOption;
    public Text resources;

    public GameGrid gameGrid;


    public enum State
    {
        Idle,
        Build
    }

    public enum Select
    {
        Empty,
        Tower,
        Tower2,
        Tower3
    }

    public State currentState;
    public Select currentSelect;

    private void Awake()
    {
        gameState = GameObject.Find("Game State").GetComponent<Text>();
        buildOptions = GameObject.Find("Build Options").GetComponent<Text>();
        selectedOption = GameObject.Find("Selected Option").GetComponent<Text>();
        resources = GameObject.Find("Resources").GetComponent<Text>();

    }

    private void Start()
    {
        currentState = State.Idle;
        currentSelect = Select.Empty;
        mana = 0;
    }

    void Update()
    {
        gameGrid.selectedNode = gameGrid.GetMapNode();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentState = State.Build;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentState = State.Idle;
            currentSelect = Select.Empty;
        }

        if (currentState == State.Build)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                currentSelect = Select.Tower;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                currentSelect = Select.Tower2;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                currentSelect = Select.Tower3;

            if (Input.GetMouseButtonDown(0))
            {
                InstantiateTurret(towers[(int) currentSelect - 1]);
            }
        }

        gameState.text = "Current State: " + currentState;
        resources.text = "Mana: " + mana;

        if (currentState == State.Build)
        {
            buildOptions.text = "Build Options \n" +
                                "Press 1 for Tower 1 \n" +
                                "Press 2 Tower 2 \n" +
                                "Press 3 Tower 3";
            selectedOption.text = "Selected Options: " + currentSelect;
        }
        else
        {
            buildOptions.text = selectedOption.text = "";
        }
    }

    public void InstantiateTurret(GameObject towerPrefab)
    {
        gameGrid.selectedNode = gameGrid.GetMapNode();
        if (!gameGrid.selectedNode.isPath && gameGrid.selectedNode != null)
        {
            GameObject tower = Instantiate(towerPrefab);
            Tower t = tower.GetComponent<Tower>();
            t.Initialize(gameGrid.selectedNode);
            gameGrid.selectedNode.setOccupant(tower);
        }
    }
}