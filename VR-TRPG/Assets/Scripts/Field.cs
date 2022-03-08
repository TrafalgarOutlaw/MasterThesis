using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    Transform visual;
    [SerializeField]
    Transform player;
    public void SetSize(float cellSize)
    {
        visual.localScale = new Vector3(cellSize, 1, cellSize);
        if (player != null)
        {
            player.localScale = Vector3.one * cellSize;
            player.localPosition = new Vector3(player.localPosition.x * cellSize, player.localPosition.y, player.localPosition.z * cellSize);
        }
    }
}
