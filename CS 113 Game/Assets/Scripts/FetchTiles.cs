using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FetchTiles : MonoBehaviour
{
    public Tilemap tileMap;
    private RoomTiles roomTiles;

    void Start()
    {
        // tileMap = GetComponent<Tilemap>();
        // roomTiles = GameObject.FindGameObjectWithTag("RoomTiles").GetComponent<RoomTiles>();
        
        // for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        // {
        //     for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
        //     {
        //         Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
        //         Vector3 place = tileMap.CellToWorld(localPlace);
        //         if (tileMap.HasTile(localPlace))
        //         {
        //             //Tile at "place"
        //             if (this.gameObject.name == "NavFloor")
        //                 roomTiles.floorTiles.Add(place);
        //             else if (this.gameObject.name == "NavWalls")
        //                 roomTiles.wallTiles.Add(place);
        //         }
        //     }
        // }

        // BoundsInt bounds = tilemap.cellBounds;
        // TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // for (int x = 0; x < bounds.size.x; x++) {
        //     for (int y = 0; y < bounds.size.y; y++) {
        //         TileBase tile = allTiles[x + y * bounds.size.x];
        //         if (tile != null) {
        //             Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
        //         } else {
        //             Debug.Log("x:" + x + " y:" + y + " tile: (null)");
        //         }
        //     }
        // }  
    }
}
