using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToRoad : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Road")){
            gameObject.transform.SetParent(other.transform.parent, true);
            print(gameObject.name + " colliding with "+ other.transform.name);
        }
    }
}
