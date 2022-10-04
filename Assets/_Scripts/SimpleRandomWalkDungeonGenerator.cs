using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator //Arver fra klassen AbstractDungeonGenerator
{
    

    [SerializeField]
    private SimpleRandomWalkSO randomWalkParameters; //Laver en SerializedField, som er af typen "SimpleRandomWalkSO.cs". 
                                                        //hvorfor er dette smart? Fordi SimpleRandomWalkSO.cs laves til et SO (Scriptable Object) i Unity editor. Så kan man nu lave SaveStates, der gemmer parametre
    
 

   


    protected override void RunProceduralGeneration(){ //Denne klasse er sat til override, da den overskriver/arver metoden fra AbstractDungeonGenerator
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters); //Tillader at child classes af SimpleRandomWalkDungeonGenerator 
        tilemapVisualizer.Clear(); //Clear tilemap, så man ikke skal gøre det i alle child classes
       tilemapVisualizer.PaintFloorTiles(floorPositions);
       WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters){
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path); //Kopierer path til floorPositions. Ved brug af UnionWith sikrer vi, at der ikke forekommer duplicates. Det sikrer at alle generede paths, tilknyttes til tidligere paths (altså er der ikke genereres nye paths, oven i eksisterende)
            if (parameters.startRandomlyEachIteration){ //StartRandomlyEachIteration sikrer, at startPosition i hver iteration ikke starter samme sted (men i stedet, at startPosition sættes et andet tilfældigt sted hvor en anden path slutter, altså så de er connectede)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count)); //ElementAt bruger System.Linq library. Random.Range gør brug af Unitys random library, i stedet for C#'s
                                                                        // floorPositions.ElementAt(Random.Range) - Denne bruges, da HashSet er unindexed, så med dette kan vi tilgå et bestemt element   
            }                                                               // Vi vælger altså en random position, fra vores floorPositions HashSet
            
        }
        return floorPositions;
    }


}
