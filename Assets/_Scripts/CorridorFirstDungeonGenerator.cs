using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;



    protected override void RunProceduralGeneration(){
        CorridorFirstGeneration();
    }
    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(); //HashSet over positioner for floor
        HashSet<Vector2Int> potentialRoomFloors = new HashSet<Vector2Int>(); //HashSet over potentielle positioner for rum, altså hvor de kan placeres

        CreateCorridors(floorPositions, potentialRoomFloors);

    HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomFloors); //HashSet over roomFloors. Kalder på metoden CreateRooms med parametren potentialRoomFloors

    List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions); //Kalder på metoden FindAllDeadEnds, med parameteren floorPositions.

    CreateRoomsAtDeadEnd(deadEnds, roomPositions); //Denne metode bruger også roomPosition som parametre, for at sikre at deadEnd ikke findes i roomPosition HashSettet (hvis det gør, så er der allerede rum)

    

    floorPositions.UnionWith(roomPositions); //Sammenslutter floorPositions med roomFloors.

        tilemapVisualizer.PaintFloorTiles(floorPositions); // Fortæller tilemapVisualizer hvilke Floortiles der er 
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer); 

    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors){ //Metode tager deadEnds og roomFloors som parametre
        foreach (var position in deadEnds) //Hvor hvert element i deadEnds
        {
            if (roomFloors.Contains(position) == false){ //hvis ingen rum er tilknyttet til position 
                var room = RunRandomWalk(randomWalkParameters, position); //Generer rum
                roomFloors.UnionWith(room); //Tilføj til liste over rooms
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions){ // metode til at finde deadEnds
        List<Vector2Int> deadEnds = new List<Vector2Int>(); //Når metoden kaldes, startes der med at lave en List over deadEnds (tom i starten)
        foreach (var position in floorPositions) //Tjek alle floorpositioner
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList) //Tjek alle directions, om der er "nabo" for floor
            {
                if (floorPositions.Contains(position + direction)){ //Hvis der er "nabo" til floor
                    neighboursCount++; //tilføj til tæller over "naboer"
                }
                
            }
            if (neighboursCount == 1){ //hvis neighboursCount er == 1, så har floortile kun én nabo, altså er der plads til at generere rum, så der ikke opstår deadend
                deadEnds.Add(position); //Tilføj position til listen over deadends
            }
        }
        return deadEnds; //Returner liste over deadends
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomFloors){
        HashSet<Vector2Int> roomFloors = new HashSet<Vector2Int>(); //Laver variablen roomFloors, som laver et nyt HashSet (Altså skal roomFloors bruges til opbevaring af hvor rum skal placeres)
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomFloors.Count * roomPercent); //variablen roomToCreate, int, gemmer hvor mange rum der skal genereres. Der bruges .Count * roomPercent, altså generes der 80% af de rum som skulle generes
                               //Mathf.RoundToInt bruges til at omregne værdien til int)

        List<Vector2Int> roomsToCreate = potentialRoomFloors.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList(); //roomToCreate er List, og vi bruger OrderBy til at putte dem tilfældigt i listen- Guid.NewGuid() laver en global unik identifier (unikt ID nummer). Sorterer altså vores hashSet, potentialRoomFloors
                                                                                           //.Take og .ToList konverterer HashSet til List
        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition); //Kalder på RunRandomWalk, fra klassen SimpleRandomWalkDungeonGenerator.
            roomFloors.UnionWith(roomFloor); //Denne bruges til at sammenslutte roomFloors og roomFloor, så der ikke sker gentagelser i HashSettet
        }
        return roomFloors; //returnerer roomFloors som et HashSet

    }
private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomFloors){
    var currentPosition = startPosition; // startPosition arves fra SimpleRandomWalkDungeonGenerator
    potentialRoomFloors.Add(currentPosition); //Tilføjer nuværende værdi for position, currentPosition, i HashSettet potentialRoomFloors. Altså til start som startPosition
    for (int i = 0; i < corridorCount; i++)
    {
        var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength); //Genererer Corridor positions
        currentPosition = corridor[corridor.Count-1]; //CurrentPosition sættes til lastPostion, altså sidste i List. Sikrer at corridors er connected
        potentialRoomFloors.Add(currentPosition); //Tilføjer nuværende værdi for position, currentPosition. hvor hver gang en corridor genereres. Når corridors generes, opbevares der altså her værdier for hvor rum skal placeres
        floorPositions.UnionWith(corridor); //Tilføjer corridor til listen floorPositions
    } 
}

}
