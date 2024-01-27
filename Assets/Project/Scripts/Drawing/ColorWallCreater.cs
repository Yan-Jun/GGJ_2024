using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorWallCreater : MonoBehaviour
{
    [SerializeField] Tilemap previewMap, defaultMap, bedrockMap;
    [SerializeField] MouseInputManager mouseInput;

    TileBase tileBase;
    ColorWallBase selectedWall;
    private ColorWallBase SelectedWall
    {
        set
        {
            selectedWall = value;

            tileBase = selectedWall != null ? selectedWall.TileBase : null;

            UpdatePreview();
        }
    }

    Camera _camera;

    Vector2 mousePos;
    Vector3Int currentGridPos;
    Vector3Int lastGridPos;

    public static ColorWallCreater i;

    int currentThickness = 5;
    [SerializeField] private int MaxThickness;
    [SerializeField] private int MinThickness;

    private void Awake()
    {
        i = this;
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        mouseInput.OnLeftClick += OnLeftClick;
        mouseInput.OnRightClick += OnRightClick;
        mouseInput.OnScroll += OnScroll;
    }

    private void OnDisable()
    {
        mouseInput.OnLeftClick -= OnLeftClick;
        mouseInput.OnRightClick -= OnRightClick;
        mouseInput.OnScroll -= OnScroll;
    }

    private void Update()
    {
        if (selectedWall != null)
        {
            Vector3 pos = mouseInput.GetMousePos();
            Vector3Int gridPos = previewMap.WorldToCell(pos);

            if (gridPos != currentGridPos)
            {
                lastGridPos = currentGridPos;
                currentGridPos = gridPos;

                UpdatePreview();
            }
        }
    }

    private void OnLeftClick()
    {
        if (selectedWall != null && !mouseInput.IsPointerOverUI())
        {
            HandleDrawing();
        }
    }

    private void OnRightClick()
    {

    }

    private void OnScroll()
    {
        currentThickness = Mathf.Clamp(currentThickness + mouseInput.GetMouseScrollDelta(), MinThickness, MaxThickness);
        previewMap.ClearAllTiles();
        UpdatePreview();
    }

    public void ObjectSelected(ColorWallBase obj)
    {
        SelectedWall = selectedWall == obj ? null : obj;
    }

    private void UpdatePreview()
    {
        foreach (Vector2Int point in PointsInCircle((Vector2Int)lastGridPos, currentThickness, 1)) 
        {
            previewMap.SetTile((Vector3Int)point, null);
        }
        foreach (Vector2Int point in PointsInCircle((Vector2Int)currentGridPos, currentThickness, 1))
        {
            previewMap.SetTile((Vector3Int)point, tileBase);
        }
    }

    private void HandleDrawing()
    {
        DrawItem();
    }

    private void DrawItem()
    {
        foreach (Vector2Int point in PointsInCircle((Vector2Int)currentGridPos, currentThickness, 1))
        {
            if (CheckCanOverrideTile(bedrockMap, (Vector3Int)point))
                defaultMap.SetTile((Vector3Int)point, tileBase);
        }
    }

    private bool InCircle(Vector2Int point, Vector2Int circlePoint, int radius)
    {
        return (point - circlePoint).sqrMagnitude <= radius * radius;
    }

    private List<Vector2Int> PointsInCircle(Vector2Int circlePos, int radius, int scale)
    {
        List<Vector2Int> points = new List<Vector2Int>();

        int minX = circlePos.x - radius;
        int maxX = circlePos.x + radius;

        int minY = circlePos.y - radius;
        int maxY = circlePos.y + radius;

        for (int y = minY; y <= maxY; y += scale)
        {
            for (int x = minX; x <= maxX; x += scale)
            {
                if (InCircle(new Vector2Int(x, y), circlePos, radius))
                {
                    points.Add(new Vector2Int(x, y));
                }
            }
        }

        return points;
    }

    private bool CheckCanOverrideTile(Tilemap tilemap, Vector3Int point)
    {
        if (tilemap.GetTile(point) == null)
            return true;
        if (tilemap.GetTile(point).name != "Bedrock" && tilemap.GetTile(point).name != "Base")
            return true;
        return false;
    }
}
