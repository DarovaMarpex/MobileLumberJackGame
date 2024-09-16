using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLevelScript : MonoBehaviour
{
    public Transform ledderPos, enemyPos, axeCenter;
    public GameObject ledderGO, enemyGO;
    public Image img1, img2, img3, img4, img5;
    public int levelNum;

    // Start is called before the first frame update
    void Start()
    {
        img1.GetComponent<Image>();
        img2.GetComponent<Image>();
        img3.GetComponent<Image>();
        img4.GetComponent<Image>();
        img5.GetComponent<Image>();
        levelNum = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnEnemy()
    {
        Instantiate(enemyGO, new Vector3(enemyPos.position.x, enemyPos.position.y, transform.position.z), Quaternion.identity);
    }
    public void SpawnStairs()
    {
        levelNum++;
        ChangeLevelUI();
        Instantiate(ledderGO, new Vector3(ledderPos.position.x, ledderPos.position.y, transform.position.z), Quaternion.identity);
        SpawnEnemy();
    }
    public void ChangeLevelUI()
    {
        switch (levelNum)
        {
            case 1:
                img1.fillAmount = 0;
                break;
            case 2:
                img2.fillAmount = 0;
                break;
            case 3:
                img3.fillAmount = 0;
                break;
            case 4:
                img4.fillAmount = 0;
                break;
            case 5:
                img5.fillAmount = 0;
                break;
            case 6:
                levelNum = 0;
                break;
        }
    }


}
