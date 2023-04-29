using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{   
    public Vector3 EnemyPosition {get; set;}

    [SerializeField] private float bulletSpeed = 3.0f; 

    private Rigidbody bulletRigidbody;


    void Start()
    {
        bulletRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        DestroyIfOutOfArena();
        bulletRigidbody.AddForce(EnemyPosition * bulletSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void DestroyIfOutOfArena()
    {
        if(transform.position.x < -15 || transform.position.x > 15 || 
            transform.position.z < -15 || transform.position.z > 15)
        {
            Destroy(gameObject);
        }
    }
}
