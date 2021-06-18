using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PoliceCarController : MonoBehaviour
{
    public GameObject player;
    public TruckController truckController;
    public List<Transform> tyres;
    public GameManager gameManager;
    

    [SerializeField] private int hitSwayAmount = 250;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] public float despawnDistance = 10f;
    [SerializeField] public float pushAttackDist = 2f;
    [SerializeField] public float defeatAttackDist = 0.5f;
    [SerializeField] public float speedIncreasePush = 0.005f;

    private GameObject policeCar;
    private Color poopBrown;
    private Renderer policeCarMat;
    private Vector3 policeCarPos;
    private Rigidbody rb;
    private Collider coll;
    private Vector3 chaseGoalPos;
    private Vector3 endGoalPos;
    private Vector3 currentGoal;

    private enum Modes{
        PUSH, DEFEATPLAYER, HIT
    }

    private Modes currentMode;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        chaseGoalPos = player.transform.position;
        
        policeCar = gameObject;
        rb = policeCar.GetComponent<Rigidbody>();
        coll = policeCar.GetComponent<BoxCollider>();
        
        policeCarMat = policeCar.transform.GetChild(0).transform.GetComponent<Renderer>();
        policeCarMat.material.color = Color.white;
        poopBrown = new Color(123f/255f, 69f/255f, 27f/255f);

        //initial speed which increases with gametime
        moveSpeed = GameManager.instance.policeStartSpeed; //TODO: refactor
        print(policeCar.name + " - CURR SPEED: "+ moveSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        truckController.SpinTires(tyres);
        
        UpdatePositions();
        

        
        //TODO: iterate through enums instead? readability would suck tho
        switch (currentMode)
        {
            //try to get in front of player
            case Modes.PUSH: 
                // print("PUSH MODE");
                PushMode();
                break;
            
            //being hit by player or poop
            case Modes.HIT:
                // print("HIT MODE");
                HitMode();
                break;
            
            //got in front of player
            case Modes.DEFEATPLAYER:
                // print("DEFEAT MODE");
                Invoke(nameof(EndChase), 1f);
                break;
            
            //chase player until close enough to change into PUSH mode
            default:
                DefaultMode();
                break;
        }
        
        

    }

    private void DefaultMode()
    {
        currentGoal = chaseGoalPos;
        if (Vector3.Distance(policeCarPos, currentGoal) <= pushAttackDist)
        {
            currentMode = Modes.PUSH;
        }
    }

    private void HitMode()
    {
        //slow down
        moveSpeed -= 0.5f;

        if (Vector3.Distance(policeCarPos, chaseGoalPos) > despawnDistance){
            Invoke(nameof(DestroyCarInvoke), 3f);
        }

    }

    private void PushMode()
    {
        currentGoal = endGoalPos;
        moveSpeed += speedIncreasePush;

        if (Vector3.Distance(policeCarPos, currentGoal) < defeatAttackDist)
        {
            currentMode = Modes.DEFEATPLAYER;
        }
    }

    private void UpdatePositions() 
    {
        var step = moveSpeed * Time.deltaTime;
        var collBounds = coll.bounds;

        chaseGoalPos = player.transform.position + new Vector3(0,collBounds.extents.y,0);
        policeCarPos = policeCar.transform.position;
        endGoalPos = chaseGoalPos + new Vector3(0, 0, -collBounds.size.z * 1.5f);
        
        //move to current goal
        policeCar.transform.position = Vector3.MoveTowards(policeCarPos, currentGoal, step);
    }
    
    
    private void OnCollisionEnter(Collision other){
        if (other.gameObject.CompareTag("Player") ){
            // attackMode = Attacks.HIT;
        }

        if (other.gameObject.CompareTag("Poop"))
        {
            var poop = other.transform;
            // poop.GetComponent<Rigidbody>().isKinematic = true;
            
            rb.AddExplosionForce(PoopController.poopExplForce, poop.position, 1f, 20f );
            rb.AddForce(new Vector3(0, 0, PoopController.poopHitForce));
            
            //attach poop
            poop.GetComponent<Rigidbody>().isKinematic = true;
            var poopColl = poop.GetComponent<MeshCollider>();
            poopColl.enabled = false;
            // var poopTransform = poop.transform;
            // poopTransform.position += new Vector3(0, 0, poopColl.bounds.size.z*3f);
            poop.transform.SetParent(this.transform);
            
            //change mat
            policeCarMat.material.color = poopBrown;
            
            //unconstrain except for y pos
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            
            //sway  to side
            rb.AddTorque(transform.up * Random.Range(-hitSwayAmount, hitSwayAmount), ForceMode.Impulse); 
            currentMode = Modes.HIT;
        }
    }
    
    void DestroyCarInvoke()
    {
        Destroy(policeCar);
    }  
    
    void EndChase()
    {
        if (GameManager.gameOver) return;
    

        GameManager.EndRace();
        // Destroy(policeCar);
    }

}
