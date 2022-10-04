using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    [SerializeField]
    public SimpleRandomWalkSO roomGenerationParameters;


    protected override void RunProceduralGeneration(){
        CorridorFirstGeneration();
    }
    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions);

        tilemapVisualizer.PaintFloorTiles(floorPositions); // Fortæller tilemapVisualizer hvilke Floortiles der er 
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer); 

    }

private void CreateCorridors(HashSet<Vector2Int> floorPositions){
    var currentPosition = startPosition; // startPosition arves fra SimpleRandomWalkDungeonGenerator
    for (int i = 0; i < corridorCount; i++)
    {
        var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength); //Genererer Corridor positions
        currentPosition = corridor[corridor.Count-1]; //CurrentPosition sættes til lastPostion, altså sidste i List. Sikrer at corridors er connected
        floorPositions.UnionWith(corridor); //Tilføjer corridor til listen floorPositions
    } 
}

}
