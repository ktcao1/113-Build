using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject host;
    public Vector3 shootDirection;
    private bool hitWall;
    private float travelSpeed = 7f;
    private float dmgAmt;

    void Start()
    {
        transform.localScale = (host.GetComponent<DemonEnemy>() != null) ? transform.localScale : transform.localScale * 1.5f;
        dmgAmt = (host.GetComponent<DemonEnemy>() != null) ? host.GetComponent<DemonEnemy>().damage : host.GetComponent<DragonEnemy>().damage;
    }

    void Update()
    {
        if (GameManager.instance.difficulty == "hard" && Time.timeScale == .1f) return;
        transform.position = transform.position + shootDirection * travelSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Obstacle" || collider2D.tag == "brick")
        {
            Destroy(gameObject);
        }
        else if (collider2D.tag == "Player")
        {
            Damage dmg = new Damage
            {
                damageAmount = dmgAmt,
                pushForce = 0f,
                origin = transform.position
            };

            Player player = collider2D.GetComponent<Player>();
            player.TakeDamage(dmg);
            Destroy(gameObject);
        }
    }
}

