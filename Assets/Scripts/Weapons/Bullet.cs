using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float despawnTimer;
    public Transform parentTransform;

    string targetTag;

    // Static Properties
    int bulletSpeed = 20;
    float bulletSize = 0.2f;

    public static Bullet Create(Transform parentTransform, string targetTag)
    {
        var a = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<Bullet>();
        a.GetComponent<Bullet>().parentTransform = parentTransform;
        a.targetTag = targetTag;

        return a;
    }

    void Start()
    {
        transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.velocity = parentTransform.forward * bulletSpeed;
        gameObject.transform.position = parentTransform.position + parentTransform.forward * parentTransform.localScale.z;

        despawnTimer = 0;
        StartCoroutine(DespawnTimer());
    }

    IEnumerator DespawnTimer()
    {
        while (despawnTimer < 5.0f)
        {
            //Debug.Log("Despawn timer: " + despawnTimer);
            despawnTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            if (collision.gameObject.CompareTag("enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().Kill();
                Destroy(gameObject);
            } 
            else if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerControl>().Kill();
            }
        }
    }
}
