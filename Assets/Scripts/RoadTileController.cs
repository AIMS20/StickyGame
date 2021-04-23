using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoadTileController : MonoBehaviour
{
    private GameObject player;
    private GameObject tile;
    private float maxDistance;

    
    // Start is called before the first frame update
    void Start()
    {
        tile = gameObject;
        player = GameObject.FindWithTag("Player");
        maxDistance = tile.GetComponent<BoxCollider>().size.z * GameManager.maxDistanceMod;
        var rb = tile.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0,0,GameManager.tileSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        checkDistance();
    }

    private void checkDistance()
    {
        var currentDistance = Vector3.Distance(player.transform.position, tile.transform.position);
        print(currentDistance);
        if (currentDistance > maxDistance)
        {
            Destroy(tile);
        }
    }
}
