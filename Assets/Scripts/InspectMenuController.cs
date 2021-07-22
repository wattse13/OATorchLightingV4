using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InspectMenuController : MonoBehaviour
{
    // NOT CURRENTLY USED //

    private TMP_Text myTitle;
    private TMP_Text myText;
    private Canvas inspectMenuCanvas;

    public GameObject backButton;

    private void Start()
    {
        myTitle = GameObject.Find("InspectName").GetComponent<TMP_Text>();
        myText = GameObject.Find("BodyText").GetComponent<TMP_Text>();
        inspectMenuCanvas = GameObject.Find("InspectMenu").GetComponent<Canvas>();

        inspectMenuCanvas.enabled = false;
    }
}
