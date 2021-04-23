using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    
    // Start is called before the first frame update
    void Awake()
    {
        var spawnPos = new Vector3(0, player.transform.GetComponent<BoxCollider>().bounds.size.y, 0);
        player = Instantiate(player, spawnPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
