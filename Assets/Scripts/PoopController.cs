using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopController : MonoBehaviour
{
    private GameObject poop;
    private Rigidbody rb;
    private Collider coll;
    [SerializeField] public static float poopHitForce = 15f;
    [SerializeField] public static float poopExplForce = 15f;
    
    [SerializeField] private float poopForce = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        poop = gameObject;
        rb = poop.GetComponent<Rigidbody>();
        coll = poop.GetComponent<Collider>();
        
        
        //initial push
        rb.AddForce(new Vector3(0,0,poopForce), ForceMode.VelocityChange);

        //destroy after 3s
        Invoke(nameof(DestroyPoop), 3f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Road")){
            
            //TODO: find another fix than this for weird "sidepoop"
            rb.AddForce(new Vector3(0,poopForce/2f,poopForce*85f));
            // poop.transform.SetParent(other.transform, true);

            // print("COLLISION WITH ROAD");

        }
    }

    void DestroyPoop(){
        Destroy(poop);
    }
}
