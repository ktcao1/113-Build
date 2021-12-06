using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldChest : MonoBehaviour
{
    public Player player;
    public Weapon weapon;
    public bool opened = false;
    public bool inWindow = false;
    public bool _inTrigger;
    public Sprite openedChestSprite, knockBackUpgradeSprite;
    public RoomTemplates roomTemplates;

    // UI
    private GameObject chestPanel;
    private GameObject confirmPanel;
    private GameObject option1, option2;
    private int option1IND, option2IND;
    private GameObject currentWindow;

    // Rewards
    [SerializeField] private List<Sprite> rewardSprites = new List<Sprite>();
    [SerializeField] private List<string> rewardTitles = new List<string>();
    [SerializeField] private List<string> rewardDescriptions = new List<string>();
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weapon = player.GetComponentInChildren<Weapon>();
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        chestPanel = GameObject.FindGameObjectWithTag("WorldChestPanel").transform.Find("ChestPanel").gameObject;
        confirmPanel = GameObject.FindGameObjectWithTag("WorldChestPanel").transform.Find("ConfirmPanel").gameObject;
        option1 = chestPanel.transform.Find("Option1").gameObject;
        option2 = chestPanel.transform.Find("Option2").gameObject;

        // Randomized rewards
        ChooseRewards();

        currentWindow = chestPanel;
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
            CloseWindow();
        }
    }

    public void SwitchWindowStarter()
    {
        currentWindow.SetActive(false);
        currentWindow = chestPanel;
        currentWindow.SetActive(true);
    }

    public void SetupConfirm(string option)
    {
        confirmPanel.transform.Find("GoBack").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        confirmPanel.transform.Find("Confirm").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();

        if (option == "option1")
        {
            confirmPanel.transform.Find("ChoiceText").gameObject.GetComponent<TMP_Text>().text = rewardDescriptions[option1IND];
            confirmPanel.transform.Find("Image").gameObject.GetComponent<Image>().sprite = rewardSprites[option1IND];
            confirmPanel.transform.Find("GoBack").gameObject.GetComponent<Button>().onClick.AddListener(SwitchWindowStarter);
            confirmPanel.transform.Find("Confirm").gameObject.GetComponent<Button>().onClick.AddListener(delegate{ConfirmSelection(option1IND);});
        }
        else
        {
            confirmPanel.transform.Find("ChoiceText").gameObject.GetComponent<TMP_Text>().text = rewardDescriptions[option2IND];
            confirmPanel.transform.Find("Image").gameObject.GetComponent<Image>().sprite = rewardSprites[option2IND];
            confirmPanel.transform.Find("GoBack").gameObject.GetComponent<Button>().onClick.AddListener(SwitchWindowStarter);
            confirmPanel.transform.Find("Confirm").gameObject.GetComponent<Button>().onClick.AddListener(delegate{ConfirmSelection(option2IND);});
        }
        SwitchWindowConfirm();
    }

    public void SwitchWindowConfirm()
    {
        currentWindow.SetActive(false);
        currentWindow = confirmPanel;
        currentWindow.SetActive(true);
    }

    public void ConfirmSelection(int option)
    {
        if (rewardTitles[option] == "+1 Heart")
        {
            ZeldaHealthBar.instance.AddContainer();
        }
        else if (rewardTitles[option] == "Meat")
        {
            weapon.weaponLevel++;
        }
        else if (rewardTitles[option] == "Small Potion")
        {
            ZeldaHealthBar.instance.AddHearts(2);
        }
        else if (rewardTitles[option] == "Large Potion")
        {
            ZeldaHealthBar.instance.AddHearts(3);
        }
        else if (rewardTitles[option] == "+2 Hearts")
        {
            ZeldaHealthBar.instance.AddContainer();
            ZeldaHealthBar.instance.AddContainer();
        }
        else if (rewardTitles[option] == "Knockback+")
        {
            weapon.pushForce = 1.5f;
            weapon.animator.gameObject.SetActive(false);
            weapon.GetComponent<SpriteRenderer>().sprite = knockBackUpgradeSprite;
            weapon.animator.gameObject.SetActive(true);
            weapon.upgraded = true;
        }
        else if (rewardTitles[option] == "Bow+")
        {
            weapon.canAim = true;
            weapon.upgraded = true;
        }
        else if (rewardTitles[option] == "Meat+")
        {
            weapon.weaponLevel++;
            weapon.weaponLevel++;
        }
        else if (rewardTitles[option] == "Max Potion")
        {
            ZeldaHealthBar.instance.AddHearts(5);
        }
        else if (rewardTitles[option] == "Curse Antidote")
        {
            player.upKey = KeyCode.W;
            player.downKey = KeyCode.S;
            player.leftKey = KeyCode.A;
            player.rightKey = KeyCode.D;

            player.attackKey = KeyCode.F;
            player.interactKey = KeyCode.E;
            player.dashKey = KeyCode.Space;
        }
        else if (rewardTitles[option] == "Barrier")
        {
            ZeldaBarrierBar.instance.AddBarriers(1);
        }
        else if (rewardTitles[option] == "Barrier+")
        {
            ZeldaBarrierBar.instance.AddBarriers(2);
        }
        GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("RedeemChest").GetComponent<AudioSource>().Play();
        opened = true;
        CloseWindow();
    }

    void CashOut()
    {
        option1.GetComponent<Button>().onClick.RemoveAllListeners();
        option2.GetComponent<Button>().onClick.RemoveAllListeners();

        // Set Buttons
        option1.GetComponent<Button>().onClick.AddListener(delegate{SetupConfirm("option1");});
        option2.GetComponent<Button>().onClick.AddListener(delegate{SetupConfirm("option2");});

        // Sprite
        option1.GetComponent<Image>().sprite = rewardSprites[option1IND];
        option2.GetComponent<Image>().sprite = rewardSprites[option2IND];

        // Title
        option1.GetComponentInChildren<TMP_Text>().text = rewardTitles[option1IND];
        option2.GetComponentInChildren<TMP_Text>().text = rewardTitles[option2IND];

        chestPanel.SetActive(true);
        currentWindow = chestPanel;
    }

    public void CloseWindow()
    {
        currentWindow.SetActive(false);
        inWindow = false;
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

    void ChooseRewards()
    {
        List<int> numbersToChooseFrom = new List<int>();

        if ((float)GameManager.instance.roomsCleared / roomTemplates.rooms.Count <= 0.30f) // player completed less than 30% of rooms
        {
            numbersToChooseFrom = new List<int>(new int[]{0, 1, 2, 9, 10});
        }
        else if ((float)GameManager.instance.roomsCleared / roomTemplates.rooms.Count <= 0.60f) // player completed 30-60% of rooms
        {
            numbersToChooseFrom = new List<int>(new int[]{1, 2, 3, 9, 10});
        }
        else if ((float)GameManager.instance.roomsCleared / roomTemplates.rooms.Count > 0.60f) // player completed more than 60% of rooms
        {
            if (weapon.weaponType == "dagger" && !weapon.upgraded)
            {
                numbersToChooseFrom = new List<int>(new int[]{4, 5, 7, 8, 9, 11});
            }

            else if (weapon.weaponType == "bow" && !weapon.upgraded)
            {
                numbersToChooseFrom = new List<int>(new int[]{4, 6, 7, 8, 9, 11});
            }

            else if (weapon.upgraded)
            {
                numbersToChooseFrom = new List<int>(new int[]{4, 7, 8, 9, 11});
            }
        }
        if (ZeldaHealthBar.instance.totalHearts == 15)
        {
            numbersToChooseFrom.Remove(1); 
            numbersToChooseFrom.Remove(4);
        }
        if (ZeldaBarrierBar.instance.currentBarriers == 10) 
        {
            numbersToChooseFrom.Remove(10); 
            numbersToChooseFrom.Remove(11);
        }
        if (GameManager.instance.difficulty == "easy") 
        {
            numbersToChooseFrom.Remove(9);
        }
        SetOptions(numbersToChooseFrom);
    }

    void SetOptions(List<int> list)
    {
        option1IND = list[Random.Range(0, list.Count)];
        list.Remove(option1IND);
        option2IND = list[Random.Range(0, list.Count)];
    }
}
