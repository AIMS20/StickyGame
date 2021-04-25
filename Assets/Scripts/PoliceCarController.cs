using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class PoliceCarController : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private float moveSpeed = 1;
    [SerializeField] public float despawnDistance = 10f;
    
    private GameObject policeCar;
    private Vector3 policeCarPos;
    private Rigidbody rb;
    private BoxCollider mainCollider;
    private Transform moveGoal;
    private Vector3 endGoal;
    private Vector3 currentGoal;
    private bool isAttacking = false;
    private bool isHit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        policeCar = gameObject;
        rb = policeCar.GetComponent<Rigidbody>();
        mainCollider = policeCar.GetComponent<BoxCollider>();

        moveGoal = player.transform.Find("Animal");
        endGoal = new Vector3(moveGoal.transform.position.x, moveGoal.transform.position.y,  moveGoal.transform.localPosition.z - 2f);

    }

    // Update is called once per frame
    void Update()
    {

        policeCarPos = policeCar.transform.position;
        var step = moveSpeed * Time.deltaTime;

        if (!isAttacking){
            currentGoal = moveGoal.position;
        }
        else{
            currentGoal = endGoal;
        }
 
        if (policeCar.transform.position.z <= moveGoal.position.z)
        {
            print("POLICE IN REACH");
            isAttacking = true;
            moveSpeed += 0.001f;
        }
        
        if (policeCar.transform.position.z < currentGoal.z)
        {
            print("POLICE GOT YOU");
            StartCoroutine(nameof(EndChase));

        }

        policeCar.transform.position = Vector3.MoveTowards(policeCarPos, currentGoal, step);
        
        //increase speed
        moveSpeed += 0.0025f;
        // print("cops speed"+moveSpeed);

        if (isAttacking || isHit){ //TODO: is hit by poop or player...
            if (Vector3.Distance(policeCarPos, moveGoal.position) > despawnDistance)
            {
                StartCoroutine((DestroyCar(policeCar)));
            }
        }
    }


    private void OnCollisionEnter(Collision other){
        if (other.gameObject.CompareTag("Player") ){
            print("THIS IS MY NO-NO SQUARE");
            isHit = true;
        }

        if (other.gameObject.CompareTag("Poop"))
        {
            other.transform.GetComponent<Rigidbody>().isKinematic = false;
            isHit = true;
        }
    }

    IEnumerator DestroyCar(GameObject car)
    {
        yield return new WaitForSeconds(3);
        print("DESTROYING");
        Destroy(car);
        
    }  
    
    IEnumerator EndChase()
    {
        yield return new WaitForSeconds(3);
        DestroyCar(player);

        GameManager.gameEnded = true;
        DestroyCar(policeCar);

    }
    
    
    
}
