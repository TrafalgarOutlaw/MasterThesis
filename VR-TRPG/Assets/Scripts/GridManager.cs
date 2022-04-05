using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;

    public static GridManager Instance { get { return _instance; } }

    [SerializeField]
    int gridLength = 10;
    [SerializeField]
    int gridHeight = 3;
    [SerializeField]
    int gridWidth = 10;
    [SerializeField]
    float cellSize = 10f;

    [SerializeField]
    List<FieldSO> fieldSOList;
    FieldSO fieldSO;
    FieldSO startFieldSO;
    public Transform pfFreeGrid;

    Grid<GridObject> _grid;
    FieldSO.Dir dir = FieldSO.Dir.Down;

    int fieldSOListIndex = 0;

    [SerializeField]
    Transform level;

    public event EventHandler OnSelectedChanged;
    Mouse mouse;

    bool _playerIsPlaced = false;
    public Transform freeGrids;
    Transform[,,] _freeGridArray;
    int currentY = 0;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
        _grid = new Grid<GridObject>(gridLength, gridHeight, gridWidth, cellSize, Vector3.zero, (Grid<GridObject> g, int x, int y, int z) => new GridObject(g, x, y, z));

        fieldSO = fieldSOList[fieldSOListIndex];
        mouse = Mouse.current;

        _freeGridArray = new Transform[gridLength, gridHeight, gridWidth];
        for (int x = 0; x < gridLength; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridWidth; z++)
                {
                    Vector2Int rotationOffset = fieldSO.GetRotationOffset(dir);
                    Vector3 freeGridPosition = _grid.GetWorldPosition(x, y, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.GetCellSize();
                    Transform freeGrid = Instantiate(pfFreeGrid, freeGridPosition, Quaternion.Euler(0, fieldSO.GetRotationAngle(dir), 0), freeGrids);
                    _freeGridArray[x, y, z] = freeGrid;
                    freeGrid.localScale = new Vector3(cellSize, cellSize, cellSize);
                    freeGrid.GetComponent<FreeGrid>().SetIndex(x, y, z);

                    if (y == 0)
                    {
                        freeGrid.GetComponent<FreeGrid>().EnableGrid();
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mouse.leftButton.wasPressedThisFrame)
        {
            _grid.GetXZ(Utils.GetMouseWorldPosition(), out int x, out int y, out int z);

            GridObject gridObject = _grid.GetGridObject(x, y, z);
            if (gridObject != null && gridObject.CanBuild())
            {
                Vector2Int rotationOffset = fieldSO.GetRotationOffset(dir);
                Vector3 placedFieldWorldPosition = GetMouseWorldSnappedPosition(); //_grid.GetWorldPosition(x, y, z); //+ new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.GetCellSize();
                Transform builtTransform;
                if (fieldSO.isPlayer)
                {
                    if (_playerIsPlaced)
                    {
                        Debug.Log("Player is already placed in level");
                        return;
                    }

                    _playerIsPlaced = true;
                    builtTransform = StartField.Instance.transform;
                    builtTransform.GetComponent<StartField>()?.SetSize(cellSize);
                    builtTransform.parent = level;
                    builtTransform.transform.position = placedFieldWorldPosition;

                    StartField.Instance.OnStartPlaced();
                    startFieldSO = fieldSO;
                    fieldSOList.RemoveAt(fieldSOListIndex);
                    fieldSOListIndex = 0;
                    fieldSO = fieldSOList[fieldSOListIndex];
                    RefreshSelectedObjectType();
                }
                else
                {
                    builtTransform = Instantiate(fieldSO.prefab, placedFieldWorldPosition, Quaternion.Euler(0, fieldSO.GetRotationAngle(dir), 0), level);
                    builtTransform.GetComponent<Field>()?.SetSize(cellSize);
                }
                Transform newField = gridObject.SetField(builtTransform);

                if (fieldSO.isWalkable)
                {
                    Vector3Int gridIndex = gridObject.GetIndex();
                    //Debug.Log(gridIndex);
                    for (int gridX = gridIndex.x - 1; gridX <= gridIndex.x + 1; gridX++)
                    {
                        if (gridX < 0 || gridX >= gridLength)
                            continue;
                        for (int gridY = gridIndex.y - 1; gridY <= gridIndex.y + 1; gridY++)
                        {
                            if (gridY < 0 || gridY >= gridHeight)
                                continue;
                            for (int gridZ = gridIndex.z - 1; gridZ <= gridIndex.z + 1; gridZ++)
                            {
                                if (gridZ < 0 || gridZ >= gridWidth || (gridX == gridIndex.x && gridY == gridIndex.y && gridZ == gridIndex.z))
                                    continue;
                                GridObject neightborGrid = _grid.GetGridObject(gridX, gridY, gridZ);
                                var isCurrentStart = newField.GetComponent<StartField>();
                                var testStart = neightborGrid.GetField()?.gameObject.GetComponent<StartField>();
                                var neightbor = neightborGrid.GetField()?.gameObject.GetComponent<Field>();
                                if (testStart != null && testStart.isWalkable)
                                {
                                    Debug.Log(neightborGrid.GetField());
                                    StartField.Instance.AddNeightbor(neightborGrid.GetField());
                                }
                                if (isCurrentStart != null && neightbor != null && neightbor.isWalkable)
                                {
                                    StartField.Instance.AddNeightbor(neightbor.transform);
                                    Debug.Log(neightborGrid.GetField());
                                }
                            }
                        }
                    }
                }
            }
        }

        if (mouse.rightButton.wasPressedThisFrame)
        {
            GridObject gridObject = _grid.GetGridObject(Utils.GetMouseWorldPosition());
            if (gridObject != null && gridObject.GetField() != null)
            {
                Transform field = gridObject.GetField();
                if (field.CompareTag("Player"))
                {
                    _playerIsPlaced = false;
                    StartField.Instance.OnStartRemoved();
                    StartField.Instance.ResetNeighbot();
                    fieldSOList.Add(startFieldSO);
                    fieldSOListIndex = fieldSOList.Count - 1;
                    fieldSO = fieldSOList[fieldSOListIndex];
                    RefreshSelectedObjectType();
                }
                gridObject.ClearField();
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            dir = FieldSO.GetNextDir(dir);
        }

        if (mouse.scroll.ReadValue().y != 0)
        {
            fieldSOListIndex = (fieldSOListIndex + (int)Mathf.Clamp(mouse.scroll.ReadValue().y, -1f, 1f)) % fieldSOList.Count;
            if (fieldSOListIndex < 0)
            {
                fieldSOListIndex += fieldSOList.Count;
            }
            fieldSO = fieldSOList[fieldSOListIndex];
            RefreshSelectedObjectType();
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

    void EnableCurrentRow(int currentY)
    {
        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                _freeGridArray[x, currentY, z].GetComponent<FreeGrid>().EnableGrid();
            }
        }
    }

    void DisableCurrentRow(int currentY)
    {
        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                _freeGridArray[x, currentY, z].GetComponent<FreeGrid>()?.DisableGrid();
            }
        }
    }

    void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3Int freeGrid = Utils.GetMouseWorldPosition();
        //_grid.GetXZ(mousePosition, out int x, out int y, out int z);

        if (fieldSO != null)
        {
            Vector2Int rotationOffset = fieldSO.GetRotationOffset(dir);
            Vector3 placedFieldWorldPosition = _grid.GetWorldPosition(freeGrid.x, freeGrid.y, freeGrid.z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * _grid.GetCellSize();
            return placedFieldWorldPosition;
        }
        else
        {
            return freeGrid;
        }
    }

    public Quaternion GetPlacedFieldRotation()
    {
        if (fieldSO != null)
        {
            return Quaternion.Euler(0, fieldSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public FieldSO GetPlacedFieldSO()
    {
        return fieldSO;
    }

    public bool IsPlayerPlaced()
    {
        return _playerIsPlaced;
    }

    public class GridObject
    {
        Grid<GridObject> _grid;
        int _x;
        int _y;
        int _z;
        Transform _transform;

        public GridObject(Grid<GridObject> grid, int x, int y, int z)
        {
            _grid = grid;
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector3Int GetIndex()
        {
            return new Vector3Int(_x, _y, _z);
        }

        public Transform SetField(Transform transform)
        {
            _transform = transform;
            _grid.TriggerGridObjectChanged(_x, _y, _z);
            return transform;

        }

        public Transform GetField()
        {
            return _transform;
        }

        public void ClearField()
        {
            if (!_transform.CompareTag("Player"))
            {
                Destroy(_transform.gameObject);
            }

            _transform = null;
        }

        public bool CanBuild()
        {
            return _transform == null;
        }
    }
}
