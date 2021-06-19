using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class RoadTileController : MonoBehaviour
{
    public GameManager gameManager;
    private GameObject tile;
    [SerializeField]private float maxDistance;
    

    // Start is called before the first frame update
    void Start()
    {
        tile = gameObject;

        maxDistance = gameManager.maxDistanceRoadMod;
    }



    // Update is called once per frame
    void Update()
    {
        UpdateDistance();
    }

    private void UpdateDistance()
    {
        var tilePos = tile.transform.position;
        tile.transform.position = Vector3.MoveTowards(tilePos, new Vector3(0,0,maxDistance), gameManager.tileSpeed*Time.deltaTime);

        print("roadtile SPEED: "+gameManager.tileSpeed*gameManager.tileSpawnSeconds*Time.deltaTime);
        if (tilePos.z > (gameManager.transform.position.z + maxDistance))
        {
            // Destroy(tile); 
            tile.SetActive(false);
            //TODO: move back?
        }
    }
}
