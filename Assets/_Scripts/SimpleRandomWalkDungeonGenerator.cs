using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator //Arver fra klassen AbstractDungeonGenerator
{
    

    [SerializeField]
    private int iterations = 10; //Antallet af iteration, som vi ønsker at køre vores randomWalk algoritme
    
    [SerializeField]
    public int walkLength = 10; // antallet af skridt, algoritme skal tage
    
    [SerializeField]
    public bool startRandomlyEachIteration = true;

   


    protected override void RunProceduralGeneration(){ //Denne klasse er sat til override, da den overskriver/arver metoden fra AbstractDungeonGenerator
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tilemapVisualizer.Clear(); //Clear tilemap, så man ikke skal gøre det i alle child classes
       tilemapVisualizer.PaintFloorTiles(floorPositions);
    }

    protected HashSet<Vector2Int> RunRandomWalk(){
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, walkLength);
            floorPositions.UnionWith(path); //Kopierer path til floorPositions. Ved brug af UnionWith sikrer vi, at der ikke forekommer duplicates. Det sikrer at alle generede paths, tilknyttes til tidligere paths (altså er der ikke genereres nye paths, oven i eksisterende)
            if (startRandomlyEachIteration){ //StartRandomlyEachIteration sikrer, at startPosition i hver iteration ikke starter samme sted (men i stedet, at startPosition sættes et andet tilfældigt sted hvor en anden path slutter, altså så de er connectede)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count)); //ElementAt bruger System.Linq library. Random.Range gør brug af Unitys random library, i stedet for C#'s
                                                                        // floorPositions.ElementAt(Random.Range) - Denne bruges, da HashSet er unindexed, så med dette kan vi tilgå et bestemt element   
            }                                                               // Vi vælger altså en random position, fra vores floorPositions HashSet
            
        }
        return floorPositions;
    }


}
