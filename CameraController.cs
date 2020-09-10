using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        if(player.transform.position.y > transform.position.y) // if the player goes above half way in the cameras view
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }


    }
}
