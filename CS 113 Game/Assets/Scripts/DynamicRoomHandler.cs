using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DynamicRoomHandler : MonoBehaviour
{
    public Player player;
    public GameObject doors, frontDoors;
    public GameObject goblin, demon, tank, dragon;
    public GameObject obstacle, waypoint, worldChest;
    public bool roomCleared = false;
    public bool enemiesSpawned = false;
    public bool doorsActive = false;
    public string roomType;
    public List<GameObject> enemiesInRoom = new List<GameObject>();
    public RoomTemplates roomTemplates;
    public Transform spawnables;
    public GameObject enemies;

    Vector3 roomCenter;
    List<Vector3> spawnedLocs = new List<Vector3>();

    public References references;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        roomCenter = this.gameObject.transform.position;
        spawnables = transform.Find("Spawnables");
        references = GameObject.FindGameObjectWithTag("References").GetComponent<References>();
        // Set room type
        roomType = "Enemies";

        // Create holder for enemies
        enemies = new GameObject();
        enemies.name = "Enemies";
        enemies.transform.parent = this.gameObject.transform;

        // Doors closed by default
        doors = transform.Find("Doors").gameObject;
        frontDoors = transform.Find("FrontDoors").gameObject;
        doors.SetActive(false);
        frontDoors.SetActive(false);

        // Spawn random amount of enemies
        goblin = references.enemyDict["goblin"];
        demon = references.enemyDict["demon"];
        tank = references.enemyDict["tank"];
        dragon = references.enemyDict["dragon"];

        obstacle = references.objectDict["obstacle"];
        waypoint = references.objectDict["waypoint"];
        worldChest = references.objectDict["world_chest"];
    }

    void Update()
    {
        if (roomCleared) return;
        if (gameObject.name == "TBLR") 
        {
            roomCleared = true;
            GameManager.instance.roomsCleared++;
        }
        else if (player.currentRoom == gameObject && !doorsActive)
        {
            GameObject roomPortals = player.currentRoom.transform.Find("Portals").gameObject;
            roomPortals.SetActive(false);
            doors.SetActive(true);
            frontDoors.SetActive(true);
            doorsActive = true;
        }
        else if (enemies.transform.childCount == 0 && enemiesSpawned)
        {
            GameObject roomPortals = player.currentRoom.transform.Find("Portals").gameObject;
            roomPortals.SetActive(true);
            doors.SetActive(false);
            frontDoors.SetActive(false);
            roomCleared = true;
            GameManager.instance.roomsCleared++;
            if (GetComponent<AddRoom>().spawnChest)
            {
                Instantiate(worldChest, transform.position, Quaternion.identity);
            }
            GetComponent<AddRoom>().mmSquare.GetComponent<SpriteRenderer>().color = new Color(67f/255f, 244f/255f, 54f/255f);
        }
        else if (player.currentRoom == gameObject && doorsActive && !enemiesSpawned)
        {
            // Invoke("SpawnEnemies", 0.5f);
            SpawnEnemiesAndObstacles();
        }
    }

    void SpawnEnemiesAndObstacles()
    {
        List<Vector3> spawnedLocs = new List<Vector3>();

        int randGoblins = 0;
        int randDemons = 0;
        int randTanks = 0;

        if (GetComponent<AddRoom>().bossRoom)
        {
            SpawnRandomNumEnemies(1, dragon);
        }
        else if ((float)GameManager.instance.roomsCleared / roomTemplates.rooms.Count <= 0.20f) // player completed less than 20% of rooms
        {
            randGoblins = Random.Range(1, 4);
        }
        else if ((float)GameManager.instance.roomsCleared / roomTemplates.rooms.Count <= 0.40f) // player completed 20-40% of rooms
        {
            randGoblins = Random.Range(1, 3);
            randDemons = Random.Range(1, 2);
        }
        else if ((float)GameManager.instance.roomsCleared / roomTemplates.rooms.Count <= 0.70f) // player completed 40-70% of rooms
        {
            randGoblins = Random.Range(1, 3);
            randDemons = Random.Range(1, 3);
        }
        else if ((float)GameManager.instance.roomsCleared / roomTemplates.rooms.Count > 0.70f) // player completed 70-100% of rooms
        {
            randGoblins = Random.Range(1, 3);
            randDemons = Random.Range(1, 3);
            randTanks = Random.Range(0, 3);
        }

        SpawnRandomNumEnemies(randGoblins, goblin);
        SpawnRandomNumEnemies(randDemons, demon);
        SpawnRandomNumEnemies(randTanks, tank);

        int randObstacles = Random.Range(1, 5);
        if (GetComponent<AddRoom>().bossRoom) randObstacles = 0;
        for (int i = 0; i < randObstacles; i++)
        {
            while (true)
            {
                bool safe = true;
                float obX = Random.Range(roomCenter.x-2.5f, roomCenter.x+2.5f);
                float obY = Random.Range(roomCenter.y-2.5f, roomCenter.y+2.5f);
                Vector3 randObjLoc = new Vector3(obX, obY, roomCenter.z);
                foreach (Vector3 loc in spawnedLocs)
                {
                    if (Vector3.Distance(randObjLoc, loc) < 1.5f || Vector3.Distance(randObjLoc, roomCenter) < 2f)
                    {
                        safe = false;
                        break;
                    }
                }
                if (!safe) continue;
                GameObject goObj = Instantiate(obstacle, randObjLoc, Quaternion.identity);
                goObj.transform.parent = spawnables.transform;
                spawnedLocs.Add(randObjLoc);
                break;
            }
        }

        enemiesSpawned = true;
    }

    void SpawnRandomNumEnemies(int num, GameObject enemy)
    {
        for (int i = 0; i < num; i++)
        {
            while (true)
            {
                bool safe = true;
                float randX = Random.Range(roomCenter.x-2f, roomCenter.x+2f);
                float randY = Random.Range(roomCenter.y-2f, roomCenter.y+2f);
                Vector3 randLoc = new Vector3(randX, randY, roomCenter.z);
                foreach (Vector3 loc in spawnedLocs)
                {
                    if (Vector3.Distance(randLoc, loc) < 1.5f)
                    {
                        safe = false;
                        break;
                    }
                }
                if (!safe) continue;
                GameObject go = Instantiate(enemy, randLoc, Quaternion.identity);
                go.transform.parent = enemies.transform;
                spawnedLocs.Add(randLoc);
                break;
            }
        }
    }
}
