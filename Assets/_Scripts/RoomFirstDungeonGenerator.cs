using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; //Bruger unitys random funktion

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator //Arver fra SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
   private int minRoomWidth = 4, minRoomHeight = 4; //Min width og height for rooms
   
   [SerializeField]
   private int dungeonWidth = 20, dungeonHeight = 20; //Definerer størrelsen af dungeon. Udfra denne kan vi så andet sted i koden generere X antal rum, ift størrelsen af dungeon
   
   [SerializeField]
   [Range(0,10)]
   private int offset = 1; //offset sikrer at vægge adskiller rum. Altså så der ikke er rum som kun er adskilt af gulvet, altså 2 rum i 1

    [SerializeField]
   private bool randomWalkRooms = false; //Angiver om vi bruger vores randomWalkRooms algoritme eller ej. Eller om vi skal bruge hele spacet, og genererer floors overalt

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        //Laver rooms via binarySpacePartitioning algoritmen
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight); //BoundsInt bliver casted til Vectors3Int

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomsList);

        List<Vector2Int> roomCenters = new List<Vector2Int>(); //opbevarer liste over rums midte. Bruges til at forbindelse rum med gange
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center)); //Vector3Int bliver casted til Vector2Int type. 
        }
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);


        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)]; //vælger et random rum, som skal forbindes med gange, via roomCenters
        roomCenters.Remove(currentRoomCenter); //Fjerner currentRoom fra roomCenters listen. Da vi nu har forbundet rum, så skal den ikke forbindes igen

        while (roomCenters.Count > 0) //Så længe der er rooms at connecte
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters); //closest får værdi fra FindClosestPointTo metoden. Opbevarer hvilket rum der er tættest på
            roomCenters.Remove(closest); //Fjerner closest fra roomCenters, da vi nu har fundet den og ikke skal finde den igen
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest); 
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor (Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter; //start position for vores corridor
        corridor.Add(position); //Tilføjer position til vores HashSet corridor

        //While loop kører så længe position.y ikke er destination.y
        while(position.y != destination.y)
        {
            if (destination.y > position.y) //hvis destination.y > position.y
            {
                position += Vector2Int.up; //Algortimen går op
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            } else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero; //opbevarer distance over hvilket rum, som er tættest på.
        float distance = float.MaxValue; //Starter med at sættes til meget højt tal. For hver iteration i foreach løkken tjekkes der så, om currentDistance er mindre end distance
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter); //Vector2.Distance returnerer distance mellem position og currentRoomCenter
            if (currentDistance < distance) //hvis currentDistance er mindre end distance
            {
                distance = currentDistance; 
                closest = position;
            }
        }
        return closest; //Returnerer tætteste distance til næste rum
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row); //room.min bliver casted til Vector2Int typen
                    floor.Add(position); //Tilføjer positioner for rooms. Altså vores HashSet floor indeholder nu alle positioner for floors på X og Y aksen
                }
            }
        }
        return floor;
    }

}
