using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterChest : MonoBehaviour
{
    public Player player;
    public Weapon weapon;
    public bool opened = false;
    public bool inWindow = false;
    public bool _inTrigger = false;
    public Sprite openedChestSprite, knockBackUpgradeSprite;

    // UI
    [SerializeField] private GameObject starterPanel;
    [SerializeField] private GameObject knockBackPanel;
    [SerializeField] private GameObject heartsPanel;
    [SerializeField] private GameObject bowPanel;
    [SerializeField] private Sprite bowSprite;
    private GameObject currentWindow;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weapon = player.GetComponentInChildren<Weapon>();
        currentWindow = starterPanel;
    }

    void Update()
    {
        if (opened && gameObject.GetComponent<SpriteRenderer>().sprite != openedChestSprite)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = openedChestSprite;
        }
        else if (_inTrigger && !opened && Input.GetKeyDown(player.interactKey) && !inWindow)
        {
            inWindow = true;
            CashOut();
        }
        else if (_inTrigger && !opened && Input.GetKeyDown(player.interactKey) && inWindow)
        {
            inWindow = false;
            CloseWindow();
        }
    }

    void CashOut()
    {
        starterPanel.SetActive(true);
        currentWindow = starterPanel;
    }

    public void CloseWindow()
    {
        currentWindow.SetActive(false);
        inWindow = false;
    }

    public void SwitchWindowStarter()
    {
        currentWindow.SetActive(false);
        currentWindow = starterPanel;
        currentWindow.SetActive(true);
    }

    public void SwitchWindowKnockBack()
    {
        currentWindow.SetActive(false);
        currentWindow = knockBackPanel;
        currentWindow.SetActive(true);
    }

    public void SwitchWindowHearts()
    {
        currentWindow.SetActive(false);
        currentWindow = heartsPanel;
        currentWindow.SetActive(true);
    }

    public void SwitchWindowBow()
    {
        currentWindow.SetActive(false);
        currentWindow = bowPanel;
        currentWindow.SetActive(true);
    }

    public void ConfirmKnockBack()
    {
        player.hasKnockBack = true;
        weapon.pushForce = 1f;
        weapon.animator.gameObject.SetActive(false);
        weapon.GetComponent<SpriteRenderer>().sprite = knockBackUpgradeSprite;
        weapon.animator.gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("RedeemChest").GetComponent<AudioSource>().Play();
        opened = true;
        CloseWindow();
    }

    public void ConfirmHearts()
    {
        if (ZeldaHealthBar.instance.currentHearts == 15)
        {
            ZeldaBarrierBar.instance.AddBarriers(2);
        }
        else if (ZeldaHealthBar.instance.currentHearts >= 14)
        {
            float remainder = 15 - ZeldaHealthBar.instance.currentHearts;
            float extra = 1 - remainder;
            ZeldaHealthBar.instance.AddContainer();
            ZeldaBarrierBar.instance.AddBarriers(1+extra);
        }
        else
        {
            ZeldaHealthBar.instance.AddContainer();
            ZeldaHealthBar.instance.AddContainer();
        }
        GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("RedeemChest").GetComponent<AudioSource>().Play();
        opened = true;
        CloseWindow();
    }

    public void ConfirmBow()
    {
        weapon.GetComponent<SpriteRenderer>().sprite = bowSprite;
        weapon.weaponType = "bow";
        weapon.attackIcon.sprite = weapon.bowSprite;
        weapon.animator.SetBool("Bow", true);
        GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("RedeemChest").GetComponent<AudioSource>().Play();
        opened = true;
        CloseWindow();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player") _inTrigger = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (!opened && collider.tag == "Player")
        {
            _inTrigger = false;
            CloseWindow();
        }
    }
}
