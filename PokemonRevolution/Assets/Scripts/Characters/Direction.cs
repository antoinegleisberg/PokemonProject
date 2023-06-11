using UnityEngine;

public enum Direction
{
    Down,
    Left,
    Up,
    Right,
}

public static class DirectionUtils
{
    public static Direction GetDirection(Vector2 vector)
    {
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
        {
            if (vector.x > 0)
                return Direction.Right;
            return Direction.Left;
        }
        if (vector.y > 0)
            return Direction.Up;
        return Direction.Down;
    }
    
    public static Direction GetDirection(float x, float y) => GetDirection(new Vector2(x, y));
    public static Direction GetDirection(Vector2Int vector) => GetDirection(new Vector2(vector.x, vector.y));
    public static Direction GetDirection(Vector3 vector) => GetDirection(new Vector2(vector.x, vector.y));
    
    public static Vector2Int ToVec2(Direction facingDirection)
    {
        switch (facingDirection) {
            case Direction.Up:
                return Vector2Int.up;
            case Direction.Right:
                return Vector2Int.right;
            case Direction.Down:
                return Vector2Int.down;
            case Direction.Left:
                return Vector2Int.left;
            default:
                return Vector2Int.down;
        }
    }

    public static int Rotation(Direction facingDirection)
    {
        return (int)facingDirection * 90;
    }
}