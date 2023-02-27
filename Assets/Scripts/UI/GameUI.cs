using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [SerializeField] private PlayerUIStatus playerStatus;

    [SerializeField] private Transform playerStatusHolder;
    [SerializeField] private TMP_Text winnerTitle;
    [SerializeField] private GameObject winnerScreen;
    [SerializeField] private float timeUntilWinUi;

    private List<PlayerUIStatus> playerUIStatuses = new List<PlayerUIStatus>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlayerSpawnManager.Instance.OnPlayerSpawn += OnPlayerSpawn;
    }

    void Start()
    {
        GameManager.instance.OnGameEnd += OnGameEnd;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnGameEnd -= OnGameEnd;
        PlayerSpawnManager.Instance.OnPlayerSpawn -= OnPlayerSpawn;
    }

    private void OnPlayerSpawn(CharacterController2D character, PlayerHealth playerHealth)
    {
        SetUpPlayer(character, playerHealth);
    }

    private void OnGameEnd(string playerName)
    {
        winnerTitle.text = playerName + " wins!";
        StartCoroutine(WaitWinUi());
    }

    IEnumerator WaitWinUi()
    {
        yield return new WaitForSeconds(timeUntilWinUi);
        winnerScreen.GetComponent<Animator>().SetTrigger("GameEnd");
    }

    private void SetUpPlayer(CharacterController2D character, PlayerHealth playerHealth)
    {
        PlayerUIStatus playerUIStatus = Instantiate(playerStatus, playerStatusHolder);

        playerUIStatuses.Add(playerUIStatus);
        playerUIStatus.SetPlayer(character, playerHealth);

        if (playerUIStatuses.Count % 2 == 0)
        {
            playerUIStatus.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void UpdateHealthUI(int playerNumber, int health)
    {

    }
}
