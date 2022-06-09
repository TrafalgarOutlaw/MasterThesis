using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRTRPG.Grid
{
    // [System.Serializable] public class OnGridEventQuaternion : UnityEvent<Quaternion> { }
    // [System.Serializable] public class OnGridEventFieldVisual : UnityEvent<FieldVisual, float> { }
    // [System.Serializable] public class OnGridEventInt : UnityEvent<int> { }


    public class PlaceSystem : MonoBehaviour
    {
        // This instance
        private static PlaceSystem _instance;
        public static PlaceSystem Instance { get { return _instance; } }

        // Events
        // public OnGridEventQuaternion OnFieldRotationChange;
        // public OnGridEventFieldVisual OnSelectedFieldChange;

        // Grid
        GridSystem grid;

        // Placeable
        List<APlaceable> placeableList;
        int placeableListIndex = 0;
        APlaceable currentPlaceable;
        PlaceableVisual currentVisual;


        // Insert into gridCells

        Quaternion currentRotationGridCell = Quaternion.identity;
        // List<GridCell> currentOccupiedGridCellList;
        // public bool isCurrentFieldPlaceable;

        // Level
        [SerializeField] Transform fieldsContainer;
        InputManager inputManager;
        private float _cellSize;
        public AGridCell currentGridCell;

        void Awake()
        {
            // Set Singleton
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            _instance = this;

            APlaceable[] placeables = Resources.FindObjectsOfTypeAll<APlaceable>();
            print("PLACEABLES: " + placeables.Length);
            //foreach(var item in placeables)
            //{
            //    print("\t" + item.name);
            //}
            placeableList = new List<APlaceable>(placeables);
        }

        void Start()
        {

            inputManager = InputManager.Instance;
            grid = GridSystem.Instance;
            _cellSize = grid.GetCellSize();

            // inputManager.OnRotateField.AddListener(RotateField);

            currentGridCell = grid.GetGridCell(Vector3.zero);
            currentPlaceable = placeableList[placeableListIndex];
            InitVisual();

            //previewField.InitPreviewField(selectedField.visual, _cellSize, selectedField.visual.position);
            //SetPreviewLayer(previewField.previewFieldTransform.gameObject);

            inputManager.OnPlaceField.AddListener(TryPlaceField);
            inputManager.OnSelectedFieldChanged.AddListener(ChangeField);
            inputManager.OnDeletField.AddListener(TryDeletePlaceable);
            grid.OnSelectedGridCellChange.AddListener(ChangeVisualPosition);
            // grid.OnSelectedGridCellChange.AddListener(UpdatePreviewFieldPosition);

        }

        void InitVisual()
        {
            Transform visualTransform = Instantiate(currentPlaceable.getVisual().transform, Vector3.zero, Quaternion.identity, transform);
            SetSize(visualTransform);
            
            currentVisual = visualTransform.GetComponent<PlaceableVisual>();
            List<Vector3Int> neededSpace = currentPlaceable.GetNeededSpace(currentVisual.transform, currentGridCell.Index);
            currentVisual.SetLayer(currentPlaceable.IsPlaceable(neededSpace));
        }

        public void SetSize(Transform transform)
        {
            //transform.localScale = Vector3.Scale(transform.localScale, new Vector3(_cellSize, _cellSize, _cellSize));

            //foreach (Transform child in transform)
            //{
            //    print("CHILDS: " + child.name);
            //    print("\tCHILDSCALE: " + child.localScale);
            //    print("\tCellsize: " + _cellSize);
            //    child.localScale = Vector3.Scale(child.localScale, new Vector3(_cellSize, _cellSize, _cellSize));
            //}
        }

        void ChangeVisualPosition(Vector3 cellWorldPosition, AGridCell gridCell)
        {
            currentGridCell = gridCell;
            currentVisual.UpdatePosition(cellWorldPosition);
            List<Vector3Int> neededSpace = currentPlaceable.GetNeededSpace(currentVisual.transform, currentGridCell.Index);
            currentVisual.SetLayer(currentPlaceable.IsPlaceable(neededSpace));
        }

        void ChangeField(float value)
        {
            placeableListIndex = (placeableListIndex + (int)Mathf.Clamp(value, -1f, 1f)) % placeableList.Count;
            if (placeableListIndex < 0)
            {
                placeableListIndex += placeableList.Count;
            }
            currentPlaceable = placeableList[placeableListIndex];

            Destroy(currentVisual.gameObject);
            InitVisual();
            ChangeVisualPosition(currentGridCell.WorldPosition, currentGridCell);
        }

        public void TryPlaceField()
        {
            List<Vector3Int> neededSpace = currentPlaceable.GetNeededSpace(currentVisual.transform, currentGridCell.Index);
            //if (!IsFieldPlaceable(neededSpace)) return;
            if (!currentPlaceable.IsPlaceable(neededSpace)) return;

            List<AGridCell> neededGridCells = GetNeededGridCells(neededSpace);
            PlaceField(neededGridCells);
        }

        public void PlaceField(List<AGridCell> neededGridCellsList)
        {
            Transform placeableTransform = Instantiate(currentPlaceable.transform, currentVisual.transform.position, Quaternion.identity, neededGridCellsList[0].transform);
            APlaceable placedObject = placeableTransform.GetComponent<APlaceable>();
            SetSize(placeableTransform);
            placedObject.SetOccupiedGridCells(neededGridCellsList);
        }

        public List<AGridCell> GetNeededGridCells(List<Vector3Int> neededGridCellsIndexList)
        {
            List<AGridCell> neededGridCellsList = new List<AGridCell>();

            foreach (Vector3Int index in neededGridCellsIndexList)
            {
                neededGridCellsList.Add(grid.GetGridCell(index));
            }

            return neededGridCellsList;
        }

        void TryDeletePlaceable()
        {
            int amountIncludedGameobjects = currentGridCell.IncludedGameobjects.Count;
            if (amountIncludedGameobjects == 0) return;
            Destroy(currentGridCell.IncludedGameobjects[amountIncludedGameobjects - 1]);
            //if (!currentGridCell.CanBuild())
            //{
            //    Field field = currentGridCell.IncludedGameobjects;
            //    currentGridCell.IncludedGameobjects.ClearOccupiedGridCells();
            //    Destroy(field.gameObject);
            //    SetPreviewLayer(previewField.previewFieldTransform.gameObject);
            
        }

        // public void RotateField(Vector3Int dir)
        // {
        //     Vector3Int rotation = selectedField.GetRotationValue() * dir;
        //     Vector3 previewFieldlocalRotation = previewFieldTransform.InverseTransformVector(rotation);
        //     currentRotationGridCell *= Quaternion.Euler(previewFieldlocalRotation);

        //     OnFieldRotationChange.Invoke(currentRotationGridCell);
        //     // SetCurrentFieldPlaceable();
        // }



        //     public GridCell GetGridCellFromPosition(Vector3 position)
        //     {
        //         return grid.GetGridCell(position / cellSize);

        //     }

        //     public GridCell GetGridCellFromIndex(Vector3 position)
        //     {
        //         return grid.GetGridCell(position);

        //     }

        //     public float GetCellSize()
        //     {
        //         return cellSize;
        //     }

    }
}
