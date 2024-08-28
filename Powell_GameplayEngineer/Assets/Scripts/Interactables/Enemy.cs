using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemySpeed = 5f;
    public float explosionRange = 2f;

    public Transform playerOrentation;
    public GameObject player;
    public GameObject explosionEffect; // Assign a particle system prefab here

    private bool isExploding = false;

    private void Start()
    {
        // Ensure the enemy has a SphereCollider set as a trigger
        SphereCollider triggerCollider = gameObject.AddComponent<SphereCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = explosionRange;
    }

    private void Explode()
    {
        isExploding = true;
        Playerinfo playerHealth = player.GetComponent<Playerinfo>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
        // Instantiate the explosion effect at the enemy's position
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(explosion, ps.main.duration); // Destroy the particle system after it finishes playing
            }
            else
            {
                Destroy(explosion, 2f); // Fallback in case there's no ParticleSystem component
            }
        }

        // Deactivate the enemy
        Destroy(gameObject);
        
    }

    private void FixedUpdate()
    {
        if (player == null || isExploding) return;

        Vector3 direction = playerOrentation.position - transform.position;
        direction.y = 0;
        transform.position += direction.normalized * enemySpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isExploding)
        {
            Explode();
        }
    }
}
