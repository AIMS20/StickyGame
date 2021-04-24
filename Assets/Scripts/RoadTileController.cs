using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoadTileController : MonoBehaviour
{
    [SerializeField] public GameObject GameManager;
    private GameObject tile;
    private float maxDistance;

    
    // Start is called before the first frame update
    void Start()
    {
        tile = gameObject;
;
        maxDistance = tile.GetComponent<BoxCollider>().size.z * global::GameManager.maxDistanceMod;
        var rb = tile.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0,0,global::GameManager.tileSpeed * tile.GetComponent<Rigidbody>().mass));
    }

    // Update is called once per frame
    void Update()
    {
        checkDistance();
    }

    private void checkDistance()
    {
        var currentDistance = Vector3.Distance(GameManager.transform.position, tile.transform.position);
        // print(currentDistance);
        if (currentDistance > maxDistance)
        {
            Destroy(tile); //TODO: fix me
        }
    }
}
