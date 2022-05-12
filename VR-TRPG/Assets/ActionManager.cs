using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.XR;
using VRTRPG.Movement;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance { get; private set; }
    private XRSystem _XRSystem;
    private MovementSystem movementSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }
    private void Start()
    {
        _XRSystem = XRSystem.Instance;
        movementSystem = MovementSystem.Instance;
    }

    public void LoadLevel()
    {
        _XRSystem.EnableControllerIndex(0);
        movementSystem.StartMovePhase(_XRSystem.CurrentController.GetPlayerObject().GetComponent<PlayerCharacter>());
    }
}
