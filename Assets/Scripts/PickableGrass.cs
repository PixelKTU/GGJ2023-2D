using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableGrass : MonoBehaviour
{
    public GameObject containedObject;
    public float minSpawnTime = 5f, maxSpawnTime = 10f;
    float currTime, genTime;
    public bool isTaken;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isTaken = false;
        currTime = 0;
        GenerateTime();
    }

    private void Update()
    {
        if (isTaken)
        {
            if (currTime < genTime)
            {
                currTime += Time.deltaTime;
            }
            else
            {
                ShowLeafs();
                currTime = 0;
                GenerateTime();
            }
        }
    }

    public void HideLeafs()
    {
        isTaken = true;
        spriteRenderer.enabled = false;
    }

    public void ShowLeafs()
    {
        isTaken = false;
        spriteRenderer.enabled = true;
    }

    void GenerateTime()
    {
        genTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
