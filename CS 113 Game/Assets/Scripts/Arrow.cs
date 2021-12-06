using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Weapon weapon;
    private Player player;
    public Vector3 shootDirection;
    private bool hitWall;
    private float travelSpeed = 7f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weapon = player.GetComponentInChildren<Weapon>();
        weapon.shootSound.Play();
    }

    void Update()
    {
        transform.position = transform.position + shootDirection * travelSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Obstacle" || collider2D.tag == "brick")
        {
            Destroy(gameObject);
        }
        else if (collider2D.tag == "Enemy")
        {
            Damage dmg = new Damage
            {
                damageAmount = weapon.damageValue,
                pushForce = 0f,
                origin = gameObject.transform.position
            };

            Enemy enemy = collider2D.GetComponent<Enemy>();
            enemy.TakeDamage(dmg);
        }
    }
}
