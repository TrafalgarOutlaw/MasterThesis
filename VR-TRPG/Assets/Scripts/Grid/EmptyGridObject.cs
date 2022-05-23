using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTRPG.Grid
{
    public class EmptyGridObject : MonoBehaviour
    {
        public Vector3 index;
        public MeshRenderer meshRenderer;
        bool _isActive = true;

        internal void SetIndex(Vector3 index)
        {
            this.index = index;
        }

        public void DisableForMouse()
        {
            meshRenderer.enabled = false;
            SetLayerRecusrive(7);
        }

        public void DisableRenderer()
        {
            meshRenderer.enabled = false;
        }

        public void EnableRenderer()
        {
            meshRenderer.enabled = true;
            SetLayerRecusrive(6);
        }

        public bool IsActive()
        {
            return _isActive;
        }

        void SetLayerRecusrive(GameObject targetGameObject, int layer)
        {
            targetGameObject.layer = layer;
            foreach (Transform child in targetGameObject.transform)
            {
                SetLayerRecusrive(child.gameObject, layer);
            }
        }

        public void SetLayerRecusrive(int layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in transform)
            {
                SetLayerRecusrive(child.gameObject, layer);
            }
        }
    }
}
