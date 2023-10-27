using PerfectMazeProject.DirectionExtensions;
using PerfectMazeProject.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingTree : MonoBehaviour
{
    [SerializeField] private MazeGrid grid;

    [SerializeField] private List<Node> spawnedNodes = new List<Node>();
    [SerializeField] private List<Node> nodes = new List<Node>();

    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeHeight;

    [SerializeField] private bool isQuickestGenerate;
    [SerializeField] private float generationSpeed = 0.01f;

    private void Start()
    {
        spawnedNodes = grid.GetNodeGrid();

        for (int i = 0; i < spawnedNodes.Count; i++)
            spawnedNodes[i].SetNodeVisible();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            GenerateTheMaze();

        if (Input.GetKeyDown(KeyCode.S))
            StopMazeSimulation();
    }

    private void GenerateTheMaze()
    {
        int chosenNodeIndex = Random.Range(0, spawnedNodes.Count);
        nodes.Add(spawnedNodes[chosenNodeIndex]);
        nodes[0].SetNodeVisited();

        StartCoroutine(GrowingTreeSimulation());
    }

    /// <summary>
    /// Maze Simulation using the Growing Tree Algorithm.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GrowingTreeSimulation()
    {
        while (nodes.Count > 0)
        {
            // Declaring Relative lists:
            List<int> possibleRelatives = new List<int>();
            List<Direction> possibleDirections = new List<Direction>();

            // Node to use for work:
            int chosenNodeIndex = Random.Range(0, nodes.Count - 1);

            // check for possible cell neighbours next to the pointed cell:
            FindRelatives(spawnedNodes.IndexOf(nodes[chosenNodeIndex]), possibleDirections, possibleRelatives);

            // Pick the next Node and remove walls:
            NextNode(nodes[chosenNodeIndex], possibleDirections, possibleRelatives);

            yield return new WaitForSeconds(generationSpeed);
        }
    }

    /// <summary>
    /// Find nodes relative to the chosen node to use
    /// </summary>
    /// <param name="possibleDirections"></param>
    /// <param name="possibleRelatives"></param>
    /// <param name="index"></param>
    private void FindRelatives(int index, List<Direction> possibleDirections, List<int> possibleRelatives)
    {
        // Calculate the indexes, we use - 1 since it shouldn't count up to max number of height.
        int cellX = index / mazeHeight; // example: 46 / 10 (height) = 4 (int). denominates float.
        int cellY = index % mazeHeight; // example: 46 % 10 = 6 remainder of height.

        // Check above relative: 
        if (cellY < mazeHeight - 1) 
            AddPossibleRelative(index + 1, possibleDirections, Direction.North, possibleRelatives);
        // Check below relative:
        if (cellY > 0) 
            AddPossibleRelative(index - 1, possibleDirections, Direction.South, possibleRelatives);
        // Check right relative:
        if (cellX < mazeWidth - 1)  
            AddPossibleRelative(index + mazeHeight, possibleDirections, Direction.East, possibleRelatives);
        // Check left relative:
        if (cellX > 0) 
            AddPossibleRelative(index - mazeHeight, possibleDirections, Direction.West, possibleRelatives);
    }

    /// <summary>
    /// Add this relative to possible directions
    /// </summary>
    /// <param name="possibleDirections"></param>
    /// <param name="dir"></param>
    /// <param name="possibleRelatives"></param>
    /// <param name="index"></param>
    private void AddPossibleRelative(int index, List<Direction> possibleDirections, Direction dir, List<int> possibleRelatives)
    {
        if (ThisRelativeIsNotVisited(index))
        {
            possibleDirections.Add(dir);
            possibleRelatives.Add(index);
        }
    }

    /// <summary>
    /// Has this relative been visited yet?
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool ThisRelativeIsNotVisited(int index)
    {
        return !spawnedNodes[index].IsVisited;
    }

    /// <summary>
    /// Pick the next node to use, mark it as visited and add it to the list of nodes
    /// </summary>
    /// <param name="possibleDirections"></param>
    /// <param name="possibleRelatives"></param>
    /// <param name="currentNode"></param>
    private void NextNode(Node currentNode, List<Direction> possibleDirections, List<int> possibleRelatives)
    {
        if (possibleDirections.Count > 0)
        {
            // Choose a random Direction:
            int dir = Random.Range(0, possibleDirections.Count);

            // Pick the next node, mark it as visited:
            Node nextNode = spawnedNodes[possibleRelatives[dir]];
            nextNode.SetNodeVisited();

            // Remove a wall from current node, remove opposite wall of relative node:
            RemoveWalls(currentNode, nextNode, possibleDirections[dir]);
            nodes.Add(nextNode);
        }
        else // if no relatives found:
        {
            nodes.Remove(currentNode);
        }
    }

    /// <summary>
    /// Remove a wall from currently used node, then remove a relative opposite wall from the next node
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="nextNode"></param>
    /// <param name="dir"></param>
    private void RemoveWalls(Node currentNode, Node nextNode, Direction dir)
    {
        currentNode.RemoveWall((int)dir);
        nextNode.RemoveWall((int)Directions.Opposite(dir));
    }

    private void StopMazeSimulation()
    {
        StopAllCoroutines();
    }

/*    private void OnDestroy()
    {
        StopMazeSimulation();
    }*/
}
