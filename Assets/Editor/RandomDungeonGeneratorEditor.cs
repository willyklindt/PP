using System.Collections;
using System.Collections.Generic;
using UnityEditor; //importeret
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator), true)] //Her sætter vi at CustomEditor (unity funktion), at den er af typen AbstractDungeonGenerator). Ved at sætte den til true, så gælder det også for child classes

public class RandomDungeonGeneratorEditor : Editor //arver fra Unity editor
{
    AbstractDungeonGenerator generator;

    private void Awake() {
        generator = (AbstractDungeonGenerator)target;    
    }

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon")){
            generator.GenerateDungeon();
        }
    }
}

//Hele denne kode er ret forvirrende. Men basically gør det bare, at alle subclasses af AbstractDungeonGenerator, får en "Genereate Dungeon" knap i Unity Inspectoren, som man så kan klikke på for at generenere dungeon