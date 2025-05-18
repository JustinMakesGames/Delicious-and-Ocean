using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Generation/ObjectGenerator")]
public class ObjectGenerator : ScriptableObject
{
    [Tooltip("The objects to be generated")]
    public List<GenerationObjectChances> objects;

    public GenerationObject GetRandomObject(System.Random randomNumGen)
    {
        int randomNum = randomNumGen.Next(0, 100);
        int currentChance = 0;

        foreach (GenerationObjectChances generationObject in objects)
        {
            currentChance += generationObject.chance;
            if (randomNum <= currentChance)
            {
                return generationObject.GO;
            }
        }

        Console.WriteLine("No object was generated, returning last object");
        return objects[objects.Count].GO;
    }

}
[System.Serializable]
public struct GenerationObjectChances
{
    public GenerationObject GO;

    [Tooltip("The chance(to a hundred) of this object being generated")]
    [Range(0, 100)]
    public int chance;
}