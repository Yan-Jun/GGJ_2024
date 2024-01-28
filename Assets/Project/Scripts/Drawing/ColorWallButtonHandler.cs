using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorWallButtonHandler : MonoBehaviour
{
    [SerializeField] ColorWallBase item;

    ColorWallCreater colorWallCreater;

    private void Awake()
    {
        colorWallCreater = ColorWallCreater.i;
    }

    public void ButtonClicked()
    {
        Debug.Log("clicked: " + item.name);
        colorWallCreater.ObjectSelected(item);
    }
}
