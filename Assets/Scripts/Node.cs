using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;

    public bool IsVisited {  get; private set; }

    private void Awake()
    {
        SetNodeInvisible();
        ResetWallsState();
    }

    public void SetNodeVisited()
    {
        IsVisited = true;
    }
    public void SetNodeUnvisited()
    {
        IsVisited = false;
    }

    public void SetNodeVisible()
    {
        gameObject.SetActive(true);
    }

    public void SetNodeInvisible()
    {
        gameObject.SetActive(false);
    }

    public void RemoveWall(int index)
    {
        walls[index].SetActive(false);
    }

    private void ResetWallsState()
    {
        foreach (GameObject wall in walls)
        {
            if (!wall.gameObject.activeInHierarchy)
                wall.gameObject.SetActive(true);
        }
    }
}
