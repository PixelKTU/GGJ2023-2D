using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VegetableSpawnData", menuName = "ScriptableObjects/VegetableSpawnData", order = 1)]
public class VegetableSpawnData : ScriptableObject
{
    public GameObject vegetablePrefab;
    public float SpawnWeight;
}
