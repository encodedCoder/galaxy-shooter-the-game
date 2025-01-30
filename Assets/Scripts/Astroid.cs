using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float rotationAngle = 0.2f;
    [SerializeField]
    private GameObject explosionPrefabHere;
    private SpawnManager spawnManager;
    [SerializeField]
    private AudioClip explosionAudioClip;
    

    // Start is called before the first frame update
    void Start()
    {
        rotationAngle = Random.Range(0.2f, -0.2f);
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, rotationAngle, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            AudioSource.PlayClipAtPoint(explosionAudioClip, transform.position);
            GameObject explosion = Instantiate(explosionPrefabHere, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            spawnManager.StartSpawning();
            Destroy(explosion, 3f);
        }
    }
}
