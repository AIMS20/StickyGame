using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cinemachine.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] List<String> poopMessages;
    
    // [SerializeField] List<Rigidbody> FrontTyresRB; //TODO: use to rotate front wheels in axis with maxturn
    
    private GameObject truck;
    private Rigidbody rb;
    private float axisInput;
    private float currentCooldown;
    private float rotationSpeed = 5f;
    private bool canPoop;
    private SimpleHelvetica poopTimer;
    private float t;
    private float rotationClamp;
    private bool messageIsShown = false;

    // Start is called before the first frame update
    void Start()
    {
        truck = gameObject;
        rb = truck.transform.GetComponent<Rigidbody>();

        print(poopSpawn);
        // FrontTyresRB.Add(tyres[2].GetComponent<Rigidbody>());
        // FrontTyresRB.Add(tyres[3].GetComponent<Rigidbody>());
        //

        currentCooldown = poopCooldown;
        poopTimer = truck.GetComponentInChildren<SimpleHelvetica>();
        poopTimer.Text = "test";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SpinTires(); 
    }
    
    void Update()
    {
        UpdateInput();
        poopSpawn = poopSpawnObj.transform.position;
        currentCooldown -= Time.deltaTime;

        PoopLogic();

        //rotation max //TODO
        // transform.Rotate(0, 0, -truck.transform.localRotation.y, Space.Self);
        // var transformEulerAngles = truck.transform.eulerAngles;
        // transformEulerAngles.y = Mathf.Clamp(transform.eulerAngles.y, -maxTurn, maxTurn);
        // truck.transform.Rotate(transformEulerAngles);
    }

    private void PoopLogic()
    {
        if (currentCooldown <= 0)
        {
            canPoop = true;
            currentCooldown = poopCooldown;
        }

        if (!canPoop)
        {
            t = (int) currentCooldown;
            poopTimer.transform.localPosition = new Vector3(-0.2f, 3f, 0);
            poopTimer.Text = $"{t.ToString(CultureInfo.CurrentCulture)}";
            poopTimer.GenerateText();
        }

        if (canPoop && !messageIsShown) //TODO: fix this dirty shit
        {
            {
                poopTimer.transform.localPosition = new Vector3(10f, 3f, 0);
                poopTimer.Text = poopMessages[Random.Range(0, poopMessages.Count)];
            }
            poopTimer.GenerateText();
            messageIsShown = true;
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
            
        

        // print(axisInput);
        
        //spawn poop on A
        if (canPoop &&  Input.GetButtonDown("Poop")){
            print("POOPING");
                
            Instantiate(poop, poopSpawn, Quaternion.AngleAxis(-90, new Vector3(1,0,0)));
            canPoop = false;
            messageIsShown = false; 
        }
    }
}
