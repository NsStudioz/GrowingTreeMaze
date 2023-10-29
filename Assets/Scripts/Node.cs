using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;

    [SerializeField] Renderer rendererGround;

    public bool IsVisited {  get; private set; }

    private void Awake() => SetNodeInitalColor();

    /// <summary>
    /// Reset node color
    /// </summary>
    private void SetNodeInitalColor()
    {
        rendererGround.sharedMaterial.color = Color.white;
    }

    /// <summary>
    /// Visited node will change color
    /// </summary>
    private void SetNodeVisitedColor()
    {
        rendererGround.material.color = Color.yellow;
    }

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
        SetNodeVisitedColor();
    }

    /// <summary>
    /// Hide a wall from the node
    /// </summary>
    /// <param name="index"></param>
    public void RemoveWall(int index)
    {
        walls[index].SetActive(false);
    }

    // UX:
    public void SetNodeCompleteColor()
    {
        rendererGround.sharedMaterial.color = Color.green;
    }

    /// <summary>
    /// Relative node found but has not been visited yet.
    /// </summary>
    public void SetNodeRelativeFoundColor()
    {
        rendererGround.material.color = Color.red;
    }

    #endregion




}
