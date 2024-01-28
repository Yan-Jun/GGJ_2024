using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Category
{
    Bedrock,
    Base,
    Wall_1,
    Wall_2,
}

[CreateAssetMenu(fileName = "ColorWall", menuName = "ColorWalls/Create ColorWall")]
public class ColorWallBase : ScriptableObject
{
    [SerializeField] Category category;
    [SerializeField] TileBase tileBase;

    public TileBase TileBase
    {
        get
        {
            return tileBase;
        }
    }

    public Category Category
    {
        get
        {
            return category;
        }
    }
}
