using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to InspectMenuController GameObject
public class InspectMenuController : MonoBehaviour
{
    private Vector2 centerTransform = new Vector2(0, 0);
    private GameObject currentPrefab;
    // private GameObject prefabReturn;

    public GameObject useButton;
    public GameObject replaceButton;

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
    // Calls CenterPrefab() function and passes currentPrefab in as argument
    public void CallCP()
    {
        currentPrefab = GetCurrentPrefab();
        CenterPrefab(currentPrefab);
    }

    // This function should be called with a button OnClick method
    public void CenterPrefab(GameObject myClickedPrefab)
    {
        if (currentPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            // Check where object currently is
            if(equipment.InitialPosition != centerTransform)
            {
                // Need to store original position value?
                equipment.InitialPosition = centerTransform;
            }
            // Move if not centered
            // Change layer order
            // Enable alternate background (should a different script handle this?)
            Debug.Log(equipment.Name);
        }
    }
}
