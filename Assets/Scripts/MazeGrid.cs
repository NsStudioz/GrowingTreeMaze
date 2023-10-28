using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerfectMazeProject.Grid
{
    public class MazeGrid : MonoBehaviour
    {

        [SerializeField] private Node nodePrefab;

        public int gridWidth {  get; private set; }
        public int gridHeight { get; private set; }

        #region EventListeners:

        private void Start()
        {
            UIController.OnWidthValueChange += ChangeGridWidth;
            UIController.OnHeightValueChange += ChangeGridHeight;
        }

        private void OnDestroy()
        {
            UIController.OnWidthValueChange -= ChangeGridWidth;
            UIController.OnHeightValueChange -= ChangeGridHeight;
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
        public List<Node> GetNodeGrid()
        {
            List<Node> nodeGridList = new List<Node>();

            for (int x = 0; x < gridWidth; x++)
                for (int z = 0; z < gridHeight; z++)
                {
                    Node newNode = SpawnNewCellInstance(nodePrefab, SetNodePosition(x, z));
                    SetNodeAsChildToParent(newNode, transform);
                    nodeGridList.Add(newNode);
                }

            return nodeGridList;
        }

        /// <summary>
        /// Destroy all nodes in scene, clearing the grid
        /// </summary>
        /// <param name="nodeGridList"></param>
        public void ClearCellGrid(List<Node> nodeGridList) 
        {
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

        #endregion
    }
}

