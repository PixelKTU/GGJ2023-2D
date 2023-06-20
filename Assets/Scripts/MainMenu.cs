using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] PlayerSelectManager selectManager;
    [SerializeField] GameObject PlayerSelectUI;
    public void Play()
    {
        gameObject.SetActive(false);
        PlayerSelectUI.SetActive(true);
        selectManager.Clear();
        selectManager.StartSelection();
    }

    public void Quit()
    {
        Debug.Log("quit");
        Application.Quit();
    }

}
