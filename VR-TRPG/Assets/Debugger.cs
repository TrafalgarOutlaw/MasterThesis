using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRTRPG.XR;

public class Debugger : MonoBehaviour
{
    private XRSystem _XRSystem;

    // Start is called before the first frame update
    void Start()
    {
        _XRSystem = XRSystem.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            _XRSystem.EnableNextControllerInList();
        }

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            _XRSystem.EnablePreviousControllerInList();
        }
    }
}
