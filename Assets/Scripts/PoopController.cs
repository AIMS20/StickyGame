using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopController : MonoBehaviour
{
    private GameObject poop;
    private Rigidbody rb;
    
    [SerializeField] private float poopForce = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        poop = gameObject;
        
        //initial push
        poop.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,poopForce));

        rb = poop.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
