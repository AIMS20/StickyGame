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
    

    [SerializeField] private float moveSpeed = 1;
    [SerializeField] public float despawnDistance = 10f;
    [SerializeField] public float pushAttackDist = 2f;
    [SerializeField] public float defeatAttackDist = 0.5f;

    private GameObject policeCar;
    private Color poopBrown;
    private Renderer policeCarMat;
    private Vector3 policeCarPos;
    private Rigidbody rb;
    private Collider coll;
    private Vector3 chaseGoalPos;
    private Vector3 endGoalPos;
    private Vector3 currentGoal;

    public enum Modes{
        PUSH, DEFEATPLAYER, HIT
    }

    private Modes modeMode;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        chaseGoalPos = player.transform.position;
        
        policeCar = gameObject;
        rb = policeCar.GetComponent<Rigidbody>();
        coll = policeCar.GetComponent<BoxCollider>();
        
        policeCarMat = policeCar.transform.GetChild(0).transform.GetComponent<Renderer>();
        poopBrown = new Color(123f/255f, 69f/255f, 27f/255f);

        //initial speed which increases with gametime
        moveSpeed = GameManager.Instance.policeMoveSpeed; //TODO: refactor

    }

    // Update is called once per frame
    void Update()
    {
        truckController.SpinTires(tyres);
        
        UpdatePositions();

        
        //TODO: iterate through enums instead? readability would suck tho
        switch (modeMode)
        {
            //try to get in front of player
            case Modes.PUSH: //TODO: add swaying from side to side
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
                StartCoroutine(nameof(EndChase));
                break;
            
            //chase player until close enough to change into PUSH mode
            default:
                DefaultMode();
                break;
        }
        
        

    }

    public void DefaultMode()
    {
        currentGoal = chaseGoalPos;
        if (Vector3.Distance(policeCarPos, currentGoal) <= pushAttackDist)
        {
            modeMode = Modes.PUSH;
        }
    }

    private void HitMode()
    {
        //slow down
        moveSpeed -= 0.5f;

        if (Vector3.Distance(policeCarPos, chaseGoalPos) > despawnDistance){
            StartCoroutine(DestroyCar(policeCar));
        }

    }

    private IEnumerator SwayToSides()
    {
        yield return new WaitForSeconds(0);

    }

    private void PushMode()
    {
        currentGoal = endGoalPos;
        moveSpeed += 0.001f;

        if (Vector3.Distance(policeCarPos, currentGoal) < defeatAttackDist)
        {
            modeMode = Modes.DEFEATPLAYER;
        }
    }

    private void UpdatePositions()
    {
        var step = moveSpeed * Time.deltaTime;

        chaseGoalPos = player.transform.position;
        policeCarPos = policeCar.transform.position;
        endGoalPos = chaseGoalPos + new Vector3(0, 0, -coll.bounds.size.z * 1.5f);
        
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
            rb.AddForce(new Vector3(0, 0, PoopController.poopHitForce)); //TODO:  test
            
            //attach poop
            poop.position += policeCar.transform.position;
            
            //change mat
            policeCarMat.material.color = poopBrown;
            
            //unconstrain except for y pos
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            
            //sway  to side
            rb.AddTorque(transform.up * Random.Range(-350f, 350f), ForceMode.Impulse); 
            modeMode = Modes.HIT;
        }
    }

    IEnumerator DestroyCar(GameObject car)
    {
        yield return new WaitForSeconds(3);
        // print("DESTROYING" + car);
        Destroy(car);
        
    }  
    
    IEnumerator EndChase()
    {
        yield return new WaitForSeconds(3);
        DestroyCar(player);

        GameManager.gameOver = true;
        DestroyCar(policeCar);

    }
    
    
    
}
