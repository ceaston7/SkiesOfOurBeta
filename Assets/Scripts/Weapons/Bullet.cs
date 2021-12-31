using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float despawnTimer;
    public Transform parentTransform;

    void Start()
    {
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.velocity = parentTransform.forward * 20;
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
        if (collision.gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Kill();
            Destroy(gameObject);
        }
    }
}
