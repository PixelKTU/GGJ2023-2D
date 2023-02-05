using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void NextMatch()
    {
        SceneManager.LoadScene(GetRandomArena());
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    int GetRandomArena()
    {
        return Random.Range(1, 4);
    }
}
