using UnityEngine;

public class GridCell
{
    public Vector2Int Coordinates { get; }
    public Vector3 Center { get; }
    public bool IsOccupied { get; set; }

    public GridCell(Vector2Int coordinates, Vector3 center)
    {
        Coordinates = coordinates;
        Center = center;
    }
}
