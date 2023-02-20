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
    Animator animator;

    [SerializeField]
    List<VegetableSpawnData> vegetableRandomWeights = new List<VegetableSpawnData>();

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isTaken = false;
        currTime = 0;
        animator = GetComponent<Animator>();
        animator.Play("Idle", 0, Random.Range(0,animator.GetCurrentAnimatorStateInfo(0).length));
        GenerateTime();
        containedObject = GetRandomThrowable(vegetableRandomWeights);
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
                animator.SetBool("Cut", false);
                animator.SetBool("Growing", true);
                currTime = 0;
                GenerateTime();
            }
        }
    }

    public void StartPicking()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void HideLeafs()
    {
        isTaken = true;
        animator.SetBool("Cut", true);
    }

    public GameObject GetRandomThrowable(List<VegetableSpawnData> vegetables)
    {
        float sum = 0;
        foreach (var val in vegetables) sum += val.SpawnWeight;

        float randVal = Random.Range(0, sum);

        int index = 0;

        for (int i = 0; i < vegetables.Count; i++)
        {
            randVal -= vegetables[i].SpawnWeight;
            if (randVal < Mathf.Epsilon)
            {
                break;
            }
            index++;
        }
        return vegetables[Mathf.Min(Mathf.Max(index, 0), vegetables.Count - 1)].vegetablePrefab;
    }

    public void ShowLeafs()
    {
        containedObject = GetRandomThrowable(vegetableRandomWeights);
        isTaken = false;
        animator.SetBool("Growing", false);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void GenerateTime()
    {
       genTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
