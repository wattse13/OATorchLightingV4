using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] private GameObject modalWindow;
    [SerializeField] private TMP_Text modalText;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject resetButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private TMP_Text exitButtonText;

    public delegate void ResetAction();
    public static event ResetAction OnReset;

    private void OnEnable()
    {
        GameEvents.OnResetMessageSent += ResetMessageRecieved;
        GameEvents.OnContinueMessageSent += ContinueMessageRecieved;
    }

    private void OnDisable()
    {
        GameEvents.OnResetMessageSent -= ResetMessageRecieved;
        GameEvents.OnContinueMessageSent -= ContinueMessageRecieved;
    }

    private void ResetMessageRecieved(string message)
    {
        modalWindow.SetActive(true);
        modalText.text = message;

        prevButton.SetActive(false);
        continueButton.SetActive(false);
        exitButton.SetActive(false);
        
        resetButton.SetActive(true);
    }

    private void ContinueMessageRecieved(string message)
    {
        modalWindow.SetActive(true);
        modalText.text = message;
        exitButtonText.text = "Ignore?";

        exitButton.SetActive(true);
        resetButton.SetActive(false);
        prevButton.SetActive(false);
        continueButton.SetActive(false);
    }

    public void CloseMenu()
    {
        modalWindow.SetActive(false);
    }

    public void ResetButton()
    {
        OnReset?.Invoke();
        modalWindow.SetActive(false);
    }
}
