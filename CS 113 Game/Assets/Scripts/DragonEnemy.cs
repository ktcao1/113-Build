using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEnemy : Enemy
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Animator dragonAnimator;
    public Transform fbLocations1, fbLocations2;
    public float shootCooldown = 0.5f;
    public float lastShoot = -0.5f;
    public float starCooldown = 1f;
    public float lastStar = -1f;
    public string lastPhase = "";
    public bool inPhase;
    public float phaseTime = 10f;
    public string currentPhase;
    public float inBetweenPhasesDelay = 2.5f;
    public float gettingUpDelay = 2f;
    List<string> phases = new List<string>(new string[]{"spam", "star", "charge"});

    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weapon = player.GetComponentInChildren<Weapon>();
        hurtSound = GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("DragonHurt").GetComponent<AudioSource>();
        fbLocations1 = gameObject.transform.Find("FireballLocations1");
        fbLocations2 = gameObject.transform.Find("FireballLocations2");
        maxHealthPoints = 25;
        healthPoints = maxHealthPoints;
        movespeed = 2f;
        wanderSpeed = 1f;
        damage = 1.5f;
        triggerCooldown = 0.5f;
        lastTrigger = -0.5f;
    }

    void Update()
    {
        if (!GetComponent<AIMovement>().inChase || player.isDead) return;

        if (!inPhase)
        {
            if (lastPhase == "")
            {
                currentPhase = phases[Random.Range(0, phases.Count)];
                lastPhase = currentPhase;
            }
            else
            {
                string temp = lastPhase;
                phases.Remove(lastPhase);
                currentPhase = phases[Random.Range(0, phases.Count)];
                phases.Add(temp);
                lastPhase = currentPhase;
            }
            GetComponent<AIMovement>().inPhase = true;
            inPhase = true;
        }

        // Timing
        if (phaseTime <= 0)
        {
            dragonAnimator.SetBool("tired", true);
            inBetweenPhasesDelay -= Time.deltaTime;
            GetComponent<AIMovement>().movespeed = 0;
            if (inBetweenPhasesDelay <= 0) phaseTime = 10f;  
            return;
        }
        if (inBetweenPhasesDelay <= 0)
        {   
            dragonAnimator.SetBool("tired", false);
            GetComponent<AIMovement>().movespeed = 2f;
            gettingUpDelay -= Time.deltaTime;
            if (gettingUpDelay <= 0) inBetweenPhasesDelay = 5f;
            return;
        }
        if (gettingUpDelay <= 0)
        {
            GetComponent<AIMovement>().inPhase = false;
            inPhase = false;
            gettingUpDelay = 2f;
            return;
        }

        // Phases
        if (currentPhase == "spam")
        {
            GetComponent<AIMovement>().movespeed = 1f;
            SpamShootPhase();
            phaseTime -= Time.deltaTime;
        }
        else if (currentPhase == "star")
        {
            GetComponent<AIMovement>().movespeed = 1f;
            StarShootPhase();
            phaseTime -= Time.deltaTime;
        }
        else if (currentPhase == "charge")
        {
            GetComponent<AIMovement>().movespeed = 3.5f;
            phaseTime -= Time.deltaTime;
        }
    }

    void SpamShootPhase()
    {
        Shoot();
    }

    void StarShootPhase()
    {
        StarShoot();
    }

    void Shoot()
    {
        if (Time.time - lastShoot > shootCooldown)
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

    void StarShoot()
    {
        if (Time.time - lastStar > starCooldown)
        {
            lastStar = Time.time;

            int rand = Random.Range(1, 3);
            if (rand == 1)
            {
                for (int i = 0; i < fbLocations1.childCount; i++)
                {
                    Vector3 player_pos = fbLocations1.GetChild(i).position;
                    player_pos.z = 5.23f;
                    player_pos.x = player_pos.x - transform.position.x;
                    player_pos.y = player_pos.y - transform.position.y;
                    float angle = Mathf.Atan2(player_pos.y, player_pos.x) * Mathf.Rad2Deg;
                    GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

                    Fireball goFireball = fireball.GetComponent<Fireball>();
                    goFireball.host = gameObject;
                    goFireball.shootDirection = fbLocations1.GetChild(i).position - transform.position;
                    goFireball.shootDirection.z = 0;
                    goFireball.shootDirection.Normalize();
                }
            }
            else if (rand == 2)
            {
                for (int i = 0; i < fbLocations2.childCount; i++)
                {
                    Vector3 player_pos = fbLocations2.GetChild(i).position;
                    player_pos.z = 5.23f;
                    player_pos.x = player_pos.x - transform.position.x;
                    player_pos.y = player_pos.y - transform.position.y;
                    float angle = Mathf.Atan2(player_pos.y, player_pos.x) * Mathf.Rad2Deg;
                    GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

                    Fireball goFireball = fireball.GetComponent<Fireball>();
                    goFireball.host = gameObject;
                    goFireball.shootDirection = fbLocations2.GetChild(i).position - transform.position;
                    goFireball.shootDirection.z = 0;
                    goFireball.shootDirection.Normalize();
                }
            }
        }
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
            dmg.pushForce /= 3;
            GetComponent<AIMovement>().PushForce(dmg);
            hp.UpdateHealth(healthPoints / maxHealthPoints);
        }
    }
}
