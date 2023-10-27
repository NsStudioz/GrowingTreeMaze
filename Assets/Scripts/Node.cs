using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;

    public bool IsVisited {  get; private set; }

    [SerializeField] private bool visited = false;

    public void SetNodeVisit()
    {
        visited = true;
    }

    public bool GetNodeVisit()
    {
        return visited;
    }

    private void Awake()
    {
        SetNodeInvisible();
        ResetWallsState();
    }

    private void ResetWallsState()
    {
        foreach (GameObject wall in walls)
        {
            if (!wall.gameObject.activeInHierarchy)
                wall.gameObject.SetActive(true);
        }
    }

    #region PublicMethods:

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

    #endregion
}
