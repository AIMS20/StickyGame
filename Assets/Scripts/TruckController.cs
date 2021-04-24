using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public GameObject poop;
    [SerializeField] float maxTurn;
    
    [SerializeField] public GameObject poopSpawnObj;
    [SerializeField] public Vector3 poopSpawn;
    [SerializeField] private float poopCooldown;
    
    [SerializeField] private float moveStrength;
    [SerializeField] private float rotateDamper;
    [SerializeField] List<Transform> tyres;
    
    // [SerializeField] List<Rigidbody> FrontTyresRB; //TODO: use to rotate front wheels in axis with maxturn
    
    private GameObject truck;
    private Rigidbody rb;
    private float axisInput;
    private float currentCooldown;
    private float rotationSpeed = 5f;
    private bool canPoop;

    // Start is called before the first frame update
    void Start()
    {
        truck = gameObject;
        rb = truck.transform.GetComponent<Rigidbody>();

        print(poopSpawn);
        // FrontTyresRB.Add(tyres[2].GetComponent<Rigidbody>());
        // FrontTyresRB.Add(tyres[3].GetComponent<Rigidbody>());
        //
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateInput();
        SpinTires(); 
    }
    
    void Update()
    {
        poopSpawn = poopSpawnObj.transform.position;
        currentCooldown -= Time.deltaTime;

        if (currentCooldown <= 0){
            canPoop = true;
            currentCooldown = poopCooldown;
        }
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

  
        rb.AddForce(new Vector3(-axisInput * moveStrength, 0, 0));
        truck.transform.Rotate(new Vector3(0, axisInput / rotateDamper, 0));
            
        

        print(axisInput);
        
        //spawn poop on A
        if (canPoop &&  Input.GetButtonDown("Poop")){
            print("POOPING");
                
            Instantiate(poop, poopSpawn, Quaternion.AngleAxis(-90, new Vector3(1,0,0)));
            canPoop = false;
        }
    }
}
