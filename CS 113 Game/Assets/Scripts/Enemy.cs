using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public HealthBarEnemy hp;
    public AudioSource hurtSound;

    // Stats
    public float healthPoints;
    public float maxHealthPoints;
    public float damage;
    public float movespeed;
    public float wanderSpeed;

    public float triggerCooldown;
    public float lastTrigger;
    public Player player;
    public Weapon weapon;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weapon = player.GetComponentInChildren<Weapon>();
        hurtSound = GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("GoblinHurt").GetComponent<AudioSource>();
        maxHealthPoints = 3;
        healthPoints = maxHealthPoints;
        movespeed = 2.5f;
        wanderSpeed = 1.5f;
        damage = 0.5f;
        triggerCooldown = 0.5f;
        lastTrigger = -0.5f;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            if (!pm.isImmune)
            {
                player.TakeDamage(new Damage{damageAmount = damage, pushForce = 0});
            }
        }
    }

    public virtual void TakeDamage(Damage dmg)
    {
        if (Time.time - lastTrigger > triggerCooldown)
        {
            lastTrigger = Time.time;
            
            hurtSound.Play();
            if (weapon.weaponType == "bow") GetComponent<AIMovement>().permaChase = true;
            healthPoints -= dmg.damageAmount;
            if (healthPoints <= 0) Die();
            GetComponent<AIMovement>().PushForce(dmg);
            hp.UpdateHealth(healthPoints / maxHealthPoints);
        }
    }

    public void Die()
    {
        hp.UpdateHealth(1f/1f);
        Destroy(this.gameObject);
    }
}
