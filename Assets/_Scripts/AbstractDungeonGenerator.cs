using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon ()//Denne er public, så vi kan kalde på metoden fra Unity expectoren
    {
        tilemapVisualizer.Clear(); //Clear tilemap, så man ikke skal gøre det i alle child classes
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
    
}
