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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isTaken = false;
        currTime = 0;
        animator = GetComponent<Animator>();
        animator.Play("Idle", 0, Random.Range(0,animator.GetCurrentAnimatorStateInfo(0).length));
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

    public void ShowLeafs()
    {
        containedObject = GameManager.instance.GetRandomThrowable();
        isTaken = false;
        animator.SetBool("Growing", false);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void GenerateTime()
    {
       genTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
