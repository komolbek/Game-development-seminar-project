using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class MultiPlayer : MonoBehaviour, IPunObservable
{
    public float movementSpeed = 10f;
    
    Rigidbody rigidBody;

    public float fireRate = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    float nextFire;

    public AudioClip playerShootingAudio;
    public GameObject bulletFiringEffect;

    [HideInInspector]
    public int health = 100;
    public Slider healthBar;

    PhotonView photonView;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        Move();

        if (Input.GetKey(KeyCode.Space))
            photonView.RPC("Fire", RpcTarget.AllViaServer);
    }

    void Move() 
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            return;

        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        var rotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0, verticalInput));
        transform.rotation = rotation;

        Vector3 movementDir = transform.forward * Time.deltaTime * movementSpeed;
        rigidBody.MovePosition(rigidBody.position + movementDir);
    }

    [PunRPC]
    void Fire() 
    {
        if (Time.time > nextFire)
        {
            print("Enter this line 1");

            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);

            bullet.GetComponent<MultiPlayerBulletController>()?.InitializeBullet(transform.rotation * Vector3.forward, photonView.Owner);

            AudioManager.Instance.Play3D(playerShootingAudio, transform.position);
            VFXManager.Instance.PlayVFX(bulletFiringEffect, bulletPosition.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            MultiPlayerBulletController bullet = collision.gameObject.GetComponent<MultiPlayerBulletController>();
            
            TakeDamage(bullet);
        }
    }

    private void TakeDamage(MultiPlayerBulletController bullet)
    {
        this.health -= bullet.damage;
        this.healthBar.value = health;

        if (health <= 0)
        {
            bullet.owner.AddScore(1);

            PlayerDied();
        }
    }

    private void PlayerDied()
    {
        health = 100;
        healthBar.value = health;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(health);
        else
            health = (int)stream.ReceiveNext();
        healthBar.value = health;
    }
}
