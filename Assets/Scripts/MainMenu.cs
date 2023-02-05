using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(GetRandomArena());
    }

    public void Quit()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    int GetRandomArena()
    {
        return Random.Range(1, 4);
    }

}
