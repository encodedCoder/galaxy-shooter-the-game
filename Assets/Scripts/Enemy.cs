using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    private Player _player;
    private Animator animator;
    private Collider2D collider2d;
    [SerializeField]
    private AudioClip explosionAudioClip;
    [SerializeField]
    private AudioSource explosionAudioSource;
    [SerializeField]
    private GameObject laserPrefabHere;
    [SerializeField]
    private GameObject laserContainer;
    private float canFire = 3f;
    private float fireRate = -1f;
    [SerializeField]
    private bool hasLaser = false;

    private void Start()
    {
        moveSpeed = Random.Range(3f, 5f);
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is null in Player GameObject Accessed in Enemy Script");
        }
        
        animator = gameObject.GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("The animtor is null in Enemy GameObject");
        }

        collider2d = gameObject.GetComponent<Collider2D>();
        if(collider2d == null)
        {
            Debug.LogError("The collider2d is null in Enemy GameObject");
        }

        explosionAudioSource = GetComponent<AudioSource>();
        if(explosionAudioClip == null)
        {
            Debug.LogError("ExplosionAudioClip is null reference in Enemy gameObejct");
        }
        else
        {
            explosionAudioSource.clip = explosionAudioClip;
        }
        laserContainer = GameObject.Find("LaserContainer");
        if (laserContainer == null)
        {
            Debug.Log("NoGameObjectInScene: LaserContainer GameObject is not present in the scene");
        }
        StartCoroutine(ShootLaserRoutine());
        hasLaser = Random.Range(1f,10f) > 5 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (hasLaser && Time.time > canFire)
        {
            ShootLaser();
        }
    }

    void CalculateMovement()
    {
        transform.position += new Vector3(0f, -1 * moveSpeed, 0f) * Time.deltaTime;
        if (transform.position.y <= -5f)
        {
            float randomX = Random.Range(-11f, 11f);
            transform.position = new Vector3(randomX, 15f, transform.position.z);
        }
    }

    void ShootLaser()
    {
        fireRate = Random.Range(4f, 7f);
        canFire = Time.time + fireRate;
        GameObject newLaser;
        newLaser = Instantiate(laserPrefabHere, transform.position + new Vector3(0f, -1f, 0f), Quaternion.identity);
        //Debug.Break();
        newLaser.transform.parent = laserContainer.transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hit: " + other.transform.tag);
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            DestroyEnemy();
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore();
            }
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        moveSpeed = 0.8f;
        animator.SetTrigger("OnEnemyDeath");
        collider2d.enabled = false;
        Destroy(this.gameObject, 2.5f);
        explosionAudioSource.Play();
    }

    IEnumerator ShootLaserRoutine()
    {
        GameObject enemyLaser = Instantiate(laserPrefabHere, transform.position, Quaternion.identity);
        enemyLaser.transform.parent = laserContainer.transform;
        yield return new WaitForSeconds(Random.Range(2f, 3f));
    }
}
