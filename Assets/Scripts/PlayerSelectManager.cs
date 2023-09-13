using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class PlayerSelectManager : MonoBehaviour
{
    Transform[] panels;
    RegisterInputClass inputClass;
    [SerializeField] int LevelCount = 3;
    [SerializeField] GameObject MenuUI;
    [SerializeField] Transform playerSelectUITransform;
    void Start()
    {
        panels = new Transform[playerSelectUITransform.GetChild(0).GetChild(0).childCount];

        inputClass = new RegisterInputClass();

        inputClass.PlayerRegister.AddPlayer.performed += TryAddDevice;
    }

    bool[] readyBools = new bool[4];
    HashSet<int> deviceIds = new HashSet<int>();
    int cardNow = 0;
    bool canToggleReady = true;
    public void Clear()
    {
        cardNow = 0;
        deviceIds.Clear();
        canToggleReady = true;

        for (int i =0; i < 4; i++)
        {
            readyBools[i] = false;
        }

        for (int i = 0; i < playerSelectUITransform.GetChild(0).GetChild(0).childCount; i++)
        {
            panels[i] = playerSelectUITransform.GetChild(0).GetChild(0).GetChild(i);
            panels[i].GetChild(0).gameObject.SetActive(false);
            panels[i].GetChild(1).gameObject.SetActive(false);

            Transform info = panels[i].GetChild(0).GetChild(0).GetChild(1);
            info.GetChild(0).gameObject.SetActive(true);
            info.GetChild(1).gameObject.SetActive(false);
        }
        
    }

    public void BackButton()
    {
        playerSelectUITransform.gameObject.SetActive(false);
        MenuUI.SetActive(true);
    }

    string[] keyboardButtons = new string[] { "S", "Down", "5", "J" };

    void ChangeReadyVisuals(TextMeshProUGUI textMeshPro, bool val)
    {
        if (val)
        {
            textMeshPro.text = "Ready";
            textMeshPro.color = Color.green;
        }
        else
        {
            textMeshPro.text = "Not Ready";
            textMeshPro.color = Color.red;
        }
    }
    void TryAddDevice(InputAction.CallbackContext callback)
    {
        if (cardNow == 4) return;
        string name = callback.control.name.ToLower();
        int inputType = -1;

        if (name == "s" && !InputManager.keyboardOccupied[0]) inputType = 0;
        else if (name == "downarrow" && !InputManager.keyboardOccupied[1]) inputType = 1;
        else if (name == "numpad5" && !InputManager.keyboardOccupied[2]) inputType = 2;
        else if (name == "j" && !InputManager.keyboardOccupied[3]) inputType = 3;
        else if (name != "s" && name != "downarrow" && name != "numpad5" && name != "j" && !deviceIds.Contains(callback.control.device.deviceId))
        {
            deviceIds.Add(callback.control.device.deviceId);
            inputType = 4;
        }

        if (inputType != -1)
        {
            InputManager.PlayerInput playerInput = InputManager.AddPlayerInput((InputManager.InputType)inputType, callback.control.device);

                if (cardNow == 0) playerInput.inputActions[2].performed += callback => ToggleReadyMain(0);
                else if (cardNow == 1) playerInput.inputActions[2].performed += callback => ToggleReadyMain(1);
                else if (cardNow == 2) playerInput.inputActions[2].performed += callback => ToggleReadyMain(2);
                else if (cardNow == 3) playerInput.inputActions[2].performed += callback => ToggleReadyMain(3);

                if (cardNow == 0) playerInput.inputActions[1].performed += callback => ToggleReadyMain(0);
                else if (cardNow == 1) playerInput.inputActions[1].performed += callback => ToggleReadyMain(1);
                else if (cardNow == 2) playerInput.inputActions[1].performed += callback => ToggleReadyMain(2);
                else if (cardNow == 3) playerInput.inputActions[1].performed += callback => ToggleReadyMain(3);


            panels[cardNow].GetChild(1).gameObject.SetActive(true);
            ChangeReadyVisuals(panels[cardNow].GetChild(1).GetComponent<TextMeshProUGUI>(), false);

            Transform info = panels[cardNow].GetChild(0).GetChild(0).GetChild(1);
            info.GetChild(0).gameObject.SetActive(false);
            info.GetChild(1).gameObject.SetActive(true);
            
            cardNow++;
            if (cardNow != 4)
            {
                panels[cardNow].GetChild(0).gameObject.SetActive(true);
                info = panels[cardNow].GetChild(0).GetChild(0).GetChild(1);
                info.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format("Press:\n{0}\nor Start",string.Join(" / ", keyboardButtons.Where((x,index) => !InputManager.keyboardOccupied[index])));
            }
        }
    }

    void ToggleReadyMain(int index)
    {
        
        if (canToggleReady)
        {
            readyBools[index] = !readyBools[index];
            ChangeReadyVisuals(panels[index].GetChild(1).GetComponent<TextMeshProUGUI>(), readyBools[index]);

            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if (readyBools[i])
                {
                    count++;
                }
            }
            if (count == cardNow && cardNow >= 2)
            {
                StartLevel();
            }
        }
    }

    public void StartSelection()
    {
        inputClass.Enable();
        panels[0].GetChild(0).gameObject.SetActive(true);
        InputManager.Clear();
    }

    void StartLevel()
    {
        canToggleReady = false;
        inputClass.Disable();
        SceneManager.LoadScene(Random.Range(1,LevelCount+1));
    }
}
