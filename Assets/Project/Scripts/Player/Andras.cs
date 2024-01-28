using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ.Ingame.Controller;

public class Andras : MonoBehaviour
{
    [SerializeField] MouseInputManager mouseInput;
    [SerializeField] DrawColorSwitchController drawColorSwitchController;

    [SerializeField] Vector3 offset;

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite drawing;
    [SerializeField] Sprite idle;

    void Update()
    {
        transform.position = mouseInput.GetMousePos() + offset;

        if (drawColorSwitchController._currentIndex == 0)
            spriteRenderer.sprite = idle;
        else
            spriteRenderer.sprite = drawing;
    }


}
