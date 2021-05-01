using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Timers;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    // public static GameManager Instance => instance; //get return

    //player
    public GameObject player;
    [NonSerialized] public Rigidbody playerRb;
    
    //police
    public GameObject policeCar;
    private List<GameObject> policeCars;
    [SerializeField] public int policeCarsMax = 5;
    [SerializeField] public float policeSpawnSeconds = 10f;
    [SerializeField] Vector3 policeSpawnPosOffset;
    public float policeMoveSpeed { get; private set; } //TODO: find out how to serialize
    
    //road
    public GameObject roadTile;
    [SerializeField] Vector3 roadSpawnposOffset;
    [SerializeField] public static float tileSpeed = 500f;
    [SerializeField] public float tileSpawnSeconds = 0.95f;
    [SerializeField] public float maxDistanceRoadMod = 6f;
    
    //cam
    public GameObject camTarget;
    public CinemachineVirtualCamera vcam;
    
    //time
    [SerializeField] float timeToStart;
    [NonSerialized] public float currentTime;
    private float timer;
    private SimpleHelvetica timerText;
    private SimpleHelvetica restartText;
    [SerializeField] private Vector3 timerVCamTargetPos;
    
    //game
    private static bool gameStarted;
    [NonSerialized] public static bool gameOver;

    
    // Start is called before the first frame update
    void Awake()
    {
        InstanceCheck();

        policeMoveSpeed = 0.25f;
        currentTime = 0f;
        gameStarted = false;
        gameOver = false;

        //initialize timer
        timerText = gameObject.GetComponentInChildren<SimpleHelvetica>();
        timerText.Text = "0";
        timerText.GenerateText();

        restartText = Instantiate(timerText);
        restartText.Text = " ";
        restartText.GenerateText();

        //spawn road tiles
        InvokeRepeating(nameof(SpawnRoads), 0f, tileSpawnSeconds);
        
        //spawn player
        var playerSpawnpos = new Vector3(0, player.transform.GetComponent<BoxCollider>().bounds.size.y, 0);
        player = Instantiate(player, playerSpawnpos, Quaternion.identity);
        playerRb = player.GetComponent<Rigidbody>();
        playerRb.useGravity = false;
        
        // rb.constraints = RigidbodyConstraints.FreezeAll; //TODO: fix with correct rotation

        //spawn police cars
        policeCars = new List<GameObject>();
        policeSpawnPosOffset = playerSpawnpos + policeSpawnPosOffset;

        InvokeRepeating(nameof(SpawnPoliceCars), 0f, policeSpawnSeconds);

    }

    private void Start(){
        Invoke(nameof(StartRace), timeToStart);

    }

    private void InstanceCheck()
    {
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void SpawnRoads()
    {
        // print("ROAD SPAWN POS: "+roadSpawnpos);
        Instantiate(roadTile, roadSpawnposOffset, Quaternion.identity);
        
    }
    
    private void SpawnPoliceCars()
    {
        policeSpawnPosOffset.x -= 4; //TODO: policecar width or random instead
        
        // print("POLICE SPAWN AT: "+policeSpawnpos.x);
        
        //reset spawn
        if (policeSpawnPosOffset.x <= -12) //TODO: fix magic numbas
        {
            policeSpawnPosOffset.x = 12;
            policeSpawnPosOffset.z += 5;
        }

        
        if ((policeCars.Count < policeCarsMax || policeCarsMax == 0) && !gameOver){
            
            policeCars.Add(Instantiate(policeCar, policeSpawnPosOffset,
                Quaternion.AngleAxis(-180, 
                new Vector3(0,1,0))));
            
            // print("SPAWNING POLICE CAR AT "+policeSpawnpos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            UpdateTimer();
            UpdatePoliceSpeed();
        }
        
        UpdateCamTarget();
        
    }

    private void UpdatePoliceSpeed()
    {
        policeMoveSpeed += 0.00025f;
        // print("curr police speed: "+policeMoveSpeed);

    }

    private void UpdateTimer()
    {
        if (!gameStarted)
        {
            timer = (int) timeToStart - Time.deltaTime;
        }
        else
        {
            currentTime += Time.deltaTime;
            timer = (int) currentTime;
        }

        timerText.Text = $"{timer.ToString(CultureInfo.CurrentCulture)}";
        timerText.GenerateText();
    }

    private void UpdateCamTarget()
    {
        var currentCamTarget = gameOver ? instance.transform.position + timerVCamTargetPos : player.transform.position;
        camTarget.transform.position = currentCamTarget;
        
    }

    private void StartRace()
    {
        //unfreeze pos X and rot y
        playerRb.constraints = RigidbodyConstraints.None;
        playerRb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        gameStarted = true;
    }

    public static void EndRace() //TODO: magic numbas
    {
        //freeze player 
        instance.playerRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        
        //move timer to middle, change into highscore
        var timerTransform = instance.timerText.transform;
        timerTransform.position = new Vector3(3, 3.15f, 1);
        timerTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        instance.timerVCamTargetPos = new Vector3(-timerTransform.position.x*2.25f, 0, -2);
        
        instance.timerText.Text = "They got you!\nHighscore: "+(int)instance.currentTime;
        instance.timerText.GenerateText();
        
        
        //create restart-text, transform and generate
        var restartTransform = instance.restartText.transform;
        instance.restartText.Text = "Press Y to exit";
        restartTransform.localPosition = timerTransform.position + new Vector3(5, -1, 17);
        restartTransform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        // instance.restartText.GetComponent<Renderer>().material.color = Color.red;
        instance.restartText.transform.localRotation = Quaternion.Euler(0, 90, 0);
        instance.restartText.GenerateText();


        print("----ENDGAME----");
        gameOver = true;
    }
}
