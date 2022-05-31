using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VRTRPG.Movement;

public class SelectionIndicator : MonoBehaviour
{
    new Camera camera;
    private Mouse mouse;
    private MovementSystem movementSystem;
    private int mouseColliderLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        mouse = Mouse.current;
        movementSystem = MovementSystem.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            int i = 0;
            if (Physics.Raycast(ray, out RaycastHit rayCastHit, 999f, ~i))
            {
                PlayerCharacter character = rayCastHit.collider.transform.root.gameObject.GetComponentInChildren<PlayerCharacter>();
                if (character != null)
                {
                    // camera.transform.position = character.transform.position;
                    // camera.transform.parent = character.transform;
                    // MovementSystem.Instance.TryWalk(fieldPosition);
                    // movementSystem.StartMovePhase(character);
                }
            }
        }

    }
}
