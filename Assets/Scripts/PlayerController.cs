using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 30.0f;
    [SerializeField] private float jumpForce = 200.0f;
    [SerializeField] private float smashForce = 2f;
    [SerializeField] private float powerupStrength = 7.0f;
    [SerializeField] private GameObject powerupIndicatior;
    [SerializeField] private GameObject bulletPrefab;

    
    private bool hasPowerUp = false;
    private bool isJumping = false;
    private bool spaceBarWasPressed = false;
    private bool smashBoosterIsActive = false;
    private float forwardInput;
    private Vector3 startPostion;
    private Rigidbody playerRigidbody;
    private GameObject focalPoint;

    private Coroutine powerUpCoroutine;
    private WaitForSeconds jumpDelay = new WaitForSeconds(0.3f);
    private WaitForSeconds powerUpDelay = new WaitForSeconds(4.0f);

    private Vector3 indicatorOffset;

    private GameObject[] enemies;
    private Vector3 attackDirection;
    private GameObject bullet;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();    
        focalPoint = GameObject.Find("Focal Point");
        indicatorOffset = new Vector3(0, 0.5f, 0);

        startPostion = transform.position;
        StartCoroutine(SmashJumpRoutine());
    }

    void FixedUpdate()
    {
        MoveHorizontal();
        IsSmashJumpReady();
    }

    void Update()
    {
        ReadMovementInputData();
        MoveIfOutOfArena();
        MovePowerUpIndicator();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerUpBooster"))
        {   
            Destroy(other.gameObject);
            ActivatePowerUpBooster();
        }

        if(other.CompareTag("ShootBooster"))
        {
            Destroy(other.gameObject);
            ActivateShootBooster();
        }

        if(other.CompareTag("SmashBooster"))
        {
            Destroy(other.gameObject);
            ActivateSmashBooster();
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

    void OnDestroy()
    {
        StopAllCoroutines();
    }


    private void ReadMovementInputData()
    {
        forwardInput = Input.GetAxis("Vertical");
        spaceBarWasPressed = Input.GetKeyDown(KeyCode.Space);
    }

    private void MoveHorizontal()
    {
        playerRigidbody.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    private void ActivatePowerUpBooster()
    {
        hasPowerUp = true;
        powerupIndicatior.SetActive(true);

        if(powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
        powerUpCoroutine = StartCoroutine(PowerUpCountdownRoutine());
    }

    private void ActivateShootBooster()
    {
        hasPowerUp = true;
        Fire();
        if(powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
        powerUpCoroutine = StartCoroutine(PowerUpCountdownRoutine());
    }
    
    private void ActivateSmashBooster()
    {
        hasPowerUp = true;
        smashBoosterIsActive = true;
        powerupIndicatior.SetActive(true);
        if(powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
        powerUpCoroutine = StartCoroutine(PowerUpCountdownRoutine());
    }

    private bool IsSmashJumpReady()
    {
        return spaceBarWasPressed && !isJumping && smashBoosterIsActive;
    }

    private IEnumerator SmashJumpRoutine()
    {
        while(true)
        {
            yield return new WaitUntil(() => IsSmashJumpReady());
            isJumping = true;
            playerRigidbody.AddForce(Vector3.up * jumpForce);
            yield return jumpDelay;
            playerRigidbody.AddForce(Vector3.down * jumpForce * smashForce);
            isJumping = false;
        }
    }

    private IEnumerator PowerUpCountdownRoutine()
    {
        yield return powerUpDelay;
        hasPowerUp = false;
        smashBoosterIsActive = false;
        powerupIndicatior.SetActive(false);
    }

    private void Fire()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");   
        foreach(GameObject enemy in enemies)
        {
            bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            attackDirection = (enemy.transform.position - bullet.transform.position).normalized;
            bullet.GetComponent<BulletController>().EnemyPosition = attackDirection;
        }
    }

    private void MoveIfOutOfArena()
    {
        if(transform.position.y < -5.0f)
        {
            playerRigidbody.velocity = Vector3.zero;
            transform.position = startPostion;
        }
    }

    void MovePowerUpIndicator()
    {
        powerupIndicatior.transform.position = transform.position - indicatorOffset;
    }
}