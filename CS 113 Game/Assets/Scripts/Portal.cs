using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Player player;
    public int openingDirection;
    public GameObject portalExit;
    public Camera minimapCam;
    public AudioSource takePortalSound;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        minimapCam = GameObject.FindGameObjectWithTag("MMCam").GetComponent<Camera>();
        takePortalSound = GameObject.FindGameObjectWithTag("SoundDevice").transform.Find("TakePortal").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            if (portalExit.GetComponentInParent<AddRoom>().bossRoom && GameManager.instance.roomsCleared < GetComponentInParent<AddRoom>().templates.rooms.Count - 1)
            {
                player.bossWarningPanel.SetActive(true);
                return;
            }

            float x = portalExit.GetComponentInParent<AddRoom>().room.transform.position.x;
            float y = portalExit.GetComponentInParent<AddRoom>().room.transform.position.y;
            float mm_x = portalExit.GetComponentInParent<AddRoom>().mmSquare.transform.position.x;
            float mm_y = portalExit.GetComponentInParent<AddRoom>().mmSquare.transform.position.y;
            
            player.currentRoom = portalExit.GetComponentInParent<AddRoom>().room;
            
            takePortalSound.Play();
            player.transform.position = portalExit.transform.position;
            float saveZ = Camera.main.transform.position.z;
            Camera.main.transform.position = new Vector3(x, y, saveZ);
            minimapCam.transform.position = new Vector3(mm_x, mm_y, saveZ);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player") player.bossWarningPanel.SetActive(false);
    }
}
