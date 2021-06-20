using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TruckController : MonoBehaviour
{
    public GameObject poop;
    [SerializeField] float maxTurn;
    
    [SerializeField] public GameObject poopSpawnObj;
    [SerializeField] public Vector3 poopSpawn;
    [SerializeField] private float poopCooldown;
    
    [SerializeField] private float moveStrength;
    [SerializeField] private float inputAxisMod;
    [SerializeField] List<Transform> tyres;
    [SerializeField] List<String> poopMessages;
    
    // [SerializeField] List<Rigidbody> FrontTyresRB; //TODO: use to rotate front wheels in axis with maxturn
    
    private GameObject truck;
    private Rigidbody rb;
    private float axisInput;
    private float currentCooldown;
    private float rotationSpeed = 5f;
    private bool canPoop;
    private bool isDisplayingMsg;
    private SimpleHelvetica poopTimer;
    private float timerText;

    // Start is called before the first frame update
    void Start()
    {
        truck = gameObject;
        rb = truck.transform.GetComponent<Rigidbody>();

        // print(poopSpawn);
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
        SpinTires(tyres); 
    }
    
    void Update()
    {
        UpdateInput();
        poopSpawn = poopSpawnObj.transform.position;
        currentCooldown -= Time.deltaTime;

        PoopLogic();
        
    }

    private void PoopLogic()
    {
        if (GameManager.gameOver)
        {
            poopTimer.Text = " ";
            poopTimer.GenerateText();
            return;
        }
        
        if (currentCooldown < 1)
        {
            if (!isDisplayingMsg)
            {
                poopTimer.transform.localPosition = new Vector3(10f, 3f, 1);
                poopTimer.Text = poopMessages[Random.Range(0, poopMessages.Count)];
                isDisplayingMsg = true;
            }
            canPoop = true;
            currentCooldown = poopCooldown;
        }
        else if (!canPoop)
        {
            timerText = (int) currentCooldown;
            poopTimer.transform.localPosition = new Vector3(-0.2f, 3f, 1);
            poopTimer.Text = $"{timerText.ToString(CultureInfo.CurrentCulture)}";
            isDisplayingMsg = false;
        }

        poopTimer.GenerateText();
    }

    public void SpinTires(List<Transform> allTyres)
    {
        foreach (var tyre in allTyres){
            tyre.transform.Rotate(-rotationSpeed * moveStrength/100f, 0, 0, Space.Self);
        }
    }

    private void UpdateInput()
    {
        
        //get horizontal input
        axisInput = Input.GetAxis("Horizontal");
        
        //move on X
        rb.AddForce(new Vector3(-axisInput * moveStrength, 0, 0));

        //rotate on Y
        var localRot = axisInput * inputAxisMod ;
        
        var currRot = truck.transform.rotation.eulerAngles.y;
        
        truck.transform.Rotate(new Vector3(0,localRot , 0) * Time.deltaTime);
        if (currRot < 360f-maxTurn && currRot > maxTurn*2)//TODO: temp
        {
            // print("true1");
            truck.transform.rotation = new Quaternion(0f, -maxTurn, 0f,0); 
        }
        if (currRot > maxTurn && currRot < 360f-maxTurn*2)
        {
            // print("true2");
            truck.transform.rotation = new Quaternion(0f, maxTurn, 0f, 0);
        }
 
        if (Mathf.Approximately(currRot, 180f))
        {
            truck.transform.rotation = new Quaternion(0, 0, 0, 0);  
        }
        // else
        // {
        //     currRot = Mathf.Min(currRot, maxTurn);
        //     // truck.transform.LookAt(gameManager.transform); //TODO: fix clamping of rotation
        //     // truck.transform.rotation = new Quaternion(0f, Mathf.Clamp(currRot, -maxTurn, maxTurn), 0f, 0f);
        // }
        // print(axisInput);
        
        //spawn poop on A
        if (canPoop && Input.GetButtonDown("Poop")){
            // print("POOPING");
                
            Instantiate(poop, poopSpawn, Quaternion.identity);
            canPoop = false;
        }
        
        if (GameManager.gameOver && Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
