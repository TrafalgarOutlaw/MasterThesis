using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTRPG.Movement;

public class MovementIndicator : MonoBehaviour
{
    new Camera camera;
    private int mouseColliderLayerMask;

    // Start is called before the first frame update
    // void Start()
    // {
    //     camera = Camera.main;
    //     mouse = Mouse.current;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (mouse.leftButton.wasPressedThisFrame)
    //     {
    //         Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
    //         int i = 0;
    //         if (Physics.Raycast(ray, out RaycastHit rayCastHit, 999f, ~i))
    //         {
    //             rayCastHit.collider.transform.root.gameObject.TryGetComponent<Field>(out Field field);
    //             if (field != null)
    //             {
    //                 Vector3 fieldPosition = field.transform.position;
    //                 MovementSystem.Instance.TryWalk(fieldPosition);
    //             }
    //         }
    //     }
    // }

}
