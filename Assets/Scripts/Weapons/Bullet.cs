using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float despawnTimer = 5.0f;
    public Transform parentTransform;

    void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.velocity = parentTransform.GetComponent<Rigidbody>().velocity + parentTransform.forward * 20;
        gameObject.transform.position = parentTransform.position + parentTransform.forward * parentTransform.localScale.z;

        StartCoroutine(DespawnTimer());
    }

    IEnumerator DespawnTimer()
    {
        while (despawnTimer > 0f)
        {
            //Debug.Log("Despawn timer: " + despawnTimer);
            despawnTimer -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
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
