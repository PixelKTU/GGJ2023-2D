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

    void Start()
    {

    }

    private void OnDestroy()
    {

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

    public void SetPlayer(CharacterController character)
    {
        //character.SO getting all the info about the player
        //character.ontakedamage += ontakedamage

        SpawnHearts(3);
        SetPlayerImage(testingPlayerImage);
    }
}
