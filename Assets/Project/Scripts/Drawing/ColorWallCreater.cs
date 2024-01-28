using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorWallCreater : MonoBehaviour
{
    public Tilemap previewMap, defaultMap, bedrockMap;
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

    int currentAltCount;

    bool doCastAlt;

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
        if (selectedWall != null /*&& !mouseInput.IsPointerOverUI()*/)
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
        int pointCount = PointsInCircle((Vector2Int)currentGridPos, currentThickness, 1).Count;
        if (pointCount > PlayerStat.i.GetWallPoint(tileBase.name))
            return;
        
        DrawItem();

        AudioManger.i.Play("Draw", false);
    }

    public int ClearItem(Vector3Int deathPos, int radius)
    {
        int clearAmount = 0;
        foreach (Vector2Int point in PointsInCircle((Vector2Int)deathPos, radius, 1))
        {
            if (bedrockMap.GetTile((Vector3Int)point) == null || bedrockMap.GetTile((Vector3Int)point).name != "Bedrock") 
            {
                if (defaultMap.GetTile((Vector3Int)point) != null)
                {
                    clearAmount++; 
                    defaultMap.SetTile((Vector3Int)point, null);
                }
            }
        }
        return clearAmount;
    }

    private void DrawItem()
    {
        foreach (Vector2Int point in PointsInCircle((Vector2Int)currentGridPos, currentThickness, 1))
        {
            if (CheckCanOverrideTile(bedrockMap, (Vector3Int)point) && !CheckAlreadyPaintedTile(defaultMap, (Vector3Int)point, tileBase.name)) 
            {
                CheckAddAltCount((Vector3Int)point);

                PlayerStat.i.AddWallPoint(tileBase.name, -1);
                defaultMap.SetTile((Vector3Int)point, tileBase);
                
                CastAlt(doCastAlt);
            }
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
    private bool CheckAlreadyPaintedTile(Tilemap tilemap, Vector3Int point, string name)
    {
        if (tilemap.GetTile(point) != null && tilemap.GetTile(point).name == name)
            return true;
        return false;
    }
    private void CheckAddAltCount(Vector3Int point)
    {
        if (point.y < -72)
        {
            if (defaultMap.GetTile(point) != null)
                return;

            currentAltCount++;
            //Debug.Log(currentAltCount);
            if (currentAltCount > 3225)
            {
                doCastAlt = true;
            }
        }
    }
    private void CastAlt(bool value)
    {
        if (value)
        {
            //clear enemy and heal 1 hp
            EnemyManager.i.ClearAllEnemy();
            PlayerStat.i.AddHealthPoint(1);

            for (int x = 171; x > -172; x--)
            {
                for (int y = -72; y > -98; y--)
                {
                    Vector3Int currentTile = new Vector3Int(x, y, 0);
                    if (defaultMap.GetTile(currentTile) != null && defaultMap.GetTile(currentTile).name != "Bedrock")
                        defaultMap.SetTile(currentTile, null);
                }
            }
            currentAltCount = 0;

            doCastAlt = false;

            AudioManger.i.Play("Superattack", false);
        }
    }
}
