using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; //anvender UnityTilemaps library 

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] //Tillader at se tilemap variabel i unity inspector
 private Tilemap floorTilemap, wallTilemap;
[SerializeField]
 private TileBase floorTile, wallTop;

 public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions) //IENumerable er en generic, og tager derfor i mod enhver form for input. I dette tilfælde, tages der i mod en Vector2Int
{
    PaintTiles(floorPositions, floorTilemap, floorTile);
}

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile){

    foreach (var position in positions)
    {
        PaintSingleTile(tilemap, tile, position);
    }
}

    internal void PaintSingleBasicWall(Vector2Int position)
    {
        PaintSingleTile(wallTilemap, wallTop, position);
    }

    private void PaintSingleTile (Tilemap tilemap, TileBase tile, Vector2Int position){
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles(); //Sletter alle tiles i Unity tilemap 
        wallTilemap.ClearAllTiles(); //Sletter alle wall tiles i unity tilemap
       
    }
}
