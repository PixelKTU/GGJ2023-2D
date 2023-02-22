using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] int playerCount;

    [SerializeField] List<PlayerSkinData> playerSkins = new List<PlayerSkinData>();

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

    void SpawnOnePlayer(Vector2 position, int playerIndex)
    {
        GameObject obj = Instantiate(playerPrefab, position, Quaternion.identity);
        CharacterController2D playControl = obj.GetComponent<CharacterController2D>();
        playControl.OnCreatePlayer("Horizontal" + playerIndex, "Vertical" + playerIndex, playerSkins[playerIndex]);
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
            SpawnOnePlayer(pos,i);
        }
    }

    private void Start()
    {
        SpawnPlayers(playerCount);
    }
}
