using PerfectMazeProject.DirectionExtensions;
using PerfectMazeProject.Grid;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class GrowingTree : MonoBehaviour
{
    [SerializeField] private List<Node> SpawnedNodes = new List<Node>();
    [SerializeField] private List<Node> Nodes = new List<Node>();

    NextNodeMode nextNode;

    [SerializeField] private MazeGrid grid;
    [SerializeField] float timer = 2f;

    private enum NextNodeMode
    {
        Random,
        Newest,
        Split // 50/50
    }

    [SerializeField] private NextNodeMode mode = NextNodeMode.Newest;

    private void Start()
    {
        SpawnedNodes = grid.GetNodeGrid();

        for (int i = 0; i < SpawnedNodes.Count; i++)
            SpawnedNodes[i].SetNodeVisible();

        // Choosing a node at start, adding it to list and marking it as visited:
        int chosenNodeIndex = Random.Range(0, SpawnedNodes.Count);
        Nodes.Add(SpawnedNodes[chosenNodeIndex]);
        Nodes[0].SetNodeVisit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(SimulateMazeCoroutine());
    }

    private IEnumerator SimulateMazeCoroutine()
    {
        // while the list is not empty:
        while (Nodes.Count > 0)
        {
            List<Direction> unvisitedDirections = new List<Direction>();
            List<int> possibleRelativesInt = new List<int>();

            int index = GetNextIndex();

            int chosenNodeIndex = SpawnedNodes.IndexOf(Nodes[index]);

            Debug.Log("Chosen Node Index: " + chosenNodeIndex);
            // Check relatives:
            FindRelatives(chosenNodeIndex, possibleRelativesInt, unvisitedDirections);

            PickNextNode(Nodes[index], unvisitedDirections, possibleRelativesInt);

            yield return new WaitForSeconds(0.01f);
        }
    }

    private int GetNextIndex()
    {
        int index = 0;

        switch (nextNode)
        {
            case NextNodeMode.Random:
                index = Random.Range(0, Nodes.Count - 1);
                break;
            case NextNodeMode.Newest:
                index = Nodes.Count - 1;
                break;
            case NextNodeMode.Split:
                index = (Random.value > 0.5f ? Random.Range(0, Nodes.Count) : Nodes.Count - 1);
                break;
        }

        return index;
    }


    private void FindRelatives(int nodeIndex, List<int> possibleRelativesInt, List<Direction> directionList)
    {
        // Find North Relative
        if (nodeIndex < 100 && IsThisRelativeUnvisited(nodeIndex + 1))
            AddPossibleRelative(nodeIndex + 1, possibleRelativesInt, directionList, Direction.North);
        // Find South Relative
        else if (nodeIndex > 0 && IsThisRelativeUnvisited(nodeIndex - 1))
            AddPossibleRelative(nodeIndex + 1, possibleRelativesInt, directionList, Direction.South);
        // Find East Relative
        else if (nodeIndex <= 90 && IsThisRelativeUnvisited(nodeIndex + 10))
            AddPossibleRelative(nodeIndex + 10, possibleRelativesInt, directionList, Direction.East);
        // Find West Relative
        else if (nodeIndex >= 10 && IsThisRelativeUnvisited(nodeIndex - 10))
            AddPossibleRelative(nodeIndex - 10, possibleRelativesInt, directionList, Direction.West);
    }
    private bool IsThisRelativeUnvisited(int relativeIndex)
    {
        return !SpawnedNodes[relativeIndex].GetNodeVisit();
    }
    private void AddPossibleRelative(int nodeIndex, List<int> possibleRelativesInt, List<Direction> directions, Direction dir)
    {
        directions.Add(dir);
        possibleRelativesInt.Add(nodeIndex);

        Debug.Log("Possible Direction: " + dir);
        Debug.Log("Possible Relative Int: " + nodeIndex);
    }

    private void PickNextNode(Node currentNode, List<Direction> directionList, List<int> possibleRelativesInt)
    {
        if (directionList.Count > 0)
        {
            int newDirection = Random.Range(0, directionList.Count);
            Node nextNode = SpawnedNodes[possibleRelativesInt[newDirection]];

            AddNodeToNodesList(nextNode);
            RemoveWalls(currentNode, nextNode, directionList[newDirection]);
        }
        else
        {
            Nodes.Remove(Nodes[Nodes.Count - 1]);
        }
    }

    private void AddNodeToNodesList(Node nextNode)
    {
        Nodes.Add(nextNode);
        nextNode.SetNodeVisit();
    }

    private void RemoveWalls(Node currentNode, Node newNode, Direction direction)
    {
        currentNode.RemoveWall((int)direction);
        newNode.RemoveWall((int)Directions.Opposite(direction));
    }
}