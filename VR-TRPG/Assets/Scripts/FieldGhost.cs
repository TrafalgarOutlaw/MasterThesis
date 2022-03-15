using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGhost : MonoBehaviour
{
    Transform visual;

    // Start is called before the first frame update
    void Start()
    {
        RefreshVisual();

        GridManager.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    {
        RefreshVisual();
    }

    void LateUpdate()
    {
        Vector3 targetPosition = GridManager.Instance.GetMouseWorldSnappedPosition();
        targetPosition.y = 0f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);


        transform.rotation = Quaternion.Lerp(transform.rotation, GridManager.Instance.GetPlacedFieldRotation(), Time.deltaTime * 15f);
    }

    void RefreshVisual()
    {

        if (visual != null && !visual.CompareTag("Player"))
        {
            Destroy(visual.gameObject);
        }

        if (visual != null && visual.CompareTag("Player"))
        {
            StartField.Instance.OnStartDeselected();
        }

        visual = null;

        FieldSO placedFieldSO = GridManager.Instance.GetPlacedFieldSO();

        if (placedFieldSO)
        {
            if (placedFieldSO.prefab.CompareTag("Player"))
            {
                visual = StartField.Instance.transform;
                StartField.Instance.OnStartSelected();
                visual.GetComponent<StartField>()?.SetSize(GridManager.Instance.GetCellSize());
            }
            else
            {
                visual = Instantiate(placedFieldSO.prefab, Vector3.zero, Quaternion.identity);
                visual.GetComponent<Field>()?.SetSize(GridManager.Instance.GetCellSize());
            }

            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            SetLayerRecusrive(visual.gameObject, 11);
        }
    }

    void SetLayerRecusrive(GameObject targetGameObject, int layer)
    {
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform)
        {
            SetLayerRecusrive(child.gameObject, layer);
        }
    }
}
