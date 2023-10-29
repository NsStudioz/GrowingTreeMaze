using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerfectMazeProject.Grid
{
    public class MazeGrid : MonoBehaviour
    {
        public static MazeGrid instance;

        [SerializeField] private Node nodePrefab;

        public int gridWidth {  get; private set; }
        public int gridHeight { get; private set; }

        public bool isGridGenerated { get; private set; }

        public static event Action<bool> OnGridGenerated;

        #region SingletonAndEventListeners:

        private void Start()
        {
            InitializeSingleton();

            UIController.OnWidthValueChange += ChangeGridWidth;
            UIController.OnHeightValueChange += ChangeGridHeight;
        }

        private void OnDestroy()
        {
            UIController.OnWidthValueChange -= ChangeGridWidth;
            UIController.OnHeightValueChange -= ChangeGridHeight;
        }

        private void InitializeSingleton()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);
            else
                instance = this;
        }

        private void ChangeGridWidth(int value)
        {
            gridWidth = value;
        }
        private void ChangeGridHeight(int value)
        {
            gridHeight = value;
        }

        #endregion

        #region PublicMethods

        /// <summary>
        /// node list getter. positioning every created node, setting it as child to this parent gameobject and adding it to the list.
        /// </summary>
        /// <returns></returns>
        public List<Node> GetNodeGridCoroutine()
        {
            List<Node> nodeGridList = new List<Node>();

            StartCoroutine(CreateGrid(nodeGridList));

            return nodeGridList;
        }

        /// <summary>
        /// nodes grid coroutine task
        /// </summary>
        /// <param name="nodeGridList"></param>
        /// <returns></returns>
        private IEnumerator CreateGrid(List<Node> nodeGridList)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridHeight; z++)
                {
                    Node newNode = SpawnNewCellInstance(nodePrefab, SetNodePosition(x, z));
                    SetNodeAsChildToParent(newNode, transform);
                    nodeGridList.Add(newNode);
                }
                yield return new WaitForSeconds(0.01f);
            }

            GridIsGenerated();
            OnGridGenerated?.Invoke(isGridGenerated);
        }

        /// <summary>
        /// Destroy all nodes in scene, clearing the grid
        /// </summary>
        /// <param name="nodeGridList"></param>
        public void ClearCellGrid(List<Node> nodeGridList) 
        {
            GridIsDeleted();

            for (int i = 0; i < nodeGridList.Count; i++)
                Destroy(nodeGridList[i].gameObject);
        }

        #endregion

        #region PrivateMethods

        private Node SpawnNewCellInstance(Node prefab, Vector3 cellPos)
        {
            return Instantiate(prefab, cellPos, Quaternion.identity);
        }

        private Vector3 SetNodePosition(int x, int z)
        {
            Vector3 nodePos = new Vector3(x, 0, z);
            return nodePos;
        }

        private void SetNodeAsChildToParent(Node prefab, Transform parent)
        {
            prefab.transform.parent = parent;
        }

        private void GridIsGenerated()
        {
            isGridGenerated = true;
        }

        private void GridIsDeleted()
        {
            isGridGenerated = false;
        }

        #endregion
    }
}

