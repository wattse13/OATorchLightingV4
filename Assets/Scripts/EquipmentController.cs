using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to EquipmentController GameObject
// Should handle GameObject appearance, position, and other values
// If necessary, should change EquipmentClass values, like isSafe or isActive
public class EquipmentController : MonoBehaviour
{
    private Vector2 centerTransform = new Vector2(0, 0);
    private GameObject currentPrefab;

    private string currentPrefabName;
    private Sprite currentPrefabImage;
    private Sprite currentPrefabUnsafeImage;
    private Sprite currentPrefabSafeImage;
    private Vector2 currentPrefabPosition;
    private string currentPrefabDescr;
    private string currentPrefabDescrUnsafe;
    private string currentPrefabDescrSafe;
    private int currentPrefabID;
    private bool isCurrentPrefabSafe;
    private bool isCurrentPrefabActive;

    // Inspect Menu is subscribed to this delegate event
    // SafetyStateController is subscribed to this delegate event
    // Alerts subscribers that a GameObject safety value has changed
    public delegate void PrefabSafeValueChange(GameObject e);
    public static event PrefabSafeValueChange OnSafetyValueChanged;

    // Inspect Menu is subscribed to this delegate event
    // SafetyStateController is subscribed to this delegate event
    // Alerts subscribers that a GameObject active value has changed
    public delegate void PrefabActiveValueChange(GameObject e);
    public static event PrefabActiveValueChange OnActiveValueChanged;

    private void OnEnable()
    {
        OnClickDelegate.OnClicked += SetCurrentPrefab;
        OnClickDelegate.OnClicked += SetCurrentPrefabValues;
        ClickMenuController.OnInspectClicked += CallMenuFunctions;
        InspectMenuController.OnInspectMenuActivate += ReplaceButtonClicked;
        InspectMenuController.OnInspectMenuActivate += ChangeSprite;
        InspectMenuController.OnInspectMenuDeactivate += ExitMenuFunctions;
        InspectMenuController.OnUseMenuActivate += UseButtonClicked;
    }

    private void OnDisable()
    {
        OnClickDelegate.OnClicked -= SetCurrentPrefab;
        OnClickDelegate.OnClicked -= SetCurrentPrefabValues;
        ClickMenuController.OnInspectClicked -= CallMenuFunctions;
        InspectMenuController.OnInspectMenuActivate -= ReplaceButtonClicked;
        InspectMenuController.OnInspectMenuActivate -= ChangeSprite;
        InspectMenuController.OnInspectMenuDeactivate -= ExitMenuFunctions;
        InspectMenuController.OnUseMenuActivate -= UseButtonClicked;
    }

    // This method is called once a the OnMessageSent delegate event is triggered
    public void SetCurrentPrefab(GameObject myClickedPrefab)
    {
        currentPrefab = myClickedPrefab;
    }

    // When this function is called by SetCurrentPrefab it should retrun the passed in GameObject parameter as a value
    public GameObject GetCurrentPrefab()
    {
        return currentPrefab;
    }

    // Indirectly invokes OnSafetyValueChanged delegate event as delegate event is invoked as part of SetSafteyValue() function
    public void ReplaceButtonClicked()
    {
        SetCurrentPrefabValues(currentPrefab);
        SetSafetyValue(currentPrefab);
    }

    // Indirectly invokes OnActiveValueChanged delegate event as delegate event is invoked as part of SetActiveValue() function
    public void UseButtonClicked()
    {
        SetCurrentPrefabValues(currentPrefab);
        SetActiveValue(currentPrefab);
    }

