using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlManager : MonoBehaviour
{
    public float GameSpeed { get; set; } = 1f;

    private void Update()
    {
        Time.timeScale = GameSpeed;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameSpeed--;
            if (GameSpeed <= 0)
            {
                GameSpeed = 0.5f;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameSpeed++;
        }
    }
}
