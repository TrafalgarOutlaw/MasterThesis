using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRTRPG.Grid
{
    [System.Serializable] public class OnGridSelectedGridCellChanged : UnityEvent<Vector3, AGridCell> { }
    public class GridSystem : MonoBehaviour
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
        public class OnGridObjectChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
            public int z;
        }

        // This instance
        private static GridSystem _instance;
        public static GridSystem Instance { get { return _instance; } }

        // Events

        [SerializeField] int gridLength = 10;
        [SerializeField] int gridHeight = 3;
        [SerializeField] int gridWidth = 10;
        [SerializeField] float cellSize = 10f;
        [SerializeField] List<AGridCell> pfGridCellList;

        AGridCell[,,] gridCellArray;

        //______________________________________________________________________________
        public AGridCell SelectedGridCell { get; private set; }
        [SerializeField] Camera editorCamera;
        [HideInInspector] public OnGridSelectedGridCellChanged OnSelectedGridCellChange;
        [SerializeField] Transform gridContainer;

        void Awake()
        {
            // Set Singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            _instance = this;

            gridCellArray = new AGridCell[gridLength, gridHeight, gridWidth];

            for (int x = 0; x < gridLength; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    for (int z = 0; z < gridWidth; z++)
                    {
                        Transform gridCell = Instantiate(pfGridCellList[0].transform, Vector3.zero, Quaternion.identity, gridContainer);
                        gridCell.name = "GridCell (" + x + "|" + y + "|" + z + ")";

                        AGridCell gridCellComponent = gridCell.GetComponent<AGridCell>();
                        gridCellComponent.Init(this, x, y, z, cellSize);
                        gridCellArray[x, y, z] = gridCellComponent;
                        SetGridCellNeighbors(gridCellComponent);
                    }
                }
            }
        }

        void Start()
        {


            // SetSelectedGridCell(activeStagePlane);
        }

        void Update()
        {
            AGridCell detectedGridCell = GetGridCellUnderMouse();
            if (SelectedGridCell != detectedGridCell)
            {
                SetSelectedGridCell(detectedGridCell);
            }
        }

        public AGridCell[,,] GetGridcellArray()
        {
            return gridCellArray;
        }

        public Vector3Int GetDimensionsVector()
        {
            return new Vector3Int(gridLength, gridHeight, gridWidth);
        }

        // private void SetSelectedGridCell(Vector3Int index)
        // {
        //     SelectedGridCell?.DisableRenderer();
        //     SelectedGridCell = GetGridCell(index);
        //     SelectedGridCell.EnableRenderer();
        //     OnSelectedGridCellChange.Invoke(SelectedGridCell.WorldPosition);
        //     // OnSelectedFieldChange.Invoke(currentField.fieldVisual, grid.GetCellSize());
        // }

        private void SetSelectedGridCell(AGridCell gridCell)
        {
            SelectedGridCell?.EnableRenderer();
            SelectedGridCell = gridCell;
            SelectedGridCell.DisableRenderer();
            // Debug.Log(gridCell);
            OnSelectedGridCellChange.Invoke(SelectedGridCell.WorldPosition, SelectedGridCell);
            // OnSelectedFieldChange.Invoke(currentField.fieldVisual, grid.GetCellSize());
        }


        private void SetGridCellNeighbors(AGridCell gridCell)
        {
            foreach (Vector3Int dir in gridCell.CellDirList)
            {
                AGridCell neighborGridCell = GetGridCell(gridCell.Index + dir);
                if (neighborGridCell != null)
                {
                    gridCell.SetNeighbor(neighborGridCell);
                }
            }
        }

        private AGridCell GetGridCellUnderMouse(bool debug = false)
        {
            // Vector3 worldPosition = Utils.GetMouseWorldPosition();
            int layerMask = 1 << 6;
            //layerMask = ~layerMask;
            Ray ray = editorCamera.ScreenPointToRay(InputManager.Instance.GetMousePosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layerMask))
            {
                AGridCell detectedGridCell = raycastHit.collider.GetComponentInParent<AGridCell>();
                if (debug)
                {
                    Debug.Log("IN RAYCAST");
                    Debug.Log("emptyGridObjectIndex: " + detectedGridCell.Index);
                }
                return detectedGridCell;
            }
            return SelectedGridCell;
        }

        public void GetIndex(Vector3 worldPosition, out int x, out int y, out int z)
        {
            x = (int)worldPosition.x;
            y = (int)worldPosition.y;
            z = (int)worldPosition.z;
        }

        public AGridCell GetGridCell(Vector3 index)
        {
            if (index.x >= 0 && index.y >= 0 && index.z >= 0 && index.x < gridLength && index.y < gridHeight && index.z < gridWidth)
            {
                return gridCellArray[(int)index.x, (int)index.y, (int)index.z];
            }
            return null;
        }

        public float GetCellSize()
        {
            return cellSize;
        }

        // if (next)
        // {
        //     EnableNextStage();
        // }
        // else
        // {
        //     EnablePreviousStage();
        // }

        // void EnablePreviousStage()
        // {
        //     DisableCurrentStage(currentYLevel);
        //     currentYLevel--;
        //     if (currentYLevel < 0)
        //     {
        //         currentYLevel += gridHeight;
        //     }
        //     EnableCurrentStage(currentYLevel);
        // }

        // void EnableNextStage()
        // {
        //     DisableCurrentStage(currentYLevel);
        //     currentYLevel = (currentYLevel + 1) % gridHeight;
        //     EnableCurrentStage(currentYLevel);

        // }

        void EnableCurrentStage(int currentY)
        {
            for (int x = 0; x < gridLength; x++)
            {
                for (int z = 0; z < gridWidth; z++)
                {
                    GetGridCell(new Vector3(x, currentY, z)).EnableRenderer();
                }
            }
        }

        void DisableCurrentStage(int currentY)
        {
            for (int x = 0; x < gridLength; x++)
            {
                for (int z = 0; z < gridWidth; z++)
                {
                    // GetGridCell(new Vector3(x, currentY, z)).DisableForMouse();
                }
            }
        }
    }
}
