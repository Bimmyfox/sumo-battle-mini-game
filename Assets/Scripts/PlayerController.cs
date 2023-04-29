using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 30.0f;
    [SerializeField] private float powerupStrength = 7.0f;
    [SerializeField] private GameObject powerupIndicatior;
    [SerializeField] private GameObject bulletPrefab;

    private bool hasPowerUp = false;
    private float forwardInput;
    private Rigidbody playerRigidbody;
    private GameObject focalPoint;

    private Vector3 indicatorOffset;
    private GameObject[] enemies;
    private Vector3 attackDirection;
    private GameObject bullet;

    // private Vector3 startPostion;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();    
        focalPoint = GameObject.Find("Focal Point");
        indicatorOffset = new Vector3(0, 0.5f, 0);

        // startPostion = transform.position;
    }

    void Update()
    {
        DestroyIfOutOfBorders();
        MovePowerUpIndicator();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerUpBooster"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            powerupIndicatior.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }

        if(other.CompareTag("ShootBooster"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            Fire();
        }
    }
    
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    void DestroyIfOutOfBorders()
    {
        if(transform.position.y < -5.0f)
        {
            Destroy(gameObject);
            // transform.position = startPostion;
        }
    }

    void MovePowerUpIndicator()
    {
        powerupIndicatior.transform.position = transform.position - indicatorOffset;
    }
    
    void MovePlayer()
    {
        forwardInput = Input.GetAxis("Vertical");
        playerRigidbody.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    void Fire()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");   
        foreach(GameObject enemy in enemies)
        {
            bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            attackDirection = (enemy.transform.position - bullet.transform.position).normalized;
            bullet.GetComponent<BulletController>().EnemyPosition = attackDirection;
        }
    }

    private IEnumerator PowerupCountdownRoutine ()
    {
        yield return new WaitForSeconds(4);
        hasPowerUp = false;
        powerupIndicatior.SetActive(false);
    }
}