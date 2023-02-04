using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> throwables;

    List<PickableGrass> grassList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        grassList = FindObjectsOfType<PickableGrass>().ToList<PickableGrass>();
        foreach(PickableGrass grass in grassList)
        {
            grass.containedObject = throwables[Random.Range(0,throwables.Count)];
        }
    }

}
