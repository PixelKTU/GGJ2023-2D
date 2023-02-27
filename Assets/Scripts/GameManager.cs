using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool gameEnded = false;
    public Action<string> OnGameEnd;//player name

    private List<CharacterController2D> players = new List<CharacterController2D>();

    private void Awake()
    {
        instance = this;
        PlayerSpawnManager.Instance.OnPlayerSpawn += OnPlayerSpawn;
    }

    private void Start()
    {
        gameEnded = false;
    }

    private void OnDestroy()
    {
        PlayerSpawnManager.Instance.OnPlayerSpawn -= OnPlayerSpawn;
    }

    private void OnPlayerSpawn(CharacterController2D characterController, PlayerHealth playerHealth)
    {
        players.Add(characterController);
    }

    public void PlayerDied(CharacterController2D player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }

        if (players.Count == 1)
        {
            CharacterController2D winner = players[0];

            PlayerSkinData skinData = winner.GetPlayerSkinData();
            winner.enabled = false;

            Rigidbody2D rb = winner.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;

            Debug.Log($"player {skinData.skinName} won");
            OnGameEnd?.Invoke(skinData.skinName);
            gameEnded = true;
        }
    }

}
