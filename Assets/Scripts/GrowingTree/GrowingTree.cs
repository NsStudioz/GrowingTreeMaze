using PerfectMazeProject.DirectionExtensions;
using PerfectMazeProject.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingTree : MonoBehaviour
{
    private static GrowingTree instance;
    
    [Header("Audio")]
    [SerializeField] private GameAudio gameAudio;

    [Header("Main Elements")]
    [SerializeField] private float generationSpeed = 0.0001f;
    [SerializeField] private List<Node> spawnedNodes = new List<Node>();
    [SerializeField] private List<Node> nodes = new List<Node>();

    private int mazeWidth;
    private int mazeHeight;

    private enum NextNodeIndex
    {
        Random,
        Newest,
        Split // 50/50
    }

    [SerializeField] private NextNodeIndex nextIndexMode = NextNodeIndex.Newest;

    /// <summary>
    /// Choose a prim's generation approach (Random), a Backtracker approach (Newest), or randomized both (Split)
    /// </summary>
    /// <returns></returns>
    private int GetNextIndex()
    {
        int index = 0;

        switch (nextIndexMode)
        {
            case NextNodeIndex.Random:
                index = Random.Range(0, nodes.Count - 1);
                break;
            case NextNodeIndex.Newest:
                index = nodes.Count - 1;
                break;
            case NextNodeIndex.Split:
                index = (Random.value > 0.5f ? Random.Range(0, nodes.Count) : nodes.Count - 1);
                break;
        }

        return index;
    }

    /// <summary>
    /// Event listener, will change based on UI dropdown state
    /// </summary>
    /// <param name="index"></param>
    private void ChangeNextIndexMode(int index)
    {
        nextIndexMode = (NextNodeIndex)index;
    }

    #region SingletonAndEventListeners:
    private void Awake()
    {
        InitializeSingleton();
        //
        UIController.OnClickGenerateTheMazeButton += StartSimulation;
        UIController.OnClickBackButton += ResetMaze;
        UIController.OnDropdownValueChange += ChangeNextIndexMode;
        MazeGrid.OnGridGenerated += SimulateTheMaze;
    }

    private void OnDestroy()
    {
        StopMazeSimulation();
        //
        UIController.OnClickGenerateTheMazeButton -= StartSimulation;
        UIController.OnClickBackButton -= ResetMaze;
        UIController.OnDropdownValueChange -= ChangeNextIndexMode;
        MazeGrid.OnGridGenerated -= SimulateTheMaze;
    }

    private void InitializeSingleton()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    #endregion

    #region MazeSimulationSetup:
    /// <summary>
    /// Event listener to start the maze generation simulation
    /// </summary>
    private void StartSimulation()
    {
        ResetMaze();
        Initialize();
    }

    /// <summary>
    /// Stop maze simulation and clear maze grid
    /// </summary>
    private void ResetMaze()
    {
        if (spawnedNodes.Count > 0)
        {
            StopMazeSimulation();

            ClearGrid();
        }
    }

    /// <summary>
    /// Clear the grid and destroy nodes
    /// </summary>
    private void ClearGrid()
    {
        MazeGrid.instance.ClearCellGrid(spawnedNodes);

        spawnedNodes.Clear();
        nodes.Clear();
    }

    /// <summary>
    /// Start Maze Simulation
    /// </summary>
    /// <param name="state"></param>
    private void SimulateTheMaze(bool state)
    {
        if (state)
        {
            SetFirstRandomIndex();

            StartCoroutine(GrowingTreeSimulation());
        }
    }

    /// <summary>
    /// Make a list of nodes, spawn and populate them on the map based on the grid's width and height and make sure all of their walls are visible. 
    /// </summary>
    private void Initialize()
    {
        mazeWidth = MazeGrid.instance.gridWidth;
        mazeHeight = MazeGrid.instance.gridHeight;

        spawnedNodes = MazeGrid.instance.GetNodeGridCoroutine();
    }

    /// <summary>
    /// Pick a node randomly to start working on when generating the maze
    /// </summary>
    private void SetFirstRandomIndex()
    {
        int chosenNodeIndex = Random.Range(0, spawnedNodes.Count);
        nodes.Add(spawnedNodes[chosenNodeIndex]);
        nodes[0].SetNodeVisited();
    }

    #endregion

    #region GrowingTreeAlgorithm:

    /// <summary>
    /// Maze Simulation using the Growing Tree Algorithm. Until the list is empty, keep looping and carve a path
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
            int chosenNodeIndex = GetNextIndex();

            // check for possible cell neighbours next to the pointed cell:
            FindRelatives(spawnedNodes.IndexOf(nodes[chosenNodeIndex]), possibleDirections, possibleRelatives);

            // Pick the next Node and remove walls:
            NextNode(nodes[chosenNodeIndex], possibleDirections, possibleRelatives);

            yield return new WaitForSeconds(generationSpeed);
        }

        gameAudio.PlayClip();
        for (int i = 0; i < spawnedNodes.Count; i++)
            spawnedNodes[i].SetNodeCompleteColor();
    }

    /// <summary>
    /// Find nodes relative to the chosen node to use
    /// </summary>
    /// <param name="possibleDirections"></param>
    /// <param name="possibleRelatives"></param>
    /// <param name="index"></param>
    private void FindRelatives(int index, List<Direction> possibleDirections, List<int> possibleRelatives)
    {
        // Calculate the indexes based on the height,
        // we use - 1 so it won't count up to max number of height.
        int cellX = index / mazeHeight;
        int cellY = index % mazeHeight;

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
            spawnedNodes[index].SetNodeRelativeFoundColor();
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

    #endregion

    /// <summary>
    /// Stop the maze generation entirely
    /// </summary>
    private void StopMazeSimulation()
    {
        StopAllCoroutines();
    }
}