    // My hope is that setting a buch of variable data here will prevent needing out parameter in other functions
    public void SetCurrentPrefabValues(GameObject myClickedPrefab)
    {
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            currentPrefabName = equipment.Name;
            currentPrefabImage = equipment.CurrentImage;
            currentPrefabUnsafeImage = equipment.UnsafeImage;
            currentPrefabSafeImage = equipment.SafeImage;
            currentPrefabPosition = equipment.InitialPosition; // Name may be kind of confusing
            currentPrefabDescr = equipment.DescriptionCurrent;
            currentPrefabDescrUnsafe = equipment.DescriptionUnsafe;
            currentPrefabDescrSafe = equipment.DescriptionSafe;
            currentPrefabID = equipment.ID;
            isCurrentPrefabSafe = equipment.IsSafe;
            isCurrentPrefabActive = equipment.IsActive;
        }
    }
    
    // Changes the isSafe value of a clicked on GameObject
    // Out parameter is still necessary to change EquipmentClass values
    // Called by ReplaceButtonClicked Function
    public void SetSafetyValue(GameObject myClickedPrefab)
    {
        isCurrentPrefabSafe = true;

        // I think this changes the EquipmentClass value
        if(myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            equipment.IsSafe = true;
        }
        // Alerts subscribers that value has been changed
        OnSafetyValueChanged?.Invoke(myClickedPrefab);
    }

    // Essentially the same function as SetSafetyValue, but it changes GameObject's isActive value
    // Called by UseButtonClicked function
    public void SetActiveValue(GameObject myClickedPrefab)
    {
        if(isCurrentPrefabActive == false)
        {
            isCurrentPrefabActive = true;

            if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
            {
                equipment.IsActive = true;
            }
        }
        else if(isCurrentPrefabActive == true)
        {
            isCurrentPrefabActive = false;

            if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
            {
                equipment.IsActive = false;
            }
        }
        OnActiveValueChanged?.Invoke(myClickedPrefab);
    }

    // If isCurrentPrefabSafe is true, the currentPrefab sprite is changed to safe variant
    // Called whene Replace button is clicked
    // Is this better, or should I do something similiar to the SetSafetyValue function above?
    public void ChangeSprite()
    {
        if(isCurrentPrefabSafe == true)
        {
            currentPrefab.GetComponent<SpriteRenderer>().sprite = currentPrefabSafeImage;
        }
    }

    #region Focus Prefab
    // Assigns currentPrefab variable to value of the GetCurrentPrefab() return value
    // Calls all functions related to centering and emphasizing inspected GameObject and passes currentPrefab in as argument
    // Called when ClickMenu Inspect or Use button is clicked
    public void CallMenuFunctions()
    {
        currentPrefab = GetCurrentPrefab();
        CenterPrefab(currentPrefab);
        FocusLayer(currentPrefab);
        DisableCollider(currentPrefab);
    }

    // Moving this EquipmentClass creates other issues, like all GameObjects centering when button is clicked
    // Is it a bad idea to directly access GameObject component, rather than use EquipmentClass properties to change GameObject component values?
    public void CenterPrefab(GameObject myClickedPrefab)
    {
        // Should I first check if GameObject is already centered?
        // Move if not centered
        currentPrefab.GetComponent<Transform>().position = centerTransform;
    }

    // Same issue as above function, directly accesses GameObject component instead of using EquipmentClass values to change GameObject component values
    public void FocusLayer(GameObject myClickedPrefab)
    {
        currentPrefab.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Focus");
    }

    public void DisableCollider(GameObject myClickedPrefab)
    {
        currentPrefab.GetComponent<BoxCollider2D>().enabled = false;
        // Should disable all box colliders to prevent bugs?
    }

    #endregion

    #region Restore Prefab

    // Called when Inspect or Use Menu Exit or Back buttons are clicked
    public void ExitMenuFunctions()
    {
        currentPrefab = GetCurrentPrefab();
        ReturnPrefabPosition(currentPrefab);
        ReturnPrefabSortLayer(currentPrefab);
        ReturnPrefabCollider(currentPrefab);
    }

    // Same general issues as CallMenuFunctions functions
    // Is using out parameter like this a good idea?
    // It does use EquipmentClass properites to change GameObject-Component values
    public void ReturnPrefabPosition(GameObject myClickedPrefab)
    {
        currentPrefab.transform.position = currentPrefabPosition;
        
        //if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        //{
        //    currentPrefab.GetComponent<Transform>().position = equipment.InitialPosition;
        //}
    }

    public void ReturnPrefabSortLayer(GameObject myClickedPrefab)
    {
        currentPrefab.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Foreground");
    }

    public void ReturnPrefabCollider(GameObject myClickedPrefab)
    {
        currentPrefab.GetComponent<BoxCollider2D>().enabled = true;
    }

    #endregion
}
