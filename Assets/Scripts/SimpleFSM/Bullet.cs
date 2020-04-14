using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    private GameObject explosionPS;
    [SerializeField]
    private float speed = 600f;
    [SerializeField]
    private float lifeTime = 3f;

    public int damage = 50;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Instantiate(explosionPS, contact.point, Quaternion.identity);
        Destroy(gameObject);
    }
}
