using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladderTag : MonoBehaviour
{
    private AxeMovmentLogic m_Logic;
    private mainCam cam;
    private PlayerMovingScript player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovingScript>();
        m_Logic = FindObjectOfType<AxeMovmentLogic>();
        cam = FindObjectOfType<mainCam>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerObject")
        {
            cam.canMoveCam = false;
            player.canJump = false;
            m_Logic.cantloose = false;
            player.cantControl = false;
            player.centerCanBeMoved = true;
            m_Logic.StartBallAfterJump();
            m_Logic.checkingNumber = + 4.3f;
            this.transform.gameObject.tag = ("Untagged");
            m_Logic.findCurrentEnemy();
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            //player.UpdateStairsObj();
        }
    }
}
