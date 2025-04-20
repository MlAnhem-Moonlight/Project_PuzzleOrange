using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    private void Start()
    {
        float spawnRate = Mathf.Max(2f, 4f - GameManager.Instance.hard);
        InvokeRepeating("SpawnObstacle", 3f, spawnRate);
    }

    private void SpawnObstacle()
    {
        //GameObject obstacle = Spawner.Instance.SpawnRandomInList(Spawner.Instance.obstacles);
      //  obstacle.transform.position = new Vector3(10, 0, 0);
    }    
}
