using PerfectMazeProject.DirectionExtensions;
using PerfectMazeProject.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingTreeTest : MonoBehaviour
{
    [SerializeField] private MazeGrid grid;
    [SerializeField] float timer = 2f;

    [SerializeField] private List<Node> totalNodes = new List<Node>();
    [SerializeField] private List<Node> visitedNodes = new List<Node>();

    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeHeight;

    private void Start()
    {
        totalNodes = grid.GetNodeGrid();

        for (int i = 0; i < totalNodes.Count; i++)
            totalNodes[i].SetNodeVisible();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            GenerateTheMaze();
    }

    private void GenerateTheMaze()
    {
        Stack<Node> pointedCell = new Stack<Node>();

        pointedCell.Push(totalNodes[Random.Range(0, totalNodes.Count)]);

        int chosenNodeIndex = Random.Range(0, totalNodes.Count);
        visitedNodes.Add(totalNodes[chosenNodeIndex]);
        Debug.Log("Starting First Index: " + chosenNodeIndex);
        visitedNodes[0].SetNodeVisited();

        StartCoroutine(CalculateMazeGeneration());
    }

    private IEnumerator CalculateMazeGeneration()
    {
        while (visitedNodes.Count > 0)
        {
            // Declaring lists and a pointed cell index:
            List<int> possibleNeighbours = new List<int>();
            List<Direction> availableDirs = new List<Direction>();

            //int pointedCellIndex = totalNodes.IndexOf(pointedCell.Peek());

            //int chosenNodeIndex = visitedNodes.Count - 1; 
            int chosenNodeIndex = Random.Range(0, visitedNodes.Count - 1);

            //int chosenNodeIndex = totalNodes.IndexOf(visitedNodes[visitedNodes.Count - 1]);
            Debug.Log("Pointed Cell Index: " + chosenNodeIndex);

            // check for possible cell neighbours next to the pointed cell:
            //CheckAvailableNeighbors(availableDirs, possibleNeighbours, chosenNodeIndex);
            CheckAvailableNeighbors(availableDirs, possibleNeighbours, totalNodes.IndexOf(visitedNodes[chosenNodeIndex]));

            PickNextCellForWork(availableDirs, possibleNeighbours, visitedNodes[chosenNodeIndex]);
            // avoids crashing:
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CheckAvailableNeighbors(List<Direction> availableDirections, List<int> possibleNeighbours, int cellIndex)
    {
        // Calculate the indexes
        int cellX = cellIndex / mazeHeight; // example: 46 / 10 (height) = 4 (int). denominates float.
        int cellY = cellIndex % mazeHeight; // example: 46 % 10 = 6 remainder of height.

        if (cellY < mazeHeight - 1) // Check above neighbour: we use - 1 since it shouldn't count up to max number of height.
            CheckTheFollowingNeighbour(availableDirections, Direction.North, possibleNeighbours, cellIndex + 1);

        if (cellY > 0) // Check below neighbour:
            CheckTheFollowingNeighbour(availableDirections, Direction.South, possibleNeighbours, cellIndex - 1);

        // Checking available neighbors.
        if (cellX < mazeWidth - 1)  // Check right neighbour:
            CheckTheFollowingNeighbour(availableDirections, Direction.East, possibleNeighbours, cellIndex + mazeHeight);

        if (cellX > 0) // Check left neighbour:
            CheckTheFollowingNeighbour(availableDirections, Direction.West, possibleNeighbours, cellIndex - mazeHeight);
    }

    private void CheckTheFollowingNeighbour(List<Direction> directions, Direction dir, List<int> neighbours, int Index)
    {
        if (ThisNeighbourIsNotVisited(Index))
        {
            directions.Add(dir);
            neighbours.Add(Index);

            Debug.Log("Possible Direction: " + dir);
            Debug.Log("Possible Relative Int: " + Index);
        }
    }

    private bool ThisNeighbourIsNotVisited(int index)
    {
        return !totalNodes[index].GetNodeVisit();
    }

    private bool ThisNeighbourCellIsNotVisitedAndPointed(Node currentNode, int index)
    {
        return !visitedNodes.Contains(totalNodes[index]);
            //&& !currentNode.Contains(totalNodes[index]);
    }


    private void PickNextCellForWork(List<Direction> availableDirections, List<int> possibleNeighbours, Node currentNode)
    {
        // Pick next cell:
        if (availableDirections.Count > 0)
        {
            // Pick a random Direction:
            int pointedDir = Random.Range(0, availableDirections.Count);
            Debug.Log("Picked Direction: " + pointedDir);

            // Pick the next cell:
            Node nextCell = totalNodes[possibleNeighbours[pointedDir]];
            nextCell.SetNodeVisit();
            RemoveCellWalls(currentNode, nextCell, availableDirections[pointedDir]);
            visitedNodes.Add(nextCell);
            //visitedNodes.Add(pointedCell.Peek());
            //pointedCell.Push(nextCell);
        }
        else // BACKTRACKING
        {
            //visitedNodes.Add(pointedCell.Peek()); // Add the top element of the stack to visited list.
            //pointedCell.Pop(); // pop top element
            visitedNodes.Remove(currentNode);
        }
    }

    private void RemoveCellWalls(Node currentNode, Node nextNode, Direction pointedCellDir)
    {
        currentNode.RemoveWall((int)pointedCellDir);
        nextNode.RemoveWall((int)Directions.Opposite(pointedCellDir));
    }
}
