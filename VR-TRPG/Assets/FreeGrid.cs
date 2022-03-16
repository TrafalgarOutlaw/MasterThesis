using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeGrid : MonoBehaviour
{
    public int x;
    public int y;
    public int z;
    public GameObject grid;

    public Vector3Int GetIndex()
    {
        return new Vector3Int(x, y, z);
    }

    public void SetIndex(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void EnableGrid()
    {
        grid.SetActive(true);
    }

    internal void DisableGrid()
    {
        grid.SetActive(false);
    }
}
