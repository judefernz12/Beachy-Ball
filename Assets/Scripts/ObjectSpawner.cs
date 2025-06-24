using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject player;
    public GameObject[] trianglePrefabs;
    private Vector3 spawnObstaclePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToHorizon = Vector3.Distance(player.gameObject.transform.position,spawnObstaclePosition);
        if(distanceToHorizon < 120)
        {
            SpawnTriangles();
        }
    }

    void SpawnTriangles(){
        spawnObstaclePosition=new Vector3(0, 0, spawnObstaclePosition.z+30);
        Instantiate(trianglePrefabs[(Random.Range(0, trianglePrefabs.Length))], spawnObstaclePosition, Quaternion.identity);
    }
}
