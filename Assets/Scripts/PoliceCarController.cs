using System.Collections;
using System.Collections.Generic;
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
    private Transform moveGoal;
    private bool isAttacking = false;
    private bool isHit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        policeCar = gameObject;
        rb = policeCar.GetComponent<Rigidbody>();

        moveGoal = player.transform.Find("Animal");

    }

    // Update is called once per frame
    void Update()
    {

        policeCarPos = policeCar.transform.position;
        var step = moveSpeed * Time.deltaTime;
        policeCar.transform.position = Vector3.MoveTowards(policeCarPos, moveGoal.position, step);
 
        if (policeCar.transform.position.z <= moveGoal.position.z)
        {
            print("POLICE IN REACH");
            moveSpeed += 0.1f;
            isAttacking = true;
        }

        if (isAttacking || isHit){ //TODO: is hit by poop...
            if (Vector3.Distance(policeCarPos, moveGoal.position) > despawnDistance)
            {
                StartCoroutine(nameof(DestroyCar));
            }
        }
    }

    IEnumerator DestroyCar()
    {
        yield return new WaitForSeconds(3);
        print("DESTROYING");
        Destroy(policeCar);
        
    }
    
    
    
}
