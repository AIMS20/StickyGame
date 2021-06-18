using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSettings : MonoBehaviour
{
    [SerializeField] public static float policeIncreaseSpeed = 0;
    [SerializeField] public static float policeStartSpeed = 0;
    [SerializeField] public static float policeSpawnSeconds = 0;


    public static void UpdateDiff(int difficulty){
        switch (difficulty){
            case 0:
                policeIncreaseSpeed = 0.0001f;
                policeStartSpeed = 0.05f;
                policeSpawnSeconds = 15f;
                break;
            case 1:
                policeIncreaseSpeed = 0.001f;
                policeStartSpeed = 0.075f;
                policeSpawnSeconds = 7f;
                break;
            case 2:
                policeIncreaseSpeed = 0.0075f;
                policeStartSpeed = 0.1f;
                policeSpawnSeconds = 4.25f;
                break;

        }
    }
}
