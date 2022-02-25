using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] float turnSpeed = 30.0f;
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float lifespan = 10.0f;
    public Enemy closestEnemy;
    public Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 0.4f);
        gameObject.transform.position = parentTransform.position + parentTransform.forward * parentTransform.localScale.z;
        gameObject.transform.rotation = Quaternion.LookRotation(parentTransform.forward, Vector3.up);

        turnSpeed = turnSpeed * Mathf.Deg2Rad;

        Enemy[] enemyArray = FindObjectsOfType<Enemy>();
        if (enemyArray.Length != 0)
        {
            closestEnemy = enemyArray[0];
            float closestDist = Vector3.Distance(transform.position, enemyArray[0].transform.position);

            for (int i = 1; i < enemyArray.Length; i++)
            {
                if (Vector3.Distance(transform.position, enemyArray[i].transform.position) < closestDist)
                {
                    closestDist = Vector3.Distance(transform.position, enemyArray[i].transform.position);
                    closestEnemy = enemyArray[i];
                }
            }

            //Add missile to list on locked enemy so that homing can be deactivated on enemy destruction
            closestEnemy.MissileLocked(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lifespan > 0f)
        {
            if (closestEnemy != null)
            {
                transform.forward = Vector3.RotateTowards(transform.forward, closestEnemy.transform.position - transform.position, turnSpeed * Time.deltaTime, 0.0f);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
        else{
            Destroy(gameObject);
        }

        lifespan -= Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy")) 
        {
            collision.gameObject.GetComponent<Enemy>().Kill();
            Destroy(gameObject); 
        }
    }
}
