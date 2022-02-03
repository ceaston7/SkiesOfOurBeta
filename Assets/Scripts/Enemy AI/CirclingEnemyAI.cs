using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclingEnemyAI : Enemy
{
    public GameObject player;
    public float rotateSpeed; //Degrees per second
    public float moveSpeed;
    public float distance;

    float timeSinceLastShot;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.Find("Player");

        if (distance == 0f){
            distance = (gameObject.transform.position - player.transform.position).magnitude;
        }

        timeSinceLastShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.up, rotateSpeed * Time.deltaTime); //Rotate around player
        transform.LookAt(player.transform);

        var separation = transform.position - player.transform.position;
        separation.y = 0;

        if (Mathf.Ceil(separation.magnitude) < distance)
        {
            //Debug.Log("Moving away");
            transform.position += separation.normalized * moveSpeed * Time.deltaTime;
            if (separation.magnitude > distance)
            {
               // Debug.Log("Setting to distance");
                transform.position = player.transform.position + (separation.normalized * distance);
            }
        }
        else if (Mathf.Floor(separation.magnitude) > distance)
        {
            transform.position -= separation.normalized * moveSpeed * Time.deltaTime;
            //Debug.Log("Moving toward");
        }

        if (timeSinceLastShot > 3.0f)
        {
            timeSinceLastShot = 0.0f;
            Bullet.Create(transform, "Player");
        }
        else
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    void FixedUpdate(){
        
    }
}
