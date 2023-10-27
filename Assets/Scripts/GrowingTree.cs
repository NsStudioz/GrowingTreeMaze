using PerfectMazeProject.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingTree : MonoBehaviour
{
    [SerializeField] private MazeGrid grid;

    [SerializeField] private List<Node> spawnedNodes = new List<Node>();
    [SerializeField] private List<Node> nodes = new List<Node>();

    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeHeight;

    [SerializeField] private bool isQuickestGenerate;
    [SerializeField] private float generationSpeed = 0.01f;

    private void Start()
    {
        spawnedNodes = grid.GetNodeGrid();

        for (int i = 0; i < spawnedNodes.Count; i++)
            spawnedNodes[i].SetNodeVisible();
    }


}
