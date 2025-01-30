using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f;
    [SerializeField]
    private GameObject laserPrefabHere;
    [SerializeField]
    private GameObject tripleShotLaserPrefabHere;
    [SerializeField]
    private GameObject playerShield;
    [SerializeField]
    private GameObject leftEngineDamage, rightEngineDamage;
    [SerializeField]
    private GameObject explosionPrefabHere;
    [SerializeField]
    private AudioClip explosionAudioClip;
    [SerializeField]
    private AudioClip laserShotAudioClip;
    [SerializeField]
    private AudioSource soundSource;
    private GameObject laserContainer;
    [SerializeField]
    private float fireRate = 0.15f;
    private float oldFireRate;
    private float canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager spawnManager;
    [SerializeField]
    private float powerdownTime = 5f;
    

    private bool isTripleShotActive = false;
    private bool isShieldActive = false;
    [SerializeField]
    private int currScore = 0;
    private UIManager uiManager;
    private GameObject mobileControls;
    


    private void Start()
    {
        oldFireRate = fireRate;
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        laserContainer = GameObject.Find("LaserContainer");
        if(laserContainer == null)
        {
            Debug.Log("NoGameObjectInScene: LaserContainer GameObject is not present in the scene");
        }
        playerShield.SetActive(false);
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(uiManager == null)
        {
            Debug.LogError("uiManager Canvas::UIManager component is null reference");
        }
        leftEngineDamage.SetActive(false);
        rightEngineDamage.SetActive(false);
        
        soundSource = GetComponent<AudioSource>();
        if(soundSource == null)
        {
            Debug.LogError("Laser shot Audio Source null reference");
        }
        else
        {
            soundSource.clip = laserShotAudioClip;
        }
        mobileControls = GameObject.Find("MobileSingleStickControl");

#if UNITY_STANDALONE_WIN
        mobileControls.SetActive(false);
#endif
//#if UNITY_EDITOR
//        mobileControls.SetActive(false);
//#endif

    }


    // Update is called once per frame
    void Update()
    {
        calculateMovement();
        if (Input.GetKey(KeyCode.Space) && Time.time > canFire)
        {
            ShootLaser();
        }
    }

    public void ShootLaser()
    {
        canFire = Time.time + fireRate; 
        GameObject newLaser;

        if (isTripleShotActive)
        {
            newLaser = Instantiate(tripleShotLaserPrefabHere, transform.position + new Vector3(0f, 1f, 0f), transform.rotation);
        }
        else
        {
            newLaser = Instantiate(laserPrefabHere, transform.position + new Vector3(0f, 1f, 0f), transform.rotation);
        }
        newLaser.transform.parent = laserContainer.transform;
        soundSource.clip = laserShotAudioClip;
        soundSource.Play();
    }

    void calculateMovement()
    {
        float horizontalMovement, verticalMovement;
        // transform.Translate(Vector3.right * speed * Time.deltaTime);
#if UNITY_ANDROID
        horizontalMovement = CrossPlatformInputManager.GetAxis("Horizontal");
        verticalMovement = CrossPlatformInputManager.GetAxis("Vertical");
#else
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
#endif

        Vector3 movement = new Vector3(horizontalMovement, verticalMovement, 0f);

        transform.position += movement * Time.deltaTime * moveSpeed;

        // Check for horizontal bounds and wrap around the movement
        if (transform.position.x >= 14.2f)
        {
            transform.position = new Vector3(-14.2f, transform.position.y, 0f);
        }
        else if (transform.position.x <= -14.2f) 
        {
            transform.position = new Vector3(14.2f, transform.position.y, 0f);
        }

        // Check for vertical bounds and restrain the player
        if (transform.position.y >= 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, 0f);
        }
        else if (transform.position.y <= -3.14f)
        {
            transform.position = new Vector3(transform.position.x, -3.14f, 0f);
        }
    }

    public void Damage()
    {
        if (isShieldActive)
        {
            isShieldActive = false;
            playerShield.SetActive(false);
            return;
        }
        _lives--;
        uiManager.UpdateLivesImage(_lives);

        if(_lives == 2)
        {
            leftEngineDamage.SetActive(true);
        }
        else if(_lives == 1)
        {
            rightEngineDamage.SetActive(true);
        }
        else if (_lives < 1)
        {
            spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void UpdateTripleShot()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(powerdownTime);
        isTripleShotActive = false;
    }

    public void UpdateMoveSpeed()
    {
        moveSpeed *= 2f;
        fireRate /= 3f;
        StartCoroutine(MoveSpeedPowerDown());
    }

    IEnumerator MoveSpeedPowerDown()
    {
        yield return new WaitForSeconds(powerdownTime);
        moveSpeed /= 2f;
        fireRate = oldFireRate;
    }

    public void ActivateShield()
    {
        isShieldActive = true;
        playerShield.SetActive(true);
        StartCoroutine(ActivateShieldRoutine());
    }

    IEnumerator ActivateShieldRoutine()
    {
        yield return new WaitForSeconds(powerdownTime);
        playerShield.SetActive(false);
        isShieldActive = false;
    }

    public void AddScore(int points = 10)
    {
        currScore += points;
        uiManager.UpdateScore(currScore);
        //Debug.Log("Score: " + score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyLaser")
        {
            //soundSource.clip = explosionAudioClip;
            //soundSource.Play();
            AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);
            Damage();
            Destroy(other.gameObject);
            //Debug.Break();
        }
    }
}
