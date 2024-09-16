using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateWeakPoints : MonoBehaviour
{
    public int amountOfWeakSpots;
    public int treeHealt;
    public GameObject weakSpot;
    public GameObject ladderObject;
    public GameObject treeObject;
    private ChangeLevelScript manager;
    public float shakeTime;

    public bool treeDead = false;
    public bool activateChange = false;


    public float shakingSpeed;
    public float shakingAmount;

    public bool canShake = false;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<ChangeLevelScript>();
        SpawnWeakPoints();
        treeHealt = amountOfWeakSpots;
    }
    private void Update()
    {
        if (canShake)
        {
            StartCoroutine(Tremble());
            StartCoroutine(QuickTremble());
        }
    }

    public void SpawnWeakPoints()
    {
        float collHeight = transform.GetComponent<Collider2D>().bounds.size.y;

        for (int i = 0; i < amountOfWeakSpots; i++)
        {
            float rnd = Random.Range(1f, (collHeight - 1));

            Instantiate(weakSpot, new Vector2(transform.position.x, (rnd - 4)), Quaternion.identity);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "WeakPoint")
        {
            GameObject[] objcts = GameObject.FindGameObjectsWithTag("WeakPoint");
            foreach (GameObject obj in objcts)
            {
                Destroy(obj);
            }
            SpawnWeakPoints();
        }
    }
    public void CheckHealth()
    {

        if (treeHealt == 0)
        {
            treeDead = true;
            activateChange = true;
            DestroyCurrentTree();
            manager.SpawnStairs();
        }
    }

    public void DestroyCurrentTree()
    {
        Destroy(this.gameObject);
    }
    IEnumerator Tremble()
    {
      
            transform.localPosition += new Vector3(0.01f, 0, 0);
            yield return new WaitForSeconds(0.001f);
            transform.localPosition -= new Vector3(0.01f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        
    }
    IEnumerator QuickTremble()
    {
        yield return new WaitForSeconds(0.1f);
        canShake = false;

    }
}
