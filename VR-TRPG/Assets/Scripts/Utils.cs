using UnityEngine;
using UnityEngine.InputSystem;

public static class Utils
{
    public const int sortingOrderDefault = 5000;

    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPositon = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null)
        {
            color = Color.white;
        }
        return CreateWorldText(parent, text, localPositon, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static Vector3 GetMouseWorldPosition(bool debug = false)
    {
        int layerMask = 1 << 6;
        //layerMask = ~layerMask;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(Mouse.current.position.ReadValue(), ray.direction);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layerMask))
        {
            EmptyGridObject emptyGridObject = raycastHit.collider.GetComponentInParent<EmptyGridObject>();
            if (debug)
            {
                Debug.Log("IN RAYCAST");
                Debug.Log("emptyGridObjectIndex: " + emptyGridObject.index);
            }
            return emptyGridObject.index;
        }
        return -Vector3.one;
        /*
            RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, 999f, layerMask);

        for (int i = hits.Length - 1; i >= 0; i--)
        {
            if (hits[i].collider.gameObject.CompareTag("FreeGrid"))
            {
                GameObject hitParent = hits[i].collider.transform.parent.gameObject;
                return hitParent.GetComponent<FreeGrid>().GetIndex();
            }
        }
        return Vector3Int.zero;
        */
    }
}
