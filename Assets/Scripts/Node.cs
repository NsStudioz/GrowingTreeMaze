using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;

    private void Awake()
    {
        SetNodeInvisible();
        InitializeNodeWalls();
    }

    private void InitializeNodeWalls()
    {
        walls = new GameObject[transform.childCount];

        for (int i = 0; i < walls.Length; i++)
            walls[i] = transform.GetChild(i).gameObject;
    }

    public void SetNodeVisible()
    {
        gameObject.SetActive(true);
    }

    public void SetNodeInvisible()
    {
        gameObject.SetActive(false);
    }
}
