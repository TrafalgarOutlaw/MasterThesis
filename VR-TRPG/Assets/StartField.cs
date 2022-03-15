using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartField : MonoBehaviour
{
    static StartField _instance;

    [SerializeField]
    GameObject xrContainer;

    [SerializeField]
    GameObject visual;
    bool _isPlaced = false;

    public static StartField Instance { get { return _instance; } }

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
    }

    public void OnStartPlaced()
    {
        _isPlaced = true;
    }

    internal void OnStartRemoved()
    {
        _isPlaced = false;

    }

    public void OnStartLevel()
    {
        xrContainer.SetActive(true);
    }

    internal void OnEndLevel()
    {
        xrContainer.SetActive(false);
    }

    internal void OnStartDeselected()
    {
        if (!_isPlaced)
        {
            visual.SetActive(false);
        }
    }

    public void SetSize(float cellSize)
    {
        visual.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
    }

    internal void OnStartSelected()
    {
        visual.SetActive(true);
    }
}