using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public GameObject poop;
    [SerializeField] float maxTurn;
    
    private GameObject truck;
    private Rigidbody rb;
    private float axisInput;
    [SerializeField] public GameObject poopSpawnObj;
    [SerializeField] public Vector3 poopSpawn;
    [SerializeField] private float turnStrength;
    [SerializeField] private float rotateStrength;
    
    [SerializeField] List<Transform> tyres;
    // [SerializeField] List<Rigidbody> FrontTyresRB;
    
    private float rotationSpeed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        truck = gameObject;
        rb = truck.transform.GetComponent<Rigidbody>();

        poopSpawn = poopSpawnObj.transform.position; //TODO: fix me
        print(poopSpawn);
        // FrontTyresRB.Add(tyres[2].GetComponent<Rigidbody>());
        // FrontTyresRB.Add(tyres[3].GetComponent<Rigidbody>());
        //
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        SpinTires();
    }

    private void SpinTires()
    {
        foreach (var tyre in tyres){
            tyre.transform.Rotate(-rotationSpeed, 0, 0, Space.Self);
        }
    }

    private void UpdateInput()
    {
        
        //get horizontal input
        axisInput = Input.GetAxis("Horizontal");
        // print(axisInput);

        // if (axisInput < -maxTurn){
        //     axisInput = -maxTurn;
        // }
        //
        // if (axisInput > maxTurn){
        //     axisInput = maxTurn;
        // }

  
        rb.AddForce(new Vector3(-axisInput * turnStrength, 0, 0));
        truck.transform.Rotate(new Vector3(0, axisInput / rotateStrength, 0));
            
        

        print(axisInput);
        
        //spawn poop on A
        if (Input.GetButtonDown("Poop")){
            print("POOPING");

            Instantiate(poop, poopSpawn, Quaternion.identity);
        }
    }
}
