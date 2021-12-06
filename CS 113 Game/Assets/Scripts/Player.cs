using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    // Inputs
    public KeyCode upKey, downKey, leftKey, rightKey;
    public KeyCode attackKey, interactKey, dashKey;
    public KeyCode menuKey = KeyCode.Escape;
    public TMP_Text upText, downText, leftText, rightText;
    public TMP_Text attackText, interactText, dashText;

    // Animations
    [SerializeField] private Animator mainAnim;
    [SerializeField] private Animator weaponAnim;
    [SerializeField] private Animator interactAnim;
    [SerializeField] private Image interactIcon;
    [SerializeField] private Sprite interactSprite, greenInteractSprite;

    // Sound
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource hurtSound;

    // Panels
    [SerializeField] private GameObject rebindPanel;
    [SerializeField] public GameObject bossWarningPanel;

    // Stats and Combat
    public bool isDead = false;
    private int healthPoints;
    private int maxHealthPoints;
    private float damageCoolDown = 1f;
    private float damageLastTaken = -1f;
    public bool hasKnockBack = false;
    private bool flashRed = false;
    private float flashRedCooldown = 0.5f;
    [SerializeField] private GameObject barrierSprite;

    public float currentTimeScale = 1;

    // Room
    public GameObject currentRoom;

    private void Start()
    {
        // TODO: Change these when rebinding and saving/loading is implemented
        upKey = KeyCode.W;
        downKey = KeyCode.S;
        leftKey = KeyCode.A;
        rightKey = KeyCode.D;

        attackKey = KeyCode.F;
        interactKey = KeyCode.E;
        dashKey = KeyCode.Space;

        // Initialize stats
        maxHealthPoints = 5;
        healthPoints = maxHealthPoints;
        ZeldaHealthBar.instance.SetupHearts(maxHealthPoints);
        ZeldaBarrierBar.instance.SetupBarriers(10);
        ZeldaBarrierBar.instance.SetCurrentHealth(0);

        // Initialize room
        currentRoom = GameObject.FindGameObjectWithTag("StartRoom");
    }

    private void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(menuKey))
        {
            GameManager.instance.PauseGame();
        }

        if (ZeldaBarrierBar.instance.currentBarriers > 0) barrierSprite.SetActive(true); else barrierSprite.SetActive(false);

        if (GameManager.instance.disableInputs || GameManager.instance.isLoading || GameManager.instance.gamePaused) return;

        upText.text = upKey.ToString().ToLower();
        downText.text = downKey.ToString().ToLower();
        leftText.text = leftKey.ToString().ToLower();
        rightText.text = rightKey.ToString().ToLower();

        attackText.text = (attackKey.ToString().ToLower() != "space") ? attackKey.ToString().ToLower() : "spa";
        interactText.text = (interactKey.ToString().ToLower() != "space") ? interactKey.ToString().ToLower() : "spa";
        dashText.text = (dashKey.ToString().ToLower() != "space") ? dashKey.ToString().ToLower() : "spa";

        interactAnim.SetBool("press", Input.GetKey(interactKey) ? true : false);
        interactIcon.sprite = interactAnim.GetBool("press") ? greenInteractSprite : interactSprite;

        // Flash Red
        if (Time.time - damageLastTaken <= flashRedCooldown && flashRed)
        {
            transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            if (GameManager.instance.difficulty == "hard" || GameManager.instance.difficulty == "normal")
            {
                Time.timeScale = 0.1f;
                currentTimeScale = 0.1f;
                rebindPanel.SetActive(true);
            }
        }
        else
        {
            flashRed = false;
            if (GameManager.instance.difficulty == "hard" || GameManager.instance.difficulty == "normal")
            {
                Time.timeScale = 1f;
                currentTimeScale = 1f;
                rebindPanel.SetActive(false);
            }
            transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        
        if (Input.GetKeyDown(attackKey))
        {
            weapon.Attack();
        }
    }

    // Combat functions
    // TODO: Add functionality for rebinding everytime player takes damage
    public void TakeDamage(Damage dmg)
    {
        if (Time.time - damageLastTaken > damageCoolDown)
        {
            damageLastTaken = Time.time;

            if (ZeldaBarrierBar.instance.currentBarriers >= dmg.damageAmount) ZeldaBarrierBar.instance.RemoveBarriers(dmg.damageAmount);
            else if (ZeldaBarrierBar.instance.currentBarriers < dmg.damageAmount) 
            {
                hurtSound.Play();
                float difference = dmg.damageAmount - ZeldaBarrierBar.instance.currentBarriers;
                ZeldaBarrierBar.instance.RemoveBarriers(ZeldaBarrierBar.instance.currentBarriers);
                ZeldaHealthBar.instance.RemoveHearts(difference);
                flashRed = true;
                if (ZeldaHealthBar.instance.currentHearts <= 0) Die();
                if (GameManager.instance.difficulty == "hard" || GameManager.instance.difficulty == "normal") RebindKey();
            }
            else
            {
                hurtSound.Play();
                ZeldaHealthBar.instance.RemoveHearts(dmg.damageAmount);
                flashRed = true;
                if (ZeldaHealthBar.instance.currentHearts <= 0) Die();
                if (GameManager.instance.difficulty == "hard" || GameManager.instance.difficulty == "normal") RebindKey();
            }
        }
    }

    public void RebindKey()
    {
        List<string> directionalKeysU_D = new List<string>(new string[]{"upKey", "downKey"});
        List<string> directionalKeysL_R = new List<string>(new string[]{"leftKey", "rightKey"});
        List<string> interactionKeys = new List<string>(new string[]{"attackKey", "interactKey", "dashKey"});

        int rebindSelection = 0;
        if (GameManager.instance.difficulty == "hard") rebindSelection = Random.Range(0, 6);
        if (rebindSelection == 1)
        {
            KeyCode temp = upKey;
            upKey = downKey;
            downKey = temp;
            rebindPanel.GetComponentInChildren<TMP_Text>().text = $"[{upKey}] up ↔ down [{downKey}]";
        }
        else if (rebindSelection == 2)
        {
            KeyCode temp = leftKey;
            leftKey = rightKey;
            rightKey = temp;
            rebindPanel.GetComponentInChildren<TMP_Text>().text = $"[{leftKey}] left ↔ right [{rightKey}]";
        }
        else
        {
            string randomKey1 = interactionKeys[Random.Range(0, 3)];
            interactionKeys.Remove(randomKey1);
            string randomKey2 = interactionKeys[Random.Range(0, 2)];
            if (randomKey1 == "attackKey" && randomKey2 == "interactKey" || randomKey1 == "interactKey" && randomKey2 == "attackKey")
            {
                KeyCode temp = attackKey;
                attackKey = interactKey;
                interactKey = temp;
                rebindPanel.GetComponentInChildren<TMP_Text>().text = $"[{attackKey}] attack ↔ interact [{interactKey}]";
            }
            else if (randomKey1 == "attackKey" && randomKey2 == "dashKey" || randomKey1 == "dashKey" && randomKey2 == "attackKey")
            {
                KeyCode temp = attackKey;
                attackKey = dashKey;
                dashKey = temp;
                rebindPanel.GetComponentInChildren<TMP_Text>().text = $"[{attackKey}] attack ↔ dash [{dashKey}]";
            }
            else if (randomKey1 == "interactKey" && randomKey2 == "dashKey" || randomKey1 == "dashKey" && randomKey2 == "interactKey")
            {
                KeyCode temp = interactKey;
                interactKey = dashKey;
                dashKey = temp;
                rebindPanel.GetComponentInChildren<TMP_Text>().text = $"[{dashKey}] dash ↔ interact [{interactKey}]";
            }
        }
    }

    // TODO: Add animation and game over screen?
    private void Die()
    {
        isDead = true;
        mainAnim.SetTrigger("death");
        weaponAnim.SetTrigger("death");
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void EventPlayDeathSound()
    {
        GameManager.instance.gameMusic.Pause();
        deathSound.Play();
    }

    public void EventDeathScreen()
    {
        mainAnim.enabled = false;
        GameManager.instance.EndScreen();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Obstacle" && !isDead) GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
