using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] 
    private bool bulletSpread = true;

    [SerializeField] 
    private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    
    public ParticleSystem impactParticleSystem;

    public Transform bulletSpawnLocation;
    public float shotDelay = 0.5f;
    private float lastShot;
    public LayerMask mask;

    public void Shoot()
    {
        if (lastShot + shotDelay < Time.time)
        {
            Vector3 direction = GetBulletTrajectory();
            if (Physics.Raycast(bulletSpawnLocation.position, direction, out RaycastHit hit, float.MaxValue, mask))
            {
                OnHit(hit);
                lastShot = Time.time;
            }
        }
    }

    private Vector3 GetBulletTrajectory()
    {
        Vector3 direction = transform.forward;
        
        if (bulletSpread)
        {
            direction += new Vector3(
                Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z));

            direction.Normalize();
        }

        return direction;
    }
    
    private void OnHit(RaycastHit hit)
    {
        Instantiate(impactParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
    }
}