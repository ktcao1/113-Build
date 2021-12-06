using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomTiles : MonoBehaviour
{
    public List<Vector3> floorTiles = new List<Vector3>();
    public List<Vector3> wallTiles = new List<Vector3>();
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile wallTile;

    public void PlaceTiles()
    {
        foreach(Vector3 tile in floorTiles)
        {
            floorTilemap.SetTile(Vector3Int.FloorToInt(tile), floorTile);
        }

        foreach(Vector3 tile in wallTiles)
        {
            wallTilemap.SetTile(Vector3Int.FloorToInt(tile), wallTile);
        }
    }
}
