using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms //static klasse. Klassen kan derfor ikke blive instatiated
{                                                       //startPosition (hvor algoritmen starter), walkLength (Hvor mange "skridt" algoritmen skal tage)
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength){ //HashSet tillader  en samling af uorginasiret data af unikke elementer
        //SimpleRandomWalk er en Random Walk algoritme, som bruges til at generere gangene mellem dungeon rooms     //HashSet gør brug af Vector2Int. Samlet set gør det, at der ikke opbevares ens data
                                                                                                    // Algortimen skal nemlig ikke kunne "gå" det samme sted to gange. Algortimen skal altså ikke processe samme felter to gange      
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition); //Dette er også af typen Vector2int. Dette er vores startposition
        var previousPosition = startPosition; //Start position gemmes her første gang, i previosPosition

        for (int i = 0; i < walkLength; i++)      { //Forloop kører X antal gange, bestemmes af walkLengt
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirections(); // Kalder på klassen Direction2D, og på metoden GetRandomCardinalDirections. Sætter nye position til tilfædig retning, f.eks. "RIGHT"
            path.Add(newPosition); //Tilføjer det nye tilfældige skridt, f.eks. RIGHT, til listen path. F.eks. starter man i bunden, og så går algortimen til højre. Hele path gemmes altså som en liste
            previousPosition = newPosition;
        }
        return path; //returnerer listen, path (oversigt over hvilken path algoritmen tegner)
    }               
                //Her bruger vi List. Man kan nemlig via List tilgå sidste element i listen, og så kan vi derved finde lastPosition. Altså gå i en tilfældig retning, og derefter gå i en ny tilfældig retning med udgangspunkt i last pos ( så hænger corridors sammen)
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength) //Denne metode vælger en tilfældig retning, og går den vej ift. corridorLength. Dette laver gangene
    {
            List<Vector2Int> corridor = new List<Vector2Int>();
            var direction = Direction2D.GetRandomCardinalDirections(); //Direction bliver sat til random direction 
            var currentPosition = startPosition;
            corridor.Add(currentPosition);
            for (int i = 0; i < corridorLength; i++)
            {
                currentPosition += direction;
                corridor.Add(currentPosition);
            }
            return corridor;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight){
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit); //tager spaceToSplit, og tilføjer dem til queue i roomsQueue
        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue(); //fjerner pågældende element fra queuen
            if (room.size.y >= minHeight && room.size.x >= minWidth) // room.size.y er height af rum. room.size.x er width af rum
            {                                                       //dette if statement kører, hvis et rums width og height er større eller = minimumsgrænsen. Således kan der enten laves et rum, eller der kan deles op i 2 rum
                if(UnityEngine.Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight*2) //Hvis rums højde er større en minHeight * 2
                    {
                        SplitHorizontally(minWidth, minHeight, roomsQueue, room); //Split rum i 2
                    } else if (room.size.x >= minWidth * 2) //ellers hvis rums width er større end minWidth * 2 
                    {
                        SplitVertically(minWidth, minHeight, roomsQueue, room); //Split rum i 2
                    } else //ellers hvis rum ikke kan splittes, så skal den bare laves
                    {
                        roomsList.Add(room); //Tilføjer room til roomsList
                    }
                }
                else //med dette elsestatement, kører vi samme kode som ovenstående (bare omvendt). Altså er der 50% chance for, at den enten starter med at splitte horizontalt, eller om den skal starte med vertikalt. 
                {
                   
                     if (room.size.x >= minWidth * 2) //ellers hvis rums width er større end minWidth * 2 
                    {
                        SplitVertically(minWidth, minHeight, roomsQueue, room); //Split rum i 2
                    } 
                         else if (room.size.y >= minHeight*2) //Hvis rums højde er større en minHeight * 2
                    {
                        SplitHorizontally(minWidth, minHeight, roomsQueue, room); //Split rum i 2
                    } 
                    
                    else //ellers hvis rum ikke kan splittes, så skal den bare laves
                    {
                        roomsList.Add(room); //Tilføjer room til roomsList
                    }

                    
                }
            }
        }
        return roomsList;
        
    }

    private static void SplitVertically(int minWidth, int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int testNummer = 0;
    }
    
    private static void SplitHorizontally(int minWidth, int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int testNummer = 0;
    }
}

public static class Direction2D{
     public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>{ // List of cardinal directions. Omdanner Vector til int værdier, og definerer UP, DOWN, RIGHT, LEFT
        new Vector2Int(0,1), // X,Y værdi. Dette er vores Up direction
        new Vector2Int(1,0), //X,Y, Dette er vores RIGHT direction
        new Vector2Int(0,-1), // X, Y, Dette er vores DOWN direction
        new Vector2Int(-1,0) //X, Y, Dette er vores LEFT direction
     };

        public static Vector2Int GetRandomCardinalDirections(){ //Returnerer en random direction, fra listen cardinalDirectionsList. Fx. kan der vælges "UP", eller "RIGHT"
            return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
        }
}