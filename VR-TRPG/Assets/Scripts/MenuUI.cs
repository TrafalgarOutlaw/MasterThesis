using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRTRPG.XR;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject container;

    [SerializeField]
    GameObject editor;
    int currentCamPos = 0;

    public EditorCameraController editorCameraController;
    private XRSystem _XRSystem;

    void Start()
    {
        _XRSystem = XRSystem.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            container.SetActive(!container.activeSelf);
        }

    }

    public void LoadLevel()
    {
        if (!_XRSystem.IsControllerPlaced)
        {
            return;
        }

        container.SetActive(false);
        editor.SetActive(false);

        InputManager.Instance.DisableAllModes();
        editorCameraController.Enable();
        ActionManager.Instance.LoadLevel();
    }

    public void LoadEditor()
    {
        container.SetActive(false);
        editor.SetActive(true);
    }
}
