using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] GameObject Heart;

    [SerializeField] List<GameObject> hearts;

    int currentUIHeart;

    PlayerStat playerStat;

    private void Start()
    {
        currentUIHeart = 4;
        playerStat = PlayerStat.i;
    }

    private void Update()
    {
        if (playerStat.Health != currentUIHeart + 1)
        {
            if (currentUIHeart == -1)
                return;
            if (playerStat.Health < currentUIHeart + 1)
            {
                hearts[currentUIHeart].SetActive(false);
                currentUIHeart--;
            }
            else
            {
                hearts[currentUIHeart + 1].SetActive(true);
                currentUIHeart++;
            }
        }
    }
}
