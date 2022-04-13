using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable] public class OnGridEventVector3 : UnityEvent<Vector3> { }

[System.Serializable] public class OnGridEventQuaternion : UnityEvent<Quaternion> { }
[System.Serializable] public class OnGridEventFieldVisual : UnityEvent<FieldVisual, float> { }
[System.Serializable] public class OnGridEventInt : UnityEvent<int> { }

public class GridManager : MonoBehaviour
{
    // This instance
    private static GridManager _instance;
    public static GridManager Instance { get { return _instance; } }
    Mouse mouse;

    // Events
    public OnGridEventVector3 OnSelectedGridCellChange;
    public OnGridEventQuaternion OnFieldRotationChange;
    public OnGridEventFieldVisual OnSelectedFieldChange;
    public OnGridEventInt OnIsFieldPlaceableChange;

    // Grid
    Grid<GridCell> grid;
    [SerializeField] int gridLength = 10;
    [SerializeField] int gridHeight = 3;
    [SerializeField] int gridWidth = 10;
    [SerializeField] float cellSize = 10f;
    [SerializeField] Transform pfGridCell;
    [SerializeField] Transform gridGameObject;
    bool isPlayerSet;
    int currentY = 1;

    // Placeable fields
    [SerializeField] List<FieldSO> fieldSOList;
    int fieldSOListIndex = 0;
    FieldSO currentField;

    // Previewfield
    [SerializeField] Transform previewFieldTransform;

    // Insert into gridCells
    GridCell currentGridCell;
    Quaternion currentRotationGridCell = Quaternion.identity;
    List<GridCell> currentOccupiedGridCellList;
    public bool isCurrentFieldPlaceable;
    public Vector3 anchor;

    // Level
    Transform levelObject;

    void Awake()
    {
        // Set Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        _instance = this;

        // Create GridObject
        grid = new Grid<GridCell>(gridLength, gridHeight, gridWidth, cellSize, Vector3.zero, pfGridCell, (Grid<GridCell> g, int x, int y, int z, Transform emptyGridObject) => new GridCell(g, x, y, z, emptyGridObject));
        currentGridCell = grid.GetGridObject(new Vector3(0, currentY, 0));

        // Set selectedField
        currentField = fieldSOList[fieldSOListIndex];

        mouse = Mouse.current;
    }

    private void Start()
    {
        InputManager.Instance.OnPlaceField.AddListener(TryPlaceField);
        InputManager.Instance.OnDeletField.AddListener(TryDeleteFields);
        InputManager.Instance.OnRotateField.AddListener(RotateField);

        anchor = GetGridCellCenter(Vector3.zero);
    }

    private void OnEnable()
    {
        SetCurrentGridCell(currentGridCell);
    }

