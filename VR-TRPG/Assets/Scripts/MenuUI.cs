using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject container;

    [SerializeField]
    GameObject editor;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            container.SetActive(!container.activeSelf);
            Time.timeScale = (Time.timeScale + 1) % 2;
        }
    }

    public void LoadLevel()
    {
        if (!GridManager.Instance.IsPlayerPlaced())
        {
            Debug.Log("Please first place a player in level");
            return;
        }

        container.SetActive(false);
        Time.timeScale = 1;

        editor.SetActive(false);
    }

    public void LoadEditor()
    {
        Time.timeScale = 1;
        container.SetActive(false);
        editor.SetActive(true);
    }
}
