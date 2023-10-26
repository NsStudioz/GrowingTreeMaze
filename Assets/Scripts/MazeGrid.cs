using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGrid : MonoBehaviour
{
    public int width;
    public int height;

    public MazeGrid(int width, int height, Transform parent, Node prefab)
    {
        this.width = width;
        this.height = height;

        // using x & z Axis's since we are generating a maze in 3D:
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
            {
                SpawnNewCellInstance(prefab, SetNodePosition(x, z));
                SetNodeAsChildToParentGameObject(prefab, parent);
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

    private void SetNodeAsChildToParentGameObject(Node prefab, Transform parent)
    {
        prefab.transform.parent = parent;
    }
}
