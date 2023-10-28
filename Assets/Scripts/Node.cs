using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;

    public bool IsVisited {  get; private set; }

    #region PublicMethods:

    public void ResetWallsState()
    {
        foreach (GameObject wall in walls)
            if (!wall.gameObject.activeInHierarchy)
                wall.gameObject.SetActive(true);
    }

    public void SetNodeVisited()
    {
        IsVisited = true;
    }

    public void RemoveWall(int index)
    {
        walls[index].SetActive(false);
    }

    #endregion
}
