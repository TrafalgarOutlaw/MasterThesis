using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public class Visual : MonoBehaviour
    {
        GridSystem gridSystem;
        Vector3 offset;

        void Start()
        {
            gridSystem = GridSystem.Instance;
            offset = transform.position * gridSystem.GetCellSize();
        }

        public void SetLayer(bool isPlaceable)
        {
            if (isPlaceable) SetLayerRecusrive(gameObject, 11);
            else SetLayerRecusrive(gameObject, 12);
        }

        void SetLayerRecusrive(GameObject go, int layer)
        {
            go.layer = layer;
            foreach (Transform child in go.transform)
            {
                SetLayerRecusrive(child.gameObject, layer);
            }
        }
    }
}
