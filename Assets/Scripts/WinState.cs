using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a hot fix
// Should probably be done better
// Attatched to WinState Game Object
public class WinState : MonoBehaviour
{
    private bool isAllSafe = false;
    private bool isAllActive = false;

    public delegate void WinEvent(string message);
    public static event WinEvent OnWin;

    private void OnEnable()
    {
        SafetyStateController.OnStatusChanged += SetSafeBool;
        SafetyStateController.OnActiveChanged += SetActiveBool;
    }

    private void OnDisable()
    {
        SafetyStateController.OnStatusChanged -= SetSafeBool;
        SafetyStateController.OnActiveChanged -= SetActiveBool;
    }

    private void SetSafeBool()
    {
        isAllSafe = true;
        SendWinMessage();
    }

    private void SetActiveBool()
    {
        isAllActive = true;
        SendWinMessage();
    }

    private void SendWinMessage()
    {
        if (isAllSafe == true && isAllActive == true)
        {
            OnWin?.Invoke("You Win! Reset and play again?");
        }
        else
        {
            Debug.Log("Safe " + isAllSafe);
            Debug.Log("Active " + isAllActive);
        }
    }
}
