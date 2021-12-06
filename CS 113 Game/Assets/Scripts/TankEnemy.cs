using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weapon = player.GetComponentInChildren<Weapon>();
        hurtSound = GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("TankHurt").GetComponent<AudioSource>();
        maxHealthPoints = 8;
        healthPoints = maxHealthPoints;
        movespeed = 1.5f;
        wanderSpeed = 1f;
        damage = 1.5f;
        triggerCooldown = 0.5f;
        lastTrigger = -0.5f;
    }

    public override void TakeDamage(Damage dmg)
    {
        if (Time.time - lastTrigger > triggerCooldown)
        {
            lastTrigger = Time.time;
            
            hurtSound.Play();
            if (weapon.weaponType == "bow") GetComponent<AIMovement>().permaChase = true;
            healthPoints -= dmg.damageAmount;
            if (healthPoints <= 0) Die();
            dmg.pushForce /= 2;
            GetComponent<AIMovement>().PushForce(dmg);
            hp.UpdateHealth(healthPoints / maxHealthPoints);
        }
    }
}
