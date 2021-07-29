using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to InspectMenuController GameObject
// Also used to control UseMenu
public class InspectMenuController : MonoBehaviour
{
    private GameObject currentPrefab;

    public GameObject blurLayer;

    // Delegates //

    // GameEvents is subscribed to this delegate event
    // SafetyStateController is subscribed to this delegate event
    // EquipmentClass is subscribed to this delegate event. Can't change values there, as it changes values for all GameObjects
    // Will be used to send a message when a GameObject's safety status has been changed
    public delegate void SafetyStatusEvent(GameObject e);
    public static event SafetyStatusEvent OnStatusChanged;
    // OnStatusChanged?.Invoke(this.gameObject);

    // Will be used to send a message when a GameObject's active status has been changed
    public delegate void ActiveStatusEvent(GameObject e);
    public static event ActiveStatusEvent OnActivate;

    // Nothing currently subscribed to this delegate event
    // Will be used to send a message when inspect menu has been activated
    // Not sure this is necessary
    public delegate void InspectMenuEvent(GameObject e);
    public static event InspectMenuEvent OnInspectMenuActivate;

    private void Awake()
    {
        blurLayer.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnMessageSent += SetCurrentPrefab;
    }
    private void OnDisable()
    {
        GameEvents.OnMessageSent -= SetCurrentPrefab;
    }
    
    // Called by clicking Replace button
    // Triggers delegate event
    public void SafetyStateChange(GameObject myClickedPrefab)
    {
        OnStatusChanged?.Invoke(currentPrefab);
        // Change Inspect Menu Text
        // Change Inspect Menu Buttons
        // Debug.Log("Hi SafetyStateChange");
    }

    public void ActiveStateChange(GameObject myClickedPrefab)
    {
        // OnActivate?.Invoke(currentPrefab);
        // Change Use Menu Text
        // Change Use Menu Buttons
    }

    // This method is called once a the OnMessageSent delegate event is triggered
    public void SetCurrentPrefab(GameObject myClickedPrefab)
    {
        currentPrefab = myClickedPrefab;
    }

    // When this function is called by SetCurrentPrefab it should retrun the passed in GameObject parameter as a value
    public GameObject GetCurrentPrefab()
    {
        // Debug.Log("Made it to GetCurrentPrefab");
        return currentPrefab;
    }

    // Assigns currentPrefab variable to value of the GetCurrentPrefab() return value
    // Calls all functions related to centering and emphasizing inspected GameObject and passes currentPrefab in as argument
    public void CallMenuFunctions()
    {
        currentPrefab = GetCurrentPrefab();
        // CenterPrefab(currentPrefab);
        // FocusLayer(currentPrefab);
        // DisableCollider(currentPrefab);
        // AddBackgroundBlur();
    }

    public void AddBackgroundBlur()
    {
        blurLayer.SetActive(true);
    }

    public void ExitMenuFunctions()
    {
        currentPrefab = GetCurrentPrefab();
        // ReturnPrefabPosition(currentPrefab);
        // ReturnPrefabSortLayer(currentPrefab);
        // ReturnPrefabCollider(currentPrefab);
        // RemoveBlur();
    }

    public void RemoveBlur()
    {
        blurLayer.SetActive(false);
    }
}
