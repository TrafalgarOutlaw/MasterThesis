using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public void SetSize(float cellSize)
    {
        foreach (Transform child in transform)
        {
            child.localScale = Vector3.Scale(child.localScale, new Vector3(cellSize, cellSize, cellSize));
        }
    }
}
