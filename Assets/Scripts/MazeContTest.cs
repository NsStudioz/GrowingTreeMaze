using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeContTest : MonoBehaviour
{
    public Node nodePrefab;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    void Start()
    {
        InitializeMazeGrid();
        DrawMazeGrid();
    }

    private void InitializeMazeGrid()
    {
        MazeGrid mazeGrid = new MazeGrid(gridWidth, gridHeight);
    }

    private void DrawMazeGrid()
    {
        for (int x = 0; x < gridWidth; x++)
            for (int z = 0; z < gridHeight; z++)
            {
                Node newNode = SpawnNewCellInstance(nodePrefab, SetNodePosition(x, z));
                SetNodeAsChildToParent(newNode, transform);
            }
    }

    private Node SpawnNewCellInstance(Node prefab, Vector3 cellPos)
    {
        return Instantiate(prefab, cellPos, Quaternion.identity);
    }

    private Vector3 SetNodePosition(int x, int z)
    {
        Vector3 nodePos = new Vector3(x, 0, z);
        return nodePos;
    }

    private void SetNodeAsChildToParent(Node prefab, Transform parent)
    {
        prefab.transform.parent = parent;
    }

}