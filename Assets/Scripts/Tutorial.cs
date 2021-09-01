using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Attached to the Tutorial GameObject in the UI scene
// Destroys itself after all but last message have been cycled through
// This leaves a missing reference on the continue button, but it still works? Is this bad?
public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject modalWindow;
    [SerializeField] private TMP_Text modalText;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject resetButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject tutorialObject;

    [SerializeField]
    [TextArea(3, 10)]
    private List<string> tutorialMessages = new List<string>();

    private int currentText = 0;

    private void Awake()
    {
        modalText.text = "Welcome to the oxygen acetylene welding lesson prototype!";

        continueButton.SetActive(true);
        resetButton.SetActive(false);
        exitButton.SetActive(true);
    }

    public void SetText()
    {
        modalText.text = tutorialMessages[currentText];
    }

    public void BackText()
    {
        if (currentText == 0)
        {
            return;
        } else
        {
            currentText -= 1;
            SetText();
        }
    }

    public void AdvanceText()
    {
        if (currentText == tutorialMessages.Count - 1) // kinda hacky... 
        {
            SelfDestruct();
            return;
        }
        if (currentText < tutorialMessages.Count)
        {
            currentText += 1;
            SetText();
        }
        //else if (currentText >= tutorialMessages.Count)
        //{
        //    SelfDestruct();
        //}
    }

    private void SelfDestruct()
    {
        modalWindow.SetActive(false);
        Destroy(tutorialObject);
    }
}
