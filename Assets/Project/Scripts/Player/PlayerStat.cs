using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat i;

    public int Health;
    public int Wall_2_Point;
    public int Wall_1_Point;

    private void Awake()
    {
        i = this;
    }

    private void Update()
    {
        
    }

    public int GetWallPoint(string name)
    {
        if (name == "Wall_2")
            return Wall_2_Point;
        if (name == "Wall_1")
            return Wall_1_Point;
        return 0;
    }

    public void AddWallPoint(string name, int amount)
    {
        if (name == "Wall_2")
            Wall_2_Point = Mathf.Clamp(Wall_2_Point + amount, 0, 5000);
        if (name == "Wall_1")
            Wall_1_Point = Mathf.Clamp(Wall_1_Point + amount, 0, 5000);
    }
}
