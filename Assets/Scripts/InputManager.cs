using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public enum InputType
    {
        WASD,
        Arrows,
        Numpad,
        UHJK,
        Other
    }

    public class PlayerInput
    {
        public PlayerInputClass input { get; set; }
        public int deviceId { get; set; }
        public InputDevice device { get; set; }
        public InputAction[] inputActions { get; set; }
    }

    static List<PlayerInput> playerInputs = new List<PlayerInput>();
    static public bool[] keyboardOccupied = new bool[4];
    public static int playerCount { get { return playerInputs.Count; } }
    public static void Clear()
    {
        for (int i = 0; i < 4; i++)
        {
            keyboardOccupied[i] = false;
        }
        playerInputs.Clear();
    }

    public static PlayerInput AddPlayerInput(InputType type, InputDevice inputDevice)
    {
        PlayerInputClass inputClass = new PlayerInputClass();

        int inputDeviceId = -1; 
        if (inputDevice != null)
        {
            InputSystem.onDeviceChange += (device, change) =>
            {
                if (change == InputDeviceChange.Reconnected)
                {
                    Debug.Log("Reconnected " + device.deviceId);
                }
            };
            inputClass.devices = new UnityEngine.InputSystem.Utilities.ReadOnlyArray<InputDevice>(new InputDevice[] { inputDevice });
            inputDeviceId = inputDevice.deviceId;
        }

        if ((int)type < 4) keyboardOccupied[(int)type] = true;

        InputAction[] actions;
        if ((int)type == 0)
        {
            
            actions = new InputAction[] { inputClass.ControlsKeyboard1.Horizontal, inputClass.ControlsKeyboard1.Jump, inputClass.ControlsKeyboard1.Pickup };
            inputClass.ControlsKeyboard1.Enable();
        }
        else if ((int)type == 1)
        {
            actions = new InputAction[] { inputClass.ControlsKeyboard2.Horizontal, inputClass.ControlsKeyboard2.Jump, inputClass.ControlsKeyboard2.Pickup };
            inputClass.ControlsKeyboard2.Enable();
        }
        else if ((int)type == 2)
        {
            actions = new InputAction[] { inputClass.ControlsKeyboard3.Horizontal, inputClass.ControlsKeyboard3.Jump, inputClass.ControlsKeyboard3.Pickup };
            inputClass.ControlsKeyboard3.Enable();
        }
        else if ((int)type == 3)
        {
            actions = new InputAction[] { inputClass.ControlsKeyboard4.Horizontal, inputClass.ControlsKeyboard4.Jump, inputClass.ControlsKeyboard4.Pickup };
            inputClass.ControlsKeyboard4.Enable();
        }
        else
        {
            actions = new InputAction[] { inputClass.ControlsOther.Horizontal, inputClass.ControlsOther.Jump, inputClass.ControlsOther.Pickup };
            inputClass.ControlsOther.Enable();
        }
        playerInputs.Add(new PlayerInput() { input = inputClass, deviceId = inputDeviceId, device = inputDevice, inputActions = actions });

        
        return playerInputs[playerInputs.Count - 1];
    }

    public static InputAction[] GetInputActions(int index)
    {
        if (index >= 0 && index < playerInputs.Count)
        {
            return playerInputs[index].inputActions;
        }
        if (index >= 0 && index < 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!keyboardOccupied[i])
                {
                    return AddPlayerInput((InputType)i, null).inputActions;
                }
            }
        }
        return null;
    }
}
