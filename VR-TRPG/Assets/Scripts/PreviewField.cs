using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewField : MonoBehaviour
{
    Transform visualTransform;
    public Transform center;
    private Quaternion rotationStartPoint;
    private Vector3Int _targetPosition;
    Quaternion _targetRotation;
    Vector3 anchor;

    float moveDuration;
    private Vector3Int moveStartPosition;
    float rotationDuration;


    // Methods
    void Start()
    {
        anchor = GridManager.Instance.anchor;

        RefreshVisual(GridManager.Instance.GetFieldVisual(), GridManager.Instance.GetCellSize());
        SetCenter(GridManager.Instance.anchor);
    }

    void OnEnable()
    {
        GridManager.Instance.OnSelectedGridCellChange.AddListener(SetAnchor);
        GridManager.Instance.OnFieldRotationChange.AddListener(SetTargetRotation);
        GridManager.Instance.OnSelectedFieldChange.AddListener(RefreshVisual);
    }

    void SetTargetRotation(Quaternion targetRotation)
    {
        transform.rotation = targetRotation;
    }

    void SetAnchor(Vector3 targetPosition)
    {
        anchor = targetPosition;
    }

    void SetCenter(Vector3 target)
    {
        center.position = target;
    }


    void LateUpdate()
    {
        Vector3Int moveVector = Vector3Int.RoundToInt(anchor - center.position);

        if (moveVector != Vector3Int.zero)
        {
            moveDuration += Time.deltaTime * 15f;
            _targetPosition = Vector3Int.RoundToInt(transform.position + moveVector);
            transform.position = Vector3.Slerp(transform.position, _targetPosition, moveDuration);
        }
        else
        {
            moveDuration = 0;
            transform.position = Vector3Int.RoundToInt(_targetPosition);
        }
    }


    void RefreshVisual(FieldVisual fieldVisual, float size)
    {
        if (visualTransform != null)
        {
            Destroy(visualTransform.gameObject);
        }

        visualTransform = Instantiate(fieldVisual.transform, Vector3.zero, Quaternion.identity);
        FieldVisual currentFieldVisual = visualTransform.GetComponent<FieldVisual>();
        currentFieldVisual.SetSize(size);

        visualTransform.parent = transform;
        visualTransform.localPosition = Vector3.zero;
        visualTransform.localEulerAngles = Vector3.zero;
        SetLayerRecusrive(visualTransform.gameObject, 11);
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
