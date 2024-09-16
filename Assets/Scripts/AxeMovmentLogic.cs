using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeMovmentLogic : MonoBehaviour
{
    [SerializeField] private float startingSpeed = 10;
    [SerializeField] private float speedIncrease = 0.25f;

    private Rigidbody2D rb;
    private int hitCounter;

    private Vector2 ballOpositeDir;

    public Animator animator;

    public GenerateWeakPoints currentEnemy;

    public PlayerMovingScript player;

    private ChangeLevelScript manager;

    public float checkingNumber;

    public bool canPickUp = false;

    public bool cantloose = false;

    public ParticleSystem fallingLeaves;

    public ParticleSystem piecesOfTree;

    public Transform leavesSpawn;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<ChangeLevelScript>();
        player = FindObjectOfType<PlayerMovingScript>();
        findCurrentEnemy();
        rb = GetComponent<Rigidbody2D>();
        Invoke("ResetBall", 0.1f);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, startingSpeed + (speedIncrease * hitCounter));

        if (currentEnemy.treeDead)
        {
            getAxeBack();
        }
    }

    private void Startball()
    {
        rb.velocity = new Vector2(-1, 0) * (startingSpeed + (speedIncrease * hitCounter));
    }

    private void ResetBall()
    {
        rb.velocity = new Vector3(manager.axeCenter.position.x, manager.axeCenter.position.y, transform.position.z);
        transform.position = new Vector2(0, 0);
        hitCounter = 0;
        Invoke("Startball", 2f);
    }
    public void PlayerBounce(Transform obj)
    {
        hitCounter++;


        Vector2 ballPos = transform.position;
        Vector3 playerPos = obj.position;

        float xDir, yDir;
        Debug.Log("bounceFromPlayer");
        if (transform.position.x > manager.axeCenter.position.x)
        {
            xDir = -1;
        }
        else
        {
            xDir = 1;
        }

        yDir = (ballPos.y - playerPos.y) / obj.GetComponent<Collider2D>().bounds.size.y;

        ballOpositeDir.y = yDir;

        if (yDir == 0)
        {
            yDir = 0.25f;
        }

        rb.velocity = new Vector2(xDir, yDir) * (startingSpeed + (speedIncrease * hitCounter));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "PlayerObject")
        {
            if (canPickUp)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
                this.gameObject.SetActive(false);
                player.MoveToMiddle();
                canPickUp = false;
                currentEnemy.activateChange = false;
            }
            else if (!canPickUp)
            {
                PlayerBounce(collision.transform);
                animator.SetTrigger("Hit");
            }
        }
        else if (collision.gameObject.tag == "EnemyObject")
        {
            BounceFromWall();

        }
        else if (collision.gameObject.name == "GameLoose")
        {
            if (!cantloose)
            {
                LostGame();
            }
        }
        else if (collision.gameObject.name == "Boarder1" || collision.gameObject.name == "Boarder2")
        {
            if(ballOpositeDir.y > 0)
            {
                ballOpositeDir.y = -Mathf.Abs(ballOpositeDir.y);
            }
            else
            {
                ballOpositeDir.y = Mathf.Abs(ballOpositeDir.y);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "WeakPoint")
        {
            GenerateWeakPoints currentTree = FindObjectOfType<GenerateWeakPoints>();
            currentTree.canShake = true;

            Vector2 axeLastPos = transform.position;
            Destroy(coll.gameObject);
            currentEnemy.treeHealt--;
            currentEnemy.CheckHealth();
            if (currentEnemy.activateChange)
            {
                rb.velocity = new Vector2(0, 0);
            }
            else
            {

                ParticleSystem pref1 = Instantiate(fallingLeaves, new Vector2(leavesSpawn.position.x, leavesSpawn.position.y), Quaternion.Euler(90, -180, 0));
                ParticleSystem pref2 = Instantiate(piecesOfTree, new Vector2(axeLastPos.x + 0.5f, axeLastPos.y), Quaternion.identity);
                Destroy(pref1, 7);
                Destroy(pref2, 2);
            }

        }
    }

    public void BounceFromWall()
    {
        Vector2 hitPoint = transform.position;
        Debug.Log("bounceFromEnemy");
        float xDir, yDir;
        if (transform.position.x > manager.axeCenter.position.x)
        {
            xDir = -1;
        }
        else
        {
            xDir = 1;
        }

        float rnd = Random.Range(1.1f, 1.4f);
        yDir = ballOpositeDir.y * rnd;

        if (yDir == 0)
        {
            yDir = 0.25f;
        }

        rb.velocity = new Vector2(xDir, yDir) * (startingSpeed + (speedIncrease * hitCounter));
    }
    public void LostGame()
    {
        Invoke("ResetBall", 0.1f);
    }
    public void findCurrentEnemy()
    {
        currentEnemy = FindObjectOfType<GenerateWeakPoints>();
    }

    public void getAxeBack()
    {
        //player.cantControl = true;
        canPickUp = true;
        cantloose = true;
        transform.position = Vector2.MoveTowards(transform.position, (player.transform.position - transform.position) * 7, 10f * Time.deltaTime);

    }
    public void StartBallAfterJump()
    {
        this.gameObject.SetActive(true);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = new Vector2(0, 0);
        transform.position = new Vector2(player.transform.position.x + 2, 0);
        hitCounter = 0;
        StartballOpposite();
    }
    private void StartballOpposite()
    {
        rb.velocity = new Vector2(1, 0) * (startingSpeed + (speedIncrease * hitCounter));
    }

}
