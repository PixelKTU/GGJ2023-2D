using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;

    public Action<CharacterController2D, PlayerHealth> OnPlayerSpawn;

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
        for (int i = 0; i < 4; i++) {
            spawnPositions[i] = new List<Vector2>();
        }
    }

    private List<Vector2>[] spawnPositions = new List<Vector2>[4];
    public void AddSpawnPosition(Vector2 pos, int playerThatSpawnsHere)
    {
        spawnPositions[playerThatSpawnsHere].Add(pos);
    }

    void SpawnOnePlayer(Vector2 position, int playerIndex)
    {
        GameObject obj = Instantiate(playerPrefab, position, Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().sortingOrder = playerIndex;
        CharacterController2D playControl = obj.GetComponent<CharacterController2D>();
        playControl.OnCreatePlayer(InputManager.GetInputActions(playerIndex), playerSkins[playerIndex]);


        OnPlayerSpawn?.Invoke(playControl, obj.GetComponent<PlayerHealth>());
    }

    void SpawnPlayers(int playCount)
    {
        

        for (int i = 0; i < playCount; i++)
        {
            List<Vector2> tempList = new List<Vector2>(spawnPositions[i]);
            Vector2 pos;
            if (tempList.Count == 0)
            {
                pos = spawnPositions[i][UnityEngine.Random.Range(0, spawnPositions[i].Count)];
            }
            else
            {
                int index = UnityEngine.Random.Range(0, tempList.Count);
                pos = tempList[index];
                tempList.RemoveAt(index);
            }
            SpawnOnePlayer(pos, i);
        }
    }

    private void Start()
    {
        if (InputManager.playerCount == 0)
        {
            SpawnPlayers(playerCount);
        }
        else
        {
            SpawnPlayers(InputManager.playerCount);
        }
    }
}
