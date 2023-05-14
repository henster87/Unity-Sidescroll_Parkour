using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private float startDelay = 2.0f;
    private float repeatRate = 2.0f;
    private PlayerController playerControllerScript;

    private int randomObstacle;

    private MoveLeft obstacleLeft;

    // Start is called before the first frame update
    void Start() 
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();       
    }
    void OnEnable()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (playerControllerScript.isGameOver == false)
        {
            randomObstacle = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[randomObstacle], spawnPos, obstaclePrefabs[randomObstacle].transform.rotation);
        }
        else if (playerControllerScript.isGameOver)
        {
            CancelInvoke("SpawnObstacle");
        }
    }

}
