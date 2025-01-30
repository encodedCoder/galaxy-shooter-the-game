using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float laserSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "EnemyLaser")
        {
            laserSpeed = laserSpeed * -1;
        }
        //Debug.Log(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        laserMovement();
    }

    void laserMovement()
    {
        transform.Translate(Vector3.up * laserSpeed * Time.deltaTime, Space.World);
        //Debug.Log(transform.position.y);
        if (transform.position.y >= 12)
        {
            if(transform.parent != null)
            {
                // Destroy the parent of laser if parent is LaserTripleShot and not LaserContainer
                if(this.transform.parent.gameObject.tag == "LaserTripleShot")
                {
                    Destroy(this.transform.parent.gameObject);
                }
                // Fix this line of code and on tripleShotLaser Parent should be deleted and not the LaserContainer
            }
            Destroy(this.gameObject);
        }
        if (transform.position.y <= -6 && gameObject.tag == "EnemyLaser")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
        }
    }
}
