using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<RoomSpawner> spawnPointers = new List<RoomSpawner>();
    public int count = 0;
    public bool check = false;

    void Update()
    {
        if (spawnPointers.Count >= 4 && check == false)
        {
            for (int i = 0; i < 4; i++) // look for number
            {
                for (int j = 0; j < 4; j++) // loop over list to look for number
                {
                    RoomSpawner rs = spawnPointers[j];
                    int op = rs.openingDirection;
                    if (op == i+1)
                    {
                        spawnPointers.RemoveAt(j);
                        spawnPointers.Insert(op - 1, rs);
                        break;
                    }
                }
            }
            check = true;

        }
        else if (spawnPointers.Count > 0 && check == true)
        {
            RoomSpawner rs = spawnPointers[0];
            spawnPointers.RemoveAt(0);

            if (count < 4)
            {
                rs.SpawnBeginning();
            }
            else
            {
                rs.Spawn();
            }
            count++;
        }
    }
}
