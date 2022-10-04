using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParameters_", menuName = "PCG/SimpleRandomWalkData")]

public class SimpleRandomWalkSO : ScriptableObject //ScriptableObject henviser til, at man i Unity editor kan lave en ny fil, som hedder Scriptable Object
//Denne fil "arbejder" sammen med SimpleRandomWalkDungeonGenerator.cs

{   //Værdier som kan ændres på i Unity Editor. Se SimpleRandomWalkDungeonGenerator.cs, her er der SerializedField, som gør nedenstående synlige i editor
 public int iterations = 10, walkLength = 10;
 public bool startRandomlyEachIteration = true;
}
