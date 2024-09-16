using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCam : MonoBehaviour
{
    PlayerMovingScript player;

    public bool canMoveCam = false;
    float playerStartrPos;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovingScript>();

        playerStartrPos = player.transform.position.x;
    }
    private void Update()
    {
        if (canMoveCam)
        {
            float valueX = player.transform.position.x - playerStartrPos;
            this.transform.position = new Vector3((valueX), transform.position.y, transform.position.z);
        }
    }
}
