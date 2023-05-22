using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 30.0f;
    [SerializeField] private float jumpForce = 2000.0f;

    //===Boosters===
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private float powerupStrength = 7.0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float smashMultiplier = 2f;
    [SerializeField] private float smashExplosionForce = 105;
    [SerializeField] private float smashExplosionRadius = 20;
    //===Boosters===
    

    private float forwardInput;
    private Vector3 startPostion;
    private Rigidbody playerRigidbody;
    private GameObject focalPoint;
    private Vector3 powerupIndicatorOffset;
    private bool hasPowerUp = false;
    private bool isJumping = false;
    private bool smashBoosterIsActive = false;
    private Coroutine powerUpCoroutine;
    private WaitForSeconds powerUpDelay = new WaitForSeconds(4.0f);
    private WaitForSeconds jumpDelay = new WaitForSeconds(0.3f);


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();    
        focalPoint = GameObject.Find("Focal Point");
        powerupIndicatorOffset = new Vector3(0, 0.5f, 0);

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
        MovePowerUpIndicator();
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerUpBooster"))
        {   
            ActivatePowerUpBooster();
            Destroy(other.gameObject);
        }

        if(other.CompareTag("ShootBooster"))
        {
            ActivateShootBooster();
            Destroy(other.gameObject);
        }

        if(other.CompareTag("SmashBooster"))
        {
            ActivateSmashBooster();
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
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }


    public bool IsPlayerOutOfArena()
    {
        return transform.position.y < -5.0f;
    }

    public void ResetPlayer()
    {
        playerRigidbody.velocity = Vector3.zero;
        transform.position = startPostion;
        hasPowerUp = false;
        isJumping = false;
        smashBoosterIsActive = false;
        powerupIndicator.SetActive(false);
        if(powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
    }


    private void ReadMovementInputData()
    {
        forwardInput = Input.GetAxis("Vertical");
    }

    private void MoveHorizontal()
    {
        playerRigidbody.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }


    #region SmashBooster
    private void ActivateSmashBooster()
    {
        hasPowerUp = true;
        smashBoosterIsActive = true;
        powerupIndicator.SetActive(true);
        if(powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
        powerUpCoroutine = StartCoroutine(PowerUpCountdownRoutine());
    }

    private bool IsSmashJumpReady()
    {
        return Input.GetKeyDown(KeyCode.Space) && !isJumping && smashBoosterIsActive;
    }

    private IEnumerator SmashJumpRoutine()
    {
        while(true)
        {
            yield return new WaitUntil(() => IsSmashJumpReady());
            isJumping = true;
            playerRigidbody.AddForce(Vector3.up * jumpForce);
            yield return jumpDelay;
            playerRigidbody.AddForce(Vector3.down * jumpForce * smashMultiplier);
            isJumping = false;
            SmashBoosterAttackEmenies();
        }
    }

    private void SmashBoosterAttackEmenies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");   
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Rigidbody>().AddExplosionForce(smashExplosionForce, playerPosition, smashExplosionRadius, 0.0f, ForceMode.Impulse);
        }
    }
    #endregion


    #region ShootBooster
    private void ActivateShootBooster()
    {
        hasPowerUp = true;
        ShootBoosterFire();

        if(powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
        powerUpCoroutine = StartCoroutine(PowerUpCountdownRoutine());
    }
    
    private void ShootBoosterFire()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");   
        GameObject bullet;
        Vector3 attackDirection;

        foreach(GameObject enemy in enemies)
        {
            bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            attackDirection = (enemy.transform.position - bullet.transform.position).normalized;
            bullet.GetComponent<BulletController>().EnemyPosition = attackDirection;
        }
    }
    #endregion


    #region PowerUpBooster
    private void ActivatePowerUpBooster()
    {
        hasPowerUp = true;
        powerupIndicator.SetActive(true);

        if(powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
        powerUpCoroutine = StartCoroutine(PowerUpCountdownRoutine());
    }


    private IEnumerator PowerUpCountdownRoutine()
    {
        yield return powerUpDelay;
        hasPowerUp = false;
        smashBoosterIsActive = false;
        powerupIndicator.SetActive(false);
        
    }
    #endregion


    private void MovePowerUpIndicator()
    {
       powerupIndicator.transform.position = transform.position - powerupIndicatorOffset;
    }
}