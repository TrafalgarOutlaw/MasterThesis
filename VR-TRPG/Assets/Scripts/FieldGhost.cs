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
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        FieldSO placedFieldSO = GridManager.Instance.GetPlacedFieldSO();

        if (placedFieldSO)
        {
            visual = Instantiate(placedFieldSO.prefab, Vector3.zero, Quaternion.identity);
            visual.GetComponent<Field>()?.SetSize(GridManager.Instance.GetCellSize());
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
