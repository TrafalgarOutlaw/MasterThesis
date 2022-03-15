using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    Transform visual;
    public void SetSize(float cellSize)
    {
        visual.localScale = new Vector3(cellSize, cellSize, cellSize);
    }
}
