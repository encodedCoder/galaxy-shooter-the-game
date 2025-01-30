using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private int powerupId;
    [SerializeField]
    private AudioClip powerupAudioClip;

    private void Start()
    {
        moveSpeed = Random.Range(2f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        if(transform.position.y <= -5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(powerupAudioClip, transform.position);
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupId)
                {
                    case 0: 
                        player.UpdateTripleShot(); 
                        break;
                    case 1: 
                        player.UpdateMoveSpeed(); 
                        break;
                    case 2: 
                        player.ActivateShield();
                        break;
                    default:
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
