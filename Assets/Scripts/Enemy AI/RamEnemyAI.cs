using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamEnemyAI : Enemy
{
    public GameObject player;
    public float turnSpeed;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.Find("Player");

        turnSpeed = turnSpeed * Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Vector3.RotateTowards(transform.forward, player.transform.position - transform.position, turnSpeed * Time.deltaTime, 0.0f);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
