using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class MouseInputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    ColorWallCreater colorWallCreater;

    public event Action OnLeftClick, OnRightClick, OnExit;

    private void Awake()
    {
        colorWallCreater = ColorWallCreater.i;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            OnLeftClick?.Invoke();
        if (Input.GetMouseButton(1))
            OnRightClick?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();


    public Vector3 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = sceneCamera.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        return mousePos;
    }
}
