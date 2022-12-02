using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerBulletController : MonoBehaviour
{

    Rigidbody rigidBody;

    public float bulletSpeed = 15f;
    public AudioClip BulletHitAudio;
    public float bulletLifeTime = 10f;
    public GameObject bulletImpactEffect;
    public int damage = 10;

    [HideInInspector]
    public Photon.Realtime.Player owner;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void InitializeBullet(Vector3 originalDirection, Photon.Realtime.Player givenPlayer)
    {
        transform.forward = originalDirection;
        rigidBody.velocity = transform.forward * bulletSpeed;

        owner = givenPlayer;

        Destroy(gameObject, bulletLifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.Play3D(BulletHitAudio, transform.position);

        VFXManager.Instance.PlayVFX(bulletImpactEffect, transform.position);

        Destroy(gameObject);
    }

}
