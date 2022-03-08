using UnityEngine;

[CreateAssetMenu(fileName = "FieldSO", menuName = "ScriptableObjects/FieldSO")]
public class FieldSO : ScriptableObject
{
    public enum Dir
    {
        Down,
        Left,
        Up,
        Right,
    }
    public string nameString;
    public int width = 1;
    public int height = 1;
    public Transform prefab;
    public Transform visual;
    public bool isWalkable;

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0, width);
            case Dir.Up: return new Vector2Int(width, height);
            case Dir.Right: return new Vector2Int(height, 0);
        }
    }

    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Up;
            case Dir.Up: return Dir.Right;
            case Dir.Right: return Dir.Down;
        }
    }

}
