using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float fireRate = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;

    float nextFire;

    public AudioClip playerShootingAudio;
    public GameObject bulletFiringEffect;

    [HideInInspector]
    public int health = 100;
    public Slider healthBar; 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {

            BulletController bullet = collision.gameObject.GetComponent<BulletController>();            

            TakeDamage(bullet.damage);
        }
    }

    private void TakeDamage(int damage)
    {
        this.health -= damage;
        this.healthBar.value = health;

        if (this.health <= 0)
        {
            EnemyDied();
        }
    }

    private void EnemyDied()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            print("Enter this line 1");
            transform.LookAt(other.transform);
            Fire();
        }
    }

    void Fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);

            bullet.GetComponent<BulletController>().InitializeBullet(transform.rotation * Vector3.forward);

            AudioManager.Instance.Play3D(playerShootingAudio, transform.position);
            VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);
        }
    }
}
