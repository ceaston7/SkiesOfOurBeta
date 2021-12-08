using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float turnSpeed;
    public float moveSpeed;
    Enemy closestEnemy;
    Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 0.4f);

        var enemyArray = FindObjectsOfType<Enemy>();
        closestEnemy = enemyArray[0];
        float closestDist = Vector3.Distance(transform.position, enemyArray[0].transform.position);

        for(int i = 1; i < enemyArray.Length; i++)
        {
            if(Vector3.Distance(transform.position, enemyArray[i].transform.position) < closestDist){
                closestDist = Vector3.Distance(transform.position, enemyArray[i].transform.position);
                closestEnemy = enemyArray[i];
            }
        }

        gameObject.transform.position = parentTransform.position + parentTransform.forward * parentTransform.localScale.z;

        turnSpeed = turnSpeed * Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Vector3.RotateTowards(transform.forward, closestEnemy.transform.position - transform.position, turnSpeed * Time.deltaTime, 0.0f);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
            Destroy(collision.gameObject);
            
        Destroy(gameObject);
    }
}
