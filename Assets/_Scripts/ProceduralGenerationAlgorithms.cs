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