using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;

    public bool IsVisited {  get; private set; }

    #region PublicMethods:
    
    /// <summary>
    /// Show all node's walls
    /// </summary>
    public void ResetWallsState()
    {
        foreach (GameObject wall in walls)
            if (!wall.gameObject.activeInHierarchy)
                wall.gameObject.SetActive(true);
    }

    /// <summary>
    /// mark node as visited
    /// </summary>
    public void SetNodeVisited()
    {
        IsVisited = true;
    }

    /// <summary>
    /// Hide a wall from the node
    /// </summary>
    /// <param name="index"></param>
    public void RemoveWall(int index)
    {
        walls[index].SetActive(false);
    }

    #endregion
}
