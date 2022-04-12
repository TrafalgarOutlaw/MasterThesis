using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyGridObject : MonoBehaviour
{
    public Vector3 index;
    public MeshRenderer meshRenderer;
    bool _isActive = true;

    internal void SetIndex(Vector3 index)
    {
        this.index = index;
    }

    public void DisableRenderer()
    {
        meshRenderer.enabled = false;
        _isActive = false;
    }

    public void EnableRenderer()
    {
        meshRenderer.enabled = true;
        _isActive = true;
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
