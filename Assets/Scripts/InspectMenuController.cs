using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to InspectMenuController GameObject
public class InspectMenuController : MonoBehaviour
{
    private Vector2 centerTransform = new Vector2(0, 0);
    private GameObject currentPrefab;
    private int centerSortLayer = 1;
    private int originalSortLayer = 0;

    public GameObject useButton;
    public GameObject replaceButton;

    // Delegates //

    // GameEvents is subscribed to this delegate event
    // Will be used to send a message when a GameObject's safety status has been changed
    public delegate void SafetyStatusEvent(GameObject e);
    public static event SafetyStatusEvent OnStatusChanged;
    // OnStatusChanged?.Invoke(this.gameObject);

    // EquipmentClass is subscribed to this delegate event
    // Will be used to send a message when inspect menu has been activated
    public delegate void InspectMenuEvent(GameObject e);
    public static event InspectMenuEvent OnInspectMenuActivate;

    private void OnEnable()
    {
        GameEvents.OnMessageSent += SetCurrentPrefab;
    }
    private void OnDisable()
    {
        GameEvents.OnMessageSent -= SetCurrentPrefab;
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
        CenterPrefab(currentPrefab);
        ChangeLayerOrder(currentPrefab);
        DisableCollider(currentPrefab);
        // AddBackgroundBlur();
    }

    // Not sure how much sense it makes to move GameObject from a menu controller
    // Moving this EquipmentClass creates other issues, like all GameObjects centering when button is clicked
    // Is it a bad idea to directly access GameObject component, rather than use EquipmentClass properties to change GameObject component values?
    public void CenterPrefab(GameObject myClickedPrefab)
    {
        // OnInspectMenuActivate?.Invoke(myClickedPrefab);
        // Should I first check if GameObject is already centered?
        // Move if not centered
        currentPrefab.GetComponent<Transform>().position = centerTransform;
    }

    // I don't currently know how to access the SpriteRenderer's Order in Layer property
    // Same issue as above function, directly accesses GameObject component instead of using EquipmentClass values to change GameObject component values
    public void ChangeLayerOrder(GameObject myClickedPrefab)
    {
        currentPrefab.GetComponent<SpriteRenderer>().sortingOrder = centerSortLayer;   
    }

    public void DisableCollider(GameObject myClickedPrefab)
    {
        currentPrefab.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void AddBackgroundBlur()
    {
        // SomeSprite.SetActive = true;
    }

    public void ExitMenuFunctions()
    {
        currentPrefab = GetCurrentPrefab();
        ReturnPrefabPosition(currentPrefab);
        ReturnPrefabSortOrder(currentPrefab);
        ReturnPrefabCollider(currentPrefab);
        // Disable BackgroundBlur
    }

    // Same general issues as CallMenuFunctions functions
    // Is using out parameter like this a good idea?
    // It does use EquipmentClass properites to change GameObject-Component values
    public void ReturnPrefabPosition(GameObject myClickedPrefab)
    {
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            currentPrefab.GetComponent<Transform>().position = equipment.InitialPosition;
        }
        
    }

    public void ReturnPrefabSortOrder(GameObject myClickedPrefab)
    {
        currentPrefab.GetComponent<SpriteRenderer>().sortingOrder = originalSortLayer;
    }

    public void ReturnPrefabCollider(GameObject myClickedPrefab)
    {
        currentPrefab.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void RemoveBackBlur()
    {
        // SomeSprite.SetActive = false;
    }
}
