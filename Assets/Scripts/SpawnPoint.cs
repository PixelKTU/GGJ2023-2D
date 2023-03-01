using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] Vector2 playerSize;
    [Range(0,3)]
    [SerializeField] int playerThatSpawnsHere;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, playerSize);
    }

    private void Awake()
    {
        PlayerSpawnManager.Instance.AddSpawnPosition(transform.position, playerThatSpawnsHere);
    }

}
