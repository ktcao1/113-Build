using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonEnemy : Enemy
{
    [SerializeField] private GameObject fireballPrefab;
    private float shootCooldown = 3f;
    private float lastShoot = -3f;

    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weapon = player.GetComponentInChildren<Weapon>();
        hurtSound = GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("DemonHurt").GetComponent<AudioSource>();
        maxHealthPoints = 4;
        healthPoints = maxHealthPoints;
        movespeed = 2f;
        wanderSpeed = 1.5f;
        damage = 1;
        triggerCooldown = 0.5f;
        lastTrigger = -0.5f;
    }

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Time.time - lastShoot > shootCooldown && GetComponent<AIMovement>().inChase)
        {
            lastShoot = Time.time;

            Vector3 player_pos = player.transform.position;
            player_pos.z = 5.23f;
            player_pos.x = player_pos.x - transform.position.x;
            player_pos.y = player_pos.y - transform.position.y;
            float angle = Mathf.Atan2(player_pos.y, player_pos.x) * Mathf.Rad2Deg;
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

            Fireball goFireball = fireball.GetComponent<Fireball>();
            goFireball.host = gameObject;
            goFireball.shootDirection = player.transform.position - transform.position;
            goFireball.shootDirection.z = 0;
            goFireball.shootDirection.Normalize();
        }
    }
}
