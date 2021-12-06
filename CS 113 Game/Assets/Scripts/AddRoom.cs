using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    public RoomTemplates templates;
    public Player player;
    public GameObject room;
    public GameObject prevRoom;
    public GameObject mmSquare;
    public bool bossRoom = false;
    public bool spawnChest = false;
    public char op;


    void Awake()
    {
        mmSquare = transform.Find("MM_square").gameObject;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        room = this.gameObject;
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
    }

    void Update()
    {
        if (player.currentRoom == gameObject)
        {
            SpriteRenderer mmSR = mmSquare.GetComponent<SpriteRenderer>();
            mmSR.color = new Color(mmSR.color.r, mmSR.color.g, mmSR.color.b, 0.9f);
        }
        else
        {
            SpriteRenderer mmSR = mmSquare.GetComponent<SpriteRenderer>();
            mmSR.color = new Color(mmSR.color.r, mmSR.color.g, mmSR.color.b, 0.5f);
        }
    }
}
