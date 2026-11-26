using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Renderer ground;
    [SerializeField] private int gridSize = 25;
    [SerializeField] private float cellSize = 20f;

    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.5f;
    [SerializeField] private Color lineColor = new Color(1f, 1f, 1f, 0.4f);
    [SerializeField] private Color hoverColor = Color.yellow;

    public GridCell HoveredCell { get; private set; }

    private GridCell[,] cells;
    private Vector3 origin;
    private Plane groundPlane;
    private Vector2 mousePosition;
    private LineRenderer hoverLine;

    private void OnValidate()
    {
        if (ground != null)
        {
            ClampGridSize();
        }
    }

    private void Awake()
    {
        ClampGridSize();

        Bounds bounds = ground.bounds;
        float halfWidth = gridSize * cellSize / 2f;
        origin = new Vector3(bounds.center.x - halfWidth, bounds.max.y + 0.05f, bounds.center.z - halfWidth);
        groundPlane = new Plane(Vector3.up, origin);

        cells = new GridCell[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 center = origin + new Vector3((x + 0.5f) * cellSize, 0f, (z + 0.5f) * cellSize);
                cells[x, z] = new GridCell(new Vector2Int(x, z), center);
            }
        }
    }

    private void OnEnable()
    {
        InputPlayer.OnMousePosition += SetMousePosition;
    }

    private void OnDisable()
    {
        InputPlayer.OnMousePosition -= SetMousePosition;
    }

    private void Start()
    {
        DrawGrid();
        hoverLine = CreateLine(new Vector3[4], hoverColor);
        hoverLine.loop = true;
        hoverLine.enabled = false;
    }

    private void Update()
    {
        UpdateHoveredCell();
    }

    public GridCell GetCell(Vector2Int coordinates)
    {
        bool inside = coordinates.x >= 0 && coordinates.y >= 0 && coordinates.x < gridSize && coordinates.y < gridSize;
        return inside ? cells[coordinates.x, coordinates.y] : null;
    }

    public bool IsFree(GridCell cell)
    {
        return cell != null && !cell.IsOccupied;
    }

    public void Occupy(GridCell cell)
    {
        cell.IsOccupied = true;
    }

    private void ClampGridSize()
    {
        cellSize = Mathf.Max(cellSize, 0.1f);

        Vector3 groundSize = ground.bounds.size;
        int maxGridSize = Mathf.Max(1, Mathf.FloorToInt(Mathf.Min(groundSize.x, groundSize.z) / cellSize + 0.001f));

        gridSize = Mathf.Clamp(gridSize, 1, maxGridSize);
    }

    private void SetMousePosition(Vector2 position)
    {
        mousePosition = position;
    }

    private void UpdateHoveredCell()
    {
        Ray ray = cam.ScreenPointToRay(mousePosition);
        HoveredCell = null;

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 local = ray.GetPoint(distance) - origin;
            HoveredCell = GetCell(new Vector2Int(Mathf.FloorToInt(local.x / cellSize), Mathf.FloorToInt(local.z / cellSize)));
        }

        hoverLine.enabled = HoveredCell != null;
        
        if (HoveredCell != null)
        {
            Vector3 half = new Vector3(cellSize / 2f, 0f, cellSize / 2f);
            Vector3 corner = HoveredCell.Center - half;
            hoverLine.SetPosition(0, corner);
            hoverLine.SetPosition(1, corner + new Vector3(cellSize, 0f, 0f));
            hoverLine.SetPosition(2, corner + new Vector3(cellSize, 0f, cellSize));
            hoverLine.SetPosition(3, corner + new Vector3(0f, 0f, cellSize));
        }
    }

    private void DrawGrid()
    {
        float length = gridSize * cellSize;

        for (int i = 0; i <= gridSize; i++)
        {
            float offset = i * cellSize;
            CreateLine(new[] { origin + new Vector3(offset, 0f, 0f), origin + new Vector3(offset, 0f, length) }, lineColor);
            CreateLine(new[] { origin + new Vector3(0f, 0f, offset), origin + new Vector3(length, 0f, offset) }, lineColor);
        }
    }

    private LineRenderer CreateLine(Vector3[] points, Color color)
    {
        LineRenderer line = new GameObject("GridLine").AddComponent<LineRenderer>();
        line.transform.SetParent(transform, false);
        line.useWorldSpace = true;
        line.material = lineMaterial;
        line.widthMultiplier = lineWidth;
        line.startColor = color;
        line.endColor = color;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.positionCount = points.Length;
        line.SetPositions(points);
        return line;
    }
}
