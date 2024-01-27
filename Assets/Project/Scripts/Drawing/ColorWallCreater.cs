using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorWallCreater : MonoBehaviour
{
    [SerializeField] Tilemap previewMap, defaultMap;
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

    private void Awake()
    {
        i = this;
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        mouseInput.OnLeftClick += OnLeftClick;
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

    public void ObjectSelected(ColorWallBase obj)
    {
        SelectedWall = selectedWall == obj ? null : obj;
    }

    private void UpdatePreview()
    {
        previewMap.SetTile(lastGridPos, null);
        previewMap.SetTile(currentGridPos, tileBase);
    }

    private void HandleDrawing()
    {
        DrawItem();
    }

    private void DrawItem()
    {
        defaultMap.SetTile(currentGridPos, tileBase);
    }
}
