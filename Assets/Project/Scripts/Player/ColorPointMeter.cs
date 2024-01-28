using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPointMeter : MonoBehaviour
{
    private PlayerStat stat;

    [SerializeField] private Slider greenMeter;
    [SerializeField] private Slider brownMeter;

    private void Awake()
    {
        stat = PlayerStat.instance;
    }

    private void Update()
    {
        greenMeter.value = stat.Wall_2_Point;
        brownMeter.value = stat.Wall_1_Point;
    }
}
