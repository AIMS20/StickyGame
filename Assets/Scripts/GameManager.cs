using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //player
    public GameObject player;
    [NonSerialized] public Rigidbody rb;
    
    //police
    public GameObject policeCar;
    private List<GameObject> policeCars;
    [SerializeField] public int policeCarsMax = 5;
    [SerializeField] public float policeSpawnRate = 10f;
    [SerializeField] Vector3 policeSpawnpos;
    
    //road
    public GameObject roadTile;
    public BoxCollider roadTileCollider;
    public Transform curbR;
    public Transform curbL;
    public GameObject camTarget;
    [SerializeField] Vector3 roadSpawnposOffset;
    [SerializeField] public static float tileSpeed = 500f;
    [SerializeField] public float tileSpawnSpeed = 1.05f;
    [SerializeField] public static float maxDistanceMod = 4f;
    
    //time
    [SerializeField] float timeToStart;
    [NonSerialized] public float currentTime;
    private float t;
    private static bool gameStarted;
    [NonSerialized] public static float moveSpeed = 0.1f;
    [NonSerialized] public static bool gameEnded;
    [NonSerialized] public static bool gameOver; //Todo: refactor
    private SimpleHelvetica timer;

    
    
    
    // Start is called before the first frame update
    void Awake()
    {
        
        currentTime = 0f;
        gameStarted = false;
        gameEnded = false;
        timer = gameObject.GetComponentInChildren<SimpleHelvetica>();

        int tmp = (int)timeToStart;
        timer.Text = tmp.ToString();
        timer.GenerateText();
        
        var roadLength = roadTile.transform.GetComponent<BoxCollider>().size.z;
        print("ROADLENGTH: "+roadLength);
        roadTileCollider = roadTile.GetComponent<BoxCollider>();
        curbR = roadTile.transform.GetChild(0); //TODO: make relative
        curbL = roadTile.transform.GetChild(1);

        //spawn road tiles
        InvokeRepeating(nameof(SpawnRoads), 0f, tileSpawnSpeed);
        
        
        // for (var i = 0; i < roadTileCount; i++)
        // {
        //     roadSpawnposNew = new Vector3(0, 0, roadSpawnposNew.z-roadLength);
        // }
        
        //spawn player
        var playerSpawnpos = new Vector3(0, player.transform.GetComponent<BoxCollider>().bounds.size.y, 0);
        player = Instantiate(player, playerSpawnpos, Quaternion.identity);
        rb = player.GetComponent<Rigidbody>();
        rb.useGravity = false;

        //spawn police cars
        policeCars = new List<GameObject>();
        policeSpawnpos = playerSpawnpos + new Vector3(12, 1f, 20);

        InvokeRepeating(nameof(SpawnPoliceCars), 0f, policeSpawnRate);

    }

    private void SpawnRoads()
    {
        // print("ROAD SPAWN POS: "+roadSpawnpos);
        Instantiate(roadTile, roadSpawnposOffset, Quaternion.identity);
        
    }
    
    private void SpawnPoliceCars()
    {
        policeSpawnpos.x -= 4; //TODO: policecar width instead
        
        print("TEST: "+policeSpawnpos.x);
        
        //reset spawn
        if (policeSpawnpos.x <= -12) //TODO: fix hardcoding
        {
            policeSpawnpos.x = 12;
            policeSpawnpos.z += 5;
        }

        
        if (policeCars.Count < policeCarsMax || policeCarsMax == 0){ //TODO: increment to up difficulty
            policeCars.Add(Instantiate(policeCar, policeSpawnpos,Quaternion.AngleAxis(-180, new Vector3(0,1,0))));
            print("SPAWNING POLICE CAR AT "+policeSpawnpos);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;
        
        moveSpeed += 0.00025f;

        //update followcam
        camTarget.transform.position = player.transform.position;
        
        //start race after x 
        currentTime += Time.deltaTime;
        if (currentTime > timeToStart){
            StartRace();
        }

        if (!gameStarted)
        {
            t = (int)timeToStart - Time.deltaTime;
        }
        else
        {
            t = (int)currentTime;

        }
        timer.Text = $"{t.ToString(CultureInfo.CurrentCulture)}";
        timer.GenerateText();

        if (gameEnded)
        {
            EndRace();
            if (Input.GetButtonDown("Restart"))
            {
                SceneManager.LoadScene("Sticky1");
            }

        }
    }

    private void StartRace()
    {
        rb.useGravity = true;
        gameStarted = true;
    }

    private void EndRace()
    {
        Time.timeScale = 0f;
        timer.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 5, gameObject.transform.position.z);
        timer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); //TODO: magic numba
        timer.Text = "They got you!\nHighscore: "+(int)currentTime;
        timer.GenerateText();
        print("----ENDGAME----");
        gameOver = true;
    }
}