    void Update()
    {
        GridCell selectedGridCell = GetGridObjectFromMouse();
        if (selectedGridCell != currentGridCell)
        {
            SetCurrentGridCell(selectedGridCell);
        }

        if (mouse.scroll.ReadValue().y != 0)
        {
            fieldSOListIndex = (fieldSOListIndex + (int)Mathf.Clamp(mouse.scroll.ReadValue().y, -1f, 1f)) % fieldSOList.Count;
            if (fieldSOListIndex < 0)
            {
                fieldSOListIndex += fieldSOList.Count;
            }
            currentField = fieldSOList[fieldSOListIndex];

            SetCurrentFieldPlaceable();
            OnSelectedFieldChange.Invoke(currentField.fieldVisual, cellSize);
        }

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            DisableCurrentRow(currentY);
            currentY = (currentY + 1) % gridHeight;
            EnableCurrentRow(currentY);
        }
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            DisableCurrentRow(currentY);
            currentY--;
            if (currentY < 0)
            {
                currentY += gridHeight;
            }
            EnableCurrentRow(currentY);
        }
    }

    private void SetCurrentGridCell(GridCell selectedGridCell)
    {
        currentGridCell.EnableRenderer();
        currentGridCell = selectedGridCell;
        selectedGridCell.DisableRenderer();
        Vector3 selectedGridObjectWordlPosition = selectedGridCell.GetWorldPosition();
        anchor = GetGridCellCenter(selectedGridObjectWordlPosition);
        OnSelectedGridCellChange.Invoke(anchor);

        SetCurrentFieldPlaceable();
        OnSelectedFieldChange.Invoke(currentField.fieldVisual, cellSize);
    }

    void SetOccupiedGridCellList()
    {
        List<GridCell> occupyGridCellList = new List<GridCell>();

        Vector3Int xDirInWorld = Vector3Int.RoundToInt(previewFieldTransform.TransformDirection(Vector3.right));
        Vector3Int yDirInWorld = Vector3Int.RoundToInt(previewFieldTransform.TransformDirection(Vector3.up));
        Vector3Int zDirInWorld = Vector3Int.RoundToInt(previewFieldTransform.TransformDirection(Vector3.forward));

        for (int x = 0; x < currentField.width; x++)
        {
            for (int y = 0; y < currentField.height; y++)
            {
                for (int z = 0; z < currentField.length; z++)
                {
                    Vector3Int occupiedGridCellIndex = (x * xDirInWorld + -y * yDirInWorld + z * zDirInWorld) + currentGridCell.GetIndex();
                    GridCell occupiedGridCell = grid.GetGridObject(occupiedGridCellIndex);
                    if (occupiedGridCell == null || !occupiedGridCell.CanBuild())
                    {
                        currentOccupiedGridCellList = null;
                        return;
                    }
                    occupyGridCellList.Add(occupiedGridCell);
                }
            }
        }
        currentOccupiedGridCellList = occupyGridCellList;
        return;
    }

    internal FieldVisual GetFieldVisual()
    {
        return currentField.fieldVisual;
    }

    void SetCurrentFieldPlaceable()
    {
        SetOccupiedGridCellList();
        if (currentOccupiedGridCellList != null)
        {
            isCurrentFieldPlaceable = true;
        }
        else
        {
            isCurrentFieldPlaceable = false;
        }

    }

    public void TryPlaceField()
    {
        if (isCurrentFieldPlaceable)
        {
            PlaceField(currentField.pfField);
        }
    }

    private void PlaceField(Field pfField)
    {
        var field = Instantiate(pfField.transform, previewFieldTransform.position, currentRotationGridCell, levelObject);
        field.GetComponent<Field>().SetSize(cellSize);

        foreach (var cell in currentOccupiedGridCellList)
        {
            cell.SetField(field, currentOccupiedGridCellList);
        }
    }

    void TryDeleteFields()
    {
        if (currentGridCell.CanBuild())
        {
            return;
        }

        Destroy(currentGridCell.GetPlacedField().gameObject);
        currentGridCell.ClearField();
    }

    public void RotateField(Vector3Int dir)
    {
        Vector3Int rotation = currentField.GetRotationValue() * dir;
        Vector3 previewFieldlocalRotation = previewFieldTransform.InverseTransformVector(rotation);
        currentRotationGridCell *= Quaternion.Euler(previewFieldlocalRotation);

        OnFieldRotationChange.Invoke(currentRotationGridCell);
        // SetCurrentFieldPlaceable();
    }

    private Vector3 GetGridCellCenter(Vector3 position)
    {
        return position + new Vector3(1, -1, 1) * cellSize * .5f;
    }

    private GridCell GetGridObjectFromMouse()
    {
        Vector3 worldPosition = Utils.GetMouseWorldPosition();
        if (worldPosition == -Vector3.one)
        {
            return currentGridCell;
        }
        GridCell gridObject = grid.GetGridObject(worldPosition);
        return gridObject;
    }

    void EnableCurrentRow(int currentY)
    {
        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                grid.GetGridObject(new Vector3(x, currentY, z)).EnableRenderer();
            }
        }
    }

    void DisableCurrentRow(int currentY)
    {
        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                grid.GetGridObject(new Vector3(x, currentY, z)).DisableForMouse();
            }
        }
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public bool IsPlayerPlaced()
    {
        return isPlayerSet;
    }

    public class GridCell
    {
        Grid<GridCell> _grid;
        int _x;
        int _y;
        int _z;
        Vector3Int index;
        Transform _emptyGridObjectTransform;
        EmptyGridObject _emptyGridObject;
        Transform _transform;
        Vector3 positionInWorld;
        List<GridCell> occupiedGridCellList = new List<GridCell>();

        public GridCell(Grid<GridCell> grid, int x, int y, int z, Transform emptyGridObject)
        {
            _grid = grid;
            _x = x;
            _y = y;
            _z = z;

            float cellSize = grid.GetCellSize();
            positionInWorld = new Vector3(x, y, z) * cellSize;
            index = new Vector3Int(_x, _y, _z);

            _emptyGridObjectTransform = Instantiate(emptyGridObject, positionInWorld, Quaternion.identity, GridManager.Instance.gridGameObject);
            _emptyGridObjectTransform.localScale *= cellSize;

            _emptyGridObject = _emptyGridObjectTransform.GetComponent<EmptyGridObject>();
            _emptyGridObject.SetIndex(new Vector3(x, y, z));
            if (y != 1)
            {
                DisableForMouse();
            }
        }

        public Transform GetPlacedField()
        {
            return _transform;
        }

        public Vector3Int GetIndex()
        {
            return new Vector3Int(_x, _y, _z);
        }

        public void SetField(Transform transform, List<GridCell> occupyGridCellList)
        {
            _transform = transform;
            foreach (var cell in occupyGridCellList)
            {
                if (cell != this)
                {
                    occupiedGridCellList.Add(cell);
                }
            }
        }

        public void ClearField()
        {
            _transform = null;
            ClearOccupiedGridCellList();
        }

        void ClearOccupiedGridCellList()
        {
            foreach (var cell in occupiedGridCellList)
            {
                cell._transform = null;
                cell.occupiedGridCellList = new List<GridCell>();
            }
            occupiedGridCellList = new List<GridCell>();
        }

        public bool CanBuild()
        {
            return _transform == null;
        }

        internal void DisableForMouse()
        {
            _emptyGridObject.DisableForMouse();
        }

        internal void DisableRenderer()
        {
            _emptyGridObject.DisableRenderer();
        }

        internal void EnableRenderer()
        {
            _emptyGridObject.EnableRenderer();
        }

        internal Vector3 GetWorldPosition()
        {
            return positionInWorld;
        }
    }
}
