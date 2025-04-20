using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float destroyDistance = 20f;

    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
        this.moveSpeed += GameManager.Instance.hard;
    }

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        if (transform.position.x < mainCamera.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
