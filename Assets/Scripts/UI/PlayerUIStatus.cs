using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIStatus : MonoBehaviour
{
    [SerializeField] private Sprite testingPlayerImage;

    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Image playerImage;
    [SerializeField] private Transform heartHolder;

    List<GameObject> playerHearts = new List<GameObject>();

    private PlayerHealth playerHealth;
    private CharacterController2D character;

    void Start()
    {

    }

    private void OnDestroy()
    {
        playerHealth.playerDamagedAction -= OnPlayerTakeDamage;
        playerHealth.playerDiedAction -= RemoveAllHearts;
    }

    private void OnPlayerTakeDamage()
    {
        RemoveHeart();
    }

    private void RemoveHeart()
    {
        for (int i = 0; i < playerHearts.Count; i++)
        {
            if (playerHearts[i].activeInHierarchy)
            {
                playerHearts[i].SetActive(false);
                break;
            }
        }
    }

    private void RemoveAllHearts()
    {
        for (int i = 0; i < playerHearts.Count; i++)
        {
            if (playerHearts[i].activeInHierarchy)
            {
                playerHearts[i].SetActive(false);
            }
        }
    }

    private void SpawnHearts(int health)
    {
        for (int i = 0; i < health; i++)
        {
            GameObject heartObject = Instantiate(heartPrefab, heartHolder);
            playerHearts.Add(heartObject);
        }
    }

    private void SetPlayerImage(Sprite sprite)
    {
        playerImage.sprite = sprite;
    }

    public void SetPlayer(CharacterController2D character, PlayerHealth playerHealth)
    {
        this.playerHealth = playerHealth;
        this.character = character;

        this.playerHealth.playerDamagedAction += OnPlayerTakeDamage;
        this.playerHealth.playerDiedAction += RemoveAllHearts;

        SpawnHearts(3);
        SetPlayerImage(character.GetPlayerSkinData().playerHeadSprite);
    }
}
