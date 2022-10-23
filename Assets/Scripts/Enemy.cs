using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameManager gameManager;
    public GameObject explosion;

    public bool isDead = false;
    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            isDead = true;
            gameObject.SetActive(false);
            Instantiate(explosion, transform.position, new Quaternion(0, 0, 0, 0));
            Debug.Log("Destroy enemy");
        }
    }
}
