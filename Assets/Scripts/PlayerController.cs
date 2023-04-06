using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float powerupStrength = 7.0f;
    [SerializeField] private GameObject powerupIndicatior;

    private bool hasPowerUp = false;
    private float forwardInput;
    private Rigidbody playerRigidbody;
    private GameObject focalPoint;

    private Vector3 indicatorOffset;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();    
        focalPoint = GameObject.Find("Focal Point");
        indicatorOffset = new Vector3(0, 0.5f, 0);
    }

    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        playerRigidbody.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicatior.transform.position = transform.position - indicatorOffset;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            powerupIndicatior.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
            Destroy(other.gameObject);
        }
    }
    
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log($"Collided with {other.gameObject.name} powerup {hasPowerUp}");
        }
    }


    private IEnumerator PowerupCountdownRoutine ()
    {
        yield return new WaitForSeconds(4);
        hasPowerUp = false;
        powerupIndicatior.SetActive(false);
    }
}