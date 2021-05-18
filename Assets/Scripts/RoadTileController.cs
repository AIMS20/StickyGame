using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class RoadTileController : MonoBehaviour
{
    public GameManager gameManager;
    private GameObject tile;
    private float maxDistance;
    

    // Start is called before the first frame update
    void Start()
    {
        tile = gameObject;

        
        maxDistance = tile.GetComponent<BoxCollider>().size.z * gameManager.maxDistanceRoadMod;
        var rb = tile.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0,0,global::GameManager.tileSpeed * tile.GetComponent<Rigidbody>().mass));
    }



    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        var currentDistance = Vector3.Distance(gameManager.transform.position, tile.transform.position);
        // print(currentDistance);
        if (currentDistance > maxDistance)
        {
            // Destroy(tile); //TODO: fix me
            tile.SetActive(false);
        }
    }
}
