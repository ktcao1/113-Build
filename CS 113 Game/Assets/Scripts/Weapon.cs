using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] public BoxCollider2D weaponCollider;
    [SerializeField] private Animator attackAnim;
    [SerializeField] private GameObject arrowPrefab;
    public AudioSource swingSound, shootSound;
    public Animator animator;
    public Image attackIcon;
    public Sprite attackSprite, greenAttackSprite;
    public Sprite bowSprite, greenBowSprite;

    // Damage
    public int damageValue;
    public float pushForce;

    // Cooldown
    private float cooldown = 0.5f;
    private float lastAttack = -0.5f;

    // Upgrade
    public int weaponLevel;
    public bool canAim = false;
    public bool upgraded = false;

    // Current weapon (bow or dagger)
    public string weaponType = "dagger";

    void Start()
    {
        damageValue = 1;
        pushForce = 0f;
        weaponLevel = 0;
        swingSound = GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("SwordSwing").GetComponent<AudioSource>();
        shootSound = GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("BowShoot").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.time - lastAttack <= cooldown)
        {
            attackAnim.SetBool("press", true);
            if (weaponType == "dagger") attackIcon.sprite = greenAttackSprite;
            else if (weaponType == "bow") attackIcon.sprite = greenBowSprite;
        }
        else
        {
            attackAnim.SetBool("press", false);
            if (weaponType == "dagger") attackIcon.sprite = attackSprite;
            else if (weaponType == "bow") attackIcon.sprite = bowSprite;
        }
    }

    public void Attack()
    {
        if (Time.time - lastAttack > cooldown)
        {
            lastAttack = Time.time;
            if (weaponType == "dagger") 
            {
                swingSound.Play();
                animator.SetTrigger("Attack");
            }
            else if (weaponType == "bow") 
            {
                animator.SetTrigger("Shoot");
            }
        }
    }

    public void CompleteShoot()
    {
        if (canAim) 
        {
            Vector3 mouse_pos = Input.mousePosition;
            mouse_pos.z = 5.23f;
            Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;

            GameObject go = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));
            Arrow goArrow = go.GetComponent<Arrow>();
            goArrow.shootDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            goArrow.shootDirection.z = 0;
            goArrow.shootDirection.Normalize();
        }
        else
        {
            Vector3 shootDir = new Vector3(-1, 0, 0);
            if (player.transform.rotation.y == 0) shootDir = new Vector3(1, 0, 0);
            GameObject go = Instantiate(arrowPrefab, transform.position, transform.rotation);
            go.GetComponent<Arrow>().shootDirection = shootDir;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag != "Enemy" || weaponType == "bow") return;

        Damage dmg = new Damage
        {
            damageAmount = damageValue + (weaponLevel * 0.25f),
            pushForce = pushForce,
            origin = player.transform.position
        };

        Enemy enemy = collider2D.GetComponent<Enemy>();
        enemy.TakeDamage(dmg);
    }

    public void EndAnimation()
    {
        animator.enabled = false;
        weaponCollider.enabled = false;
    }
}
