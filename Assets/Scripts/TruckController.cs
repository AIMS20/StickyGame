using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public GameObject poop;
    
    private GameObject truck;
    private Transform animal;
    private Vector3 poopSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        truck = gameObject;
        animal = truck.transform.Find("Animal");
        poopSpawn = animal.GetChild(0).transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        //spawn poop on A
        if (Input.GetButtonDown("Poop")){
            print("POOPING");

            Instantiate(poop, poopSpawn, Quaternion.identity);
        }
    }
}
