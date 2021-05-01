using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopController : MonoBehaviour
{
    private GameObject poop;
    [SerializeField] public static float poopHitForce = 15f;
    [SerializeField] public static float poopExplForce = 15f;
    
    [SerializeField] private float poopForce = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        poop = gameObject;
        
        //initial push
        poop.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,poopForce));

        
        Invoke(nameof(DestroyPoop), 3f);
    }

    // Update is called once per frame
    void Update()
    {
    }


    void DestroyPoop(){
        Destroy(poop);
    }
}
