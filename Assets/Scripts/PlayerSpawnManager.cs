using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] int playerCount;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private List<Vector2> spawnPositions = new List<Vector2>();
    public void AddSpawnPosition(Vector2 pos)
    {
        spawnPositions.Add(pos);
    }

    void SpawnOnePlayer(Vector2 position)
    {
        GameObject obj = Instantiate(playerPrefab, position, Quaternion.identity);
    }

    void SpawnPlayers(int playCount)
    {
        List<Vector2> tempList = new List<Vector2>(spawnPositions);
        
        for (int i = 0; i < playCount; i++)
        {
            Vector2 pos;
            if (tempList.Count == 0)
            {
                pos = spawnPositions[Random.Range(0,spawnPositions.Count)];
            }
            else
            {
                int index = Random.Range(0, tempList.Count);
                pos = tempList[index];
                tempList.RemoveAt(index);
            }
            SpawnOnePlayer(pos);
        }
    }

    private void Start()
    {
        SpawnPlayers(playerCount);
    }
}
