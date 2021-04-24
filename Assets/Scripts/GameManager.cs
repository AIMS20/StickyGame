using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Rigidbody rb;
    public GameObject roadTile;
    public GameObject camTarget;
    [SerializeField] Vector3 roadSpawnpos;
    [SerializeField] public static float tileSpeed = 500f;
    [SerializeField] public static float maxDistanceMod = 3f;
    [SerializeField] float timeToStart;
    public float currentTime;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        
        currentTime = 0f;
        var roadLength = roadTile.transform.GetComponent<BoxCollider>().size.z;
        print("ROADLENGTH: "+roadLength);

        //spawn road tiles
        InvokeRepeating(nameof(SpawnRoads), 0f, 1f);
        
        // for (var i = 0; i < roadTileCount; i++)
        // {
        //     roadSpawnposNew = new Vector3(0, 0, roadSpawnposNew.z-roadLength);
        // }
        
        //spawn player
        var playerSpawnpos = new Vector3(0, player.transform.GetComponent<BoxCollider>().bounds.size.y, 0);
        player = Instantiate(player, playerSpawnpos, Quaternion.identity);

        rb = player.GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void SpawnRoads()
    {
        print("ROAD SPAWN POS: "+roadSpawnpos);
        Instantiate(roadTile, roadSpawnpos, Quaternion.identity);
        
    }

    // Update is called once per frame
    void Update()
    {
        camTarget.transform.position = player.transform.position;
        
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
