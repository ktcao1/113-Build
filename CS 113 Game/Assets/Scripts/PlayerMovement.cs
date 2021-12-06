using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 movement;
    private Player player;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Vector3 lastMoveDir;

    // I-Frame dash
    [SerializeField] private LayerMask dashLayerMask;
    [SerializeField] private Image dashCDBar;
    private bool isDashButtonDown = false;
    public float lastDash = -1.5f;
    public float dashCooldown = 1.5f;
    private float immuneTime = 0.5f;
    private float phaseTime = 0.2f;
    public bool isImmune = false;
    private float dashAmount = 1.5f;
    public AudioSource dashSound;

    // Animator controllers
    [SerializeField] private Animator upAnim, downAnim, leftAnim, rightAnim, dashAnim;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite dashSprite, greenDashSprite;
    [SerializeField] private Animator walkAnim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        bc = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        dashSound = GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("Dash").GetComponent<AudioSource>();;
    }

    private void Update()
    {
        if (GameManager.instance.isLoading || GameManager.instance.disableInputs || player.isDead) return;
        
        upAnim.SetBool("press", Input.GetKey(player.upKey) ? true : false);
        downAnim.SetBool("press", Input.GetKey(player.downKey) ? true : false);
        leftAnim.SetBool("press", Input.GetKey(player.leftKey) ? true : false);
        rightAnim.SetBool("press", Input.GetKey(player.rightKey) ? true : false);

        icon.sprite = dashAnim.GetBool("press") ? greenDashSprite : dashSprite;

        movement.x = (Input.GetKey(player.rightKey) ? 1 : 0) - (Input.GetKey(player.leftKey) ? 1 : 0);
        movement.y = (Input.GetKey(player.upKey) ? 1 : 0) - (Input.GetKey(player.downKey) ? 1 : 0);

        if (movement != Vector2.zero) walkAnim.SetBool("moving", true);
        else walkAnim.SetBool("moving", false);

        // Immunity to enemies while dashing
        if (isImmune && immuneTime > 0) {
            immuneTime -= Time.deltaTime;
        }
        else {
            isImmune = false;
            immuneTime = 0.5f;
            dashAnim.SetBool("press", false);
        }
        if (bc.isTrigger && phaseTime > 0) {
            phaseTime -= Time.deltaTime;
        }
        else {
            bc.isTrigger = false;
            phaseTime = 0.2f;
        }

        if (Input.GetKeyDown(player.dashKey))
            isDashButtonDown = true;
    }

    private void FixedUpdate()
    {
        if (player.isDead) return;
        if (movement.x > 0) player.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (movement.x < 0) player.transform.rotation = Quaternion.Euler(0, 180, 0);
        if (movement != Vector2.zero)
            lastMoveDir = movement;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

        if (Time.time - lastDash <= dashCooldown) dashCDBar.fillAmount = Mathf.Min(1, (Time.time - lastDash) / dashCooldown);
        else dashCDBar.fillAmount = 1;

        Vector3 dashPosition = transform.position + lastMoveDir * dashAmount;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, lastMoveDir, dashAmount, dashLayerMask);

        if (isDashButtonDown && Time.time - lastDash > dashCooldown)
        {
            dashSound.Play();
            dashAnim.SetBool("press", true);
            walkAnim.SetTrigger("dashing");
            lastDash = Time.time;
            isImmune = true;

            if (raycastHit2D.collider != null) 
            {
                rb.MovePosition(raycastHit2D.point);
            }
            else
            {
                bc.isTrigger = true;
                rb.MovePosition(dashPosition);
            }
        }
        isDashButtonDown = false;
    }
}
