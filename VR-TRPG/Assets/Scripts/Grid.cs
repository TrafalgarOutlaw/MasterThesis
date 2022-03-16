using System;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
        public int z;
    }

    int _length;
    int _width;
    int _height;
    float _cellSize;
    Vector3 _originPosition;
    TGridObject[,,] _gridArray;
    bool _showDebug = true;

    public Grid(int length, int height, int width, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, int, TGridObject> createGridObject)
    {
        _length = length;
        _height = height;
        _width = width;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[length, height, width];

        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < _gridArray.GetLength(2); z++)
                {
                    _gridArray[x, y, z] = createGridObject(this, x, y, z);
                }
            }
        }


        if (_showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < _gridArray.GetLength(1); z++)
                {
                    //debugTextArray[x, z] = Utils.CreateWorldText(_gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 15, Color.red, TextAnchor.MiddleCenter, TextAlignment.Center);
                    //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.red, 100f);
                    //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.red, 100f);
                }
            }
            //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
            //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                //debugTextArray[eventArgs.x, eventArgs.z].text = _gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    public float GetCellSize()
    {
        return _cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * _cellSize + _originPosition;
    }


    public void GetXZ(Vector3 worlsPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worlsPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worlsPosition - _originPosition).y / _cellSize);
        z = Mathf.FloorToInt((worlsPosition - _originPosition).z / _cellSize);
    }

    public TGridObject GetGridObject(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < _length && y < _height && z < _width)
        {
            return _gridArray[x, y, z];
        }
        else
        {
            return default(TGridObject);
        }
    }
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        GetXZ(worldPosition, out int x, out int y, out int z);
        return GetGridObject(x, y, z);
    }

    public void SetGridObject(int x, int y, int z, TGridObject value)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < _length && y < _height && z < _width)
        {
            _gridArray[x, y, z] = value;
            TriggerGridObjectChanged(x, y, z);
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        GetXZ(worldPosition, out int x, out int y, out int z);
        SetGridObject(x, y, z, value);
    }

    public void TriggerGridObjectChanged(int x, int y, int z)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
    }


}

