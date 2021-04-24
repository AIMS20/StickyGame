using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.WSA;

public class GameManager : MonoBehaviour
{
    //player
    public GameObject player;
    [NonSerialized] public Rigidbody rb;
    
    //police
    public GameObject policeCar;
    private List<GameObject> policeCars;
    [SerializeField] public int policeCarsMax;
    [SerializeField] Vector3 policeSpawnpos;
    
    //road
    public GameObject roadTile;
    public GameObject camTarget;
    [SerializeField] Vector3 roadSpawnpos;
    [SerializeField] public static float tileSpeed = 500f;
    [SerializeField] public float tileSpawnSpeed = 1.05f;
    [SerializeField] public static float maxDistanceMod = 3f;
    
    //time
    [SerializeField] float timeToStart;
    [NonSerialized] public float currentTime;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        
        currentTime = 0f;
        var roadLength = roadTile.transform.GetComponent<BoxCollider>().size.z;
        print("ROADLENGTH: "+roadLength);

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
        policeSpawnpos = playerSpawnpos + new Vector3(0, policeCar.transform.GetComponent<BoxCollider>().bounds.size.y*2, 20);
        
        InvokeRepeating(nameof(SpawnPoliceCars), 0f, 10f);

    }

    private void SpawnRoads()
    {
        // print("ROAD SPAWN POS: "+roadSpawnpos);
        Instantiate(roadTile, roadSpawnpos, Quaternion.identity);
        
    }
    
    private void SpawnPoliceCars()
    {
        if (policeCars.Count < policeCarsMax){ //TODO: increment to up difficulty
            policeCars.Add(Instantiate(policeCar, policeSpawnpos,Quaternion.AngleAxis(-180, new Vector3(0,1,0))));
            print("SPAWNING POLICE CAR AT "+policeSpawnpos);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //update followcam
        camTarget.transform.position = player.transform.position;
        
        //start race after x //TODO: needed?
        currentTime += Time.deltaTime;
        if (currentTime > timeToStart){
            StartRace();
        }
    }

    private void StartRace()
    {
        rb.useGravity = true;
    }
}
