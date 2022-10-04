using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{                                                      //Floor positions, altså hvor væggene skal være. Denne metode tager også imod parameteren TilemapVisualizer, altså den visualizer der aktuelt bruges i Unity
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {

        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList); 
        foreach (var position in basicWallPositions)
        {
            tilemapVisualizer.PaintSingleBasicWall(position);
        }
    }

        private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
        {
            HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
            foreach (var position in floorPositions)
            {
                foreach (var direction in directionList)
                {      
                    var neighbourPosition = position + direction;
                    if (floorPositions.Contains(neighbourPosition) == false) //Tjekker hvorvidt omkringliggende tile er en wall. Hvis det ikke er en wall, så tilføjes der en Wall
                    wallPositions.Add(neighbourPosition);
                }
            }
            return wallPositions; //returnerer et HashSet af wallPositions
        }
}
