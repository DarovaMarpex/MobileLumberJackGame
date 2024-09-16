using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using Color = UnityEngine.Color;

public class PlayerMovingScript : MonoBehaviour
{
    [SerializeField] private float speed;

    public GameObject stairs;

    public Rigidbody2D RB;

    public Animator animator;
    private Vector2 playerMove;
    private Rigidbody2D rb;
    public bool canMove;
    public bool canJump = false;
    public bool cantControl;
    public bool centerCanBeMoved = false;

    public mainCam cam;

    public Vector3 center;

    public Vector3 startingPos;

    // Update is called once per frame

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam.GetComponent<mainCam>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (rb.velocity.y != 0)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
        if (canMove)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector2(transform.position.x, 0), speed * Time.deltaTime);
        }

        if(canMove)
        {
            if(transform.position.y < 0.05 && transform.position.y > -0.05)
            {
                canMove = false;
                canJump = true;
            }
        }

        if (canJump)
        {
            JumpToNextStrairs();
        }

        PlayerControls();
    }

    private void PlayerControls()
    {
        if (!cantControl)
        {
            playerMove = new Vector2(0, Input.GetAxisRaw("Vertical"));
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = playerMove * speed;
    }
    public void MoveToMiddle()
    {
        canMove = true;
    }
    private void JumpToNextStrairs()
    {
        UpdateStairsObj();
        cam.canMoveCam = true;
        
        center = transform.position + (stairs.transform.position - transform.position) / 2f;

        


        center -= new Vector3(0, 2, 0);

        if (centerCanBeMoved == true)
        {
            center += new Vector3(5f, 0, 0);
            centerCanBeMoved = false;
        }
        Vector3 relCenter = this.transform.position - center;
        Vector3 relCenter2 = stairs.transform.position - center;

        transform.position = Vector3.Slerp(relCenter, relCenter2, speed * Time.deltaTime);
        transform.position += center;

        Debug.Log("center: " + center);
        Debug.DrawLine(Vector3.zero, center, Color.green);
    }
    public void UpdateStairsObj()
    {
       stairs = GameObject.FindGameObjectWithTag("stairs");
    }
}
