using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to EquipmentController GameObject
public class EquipmentController : MonoBehaviour
{
    private Vector2 centerTransform = new Vector2(0, 0);
    private GameObject currentPrefab;

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

    #region Focus Prefab
    // Assigns currentPrefab variable to value of the GetCurrentPrefab() return value
    // Calls all functions related to centering and emphasizing inspected GameObject and passes currentPrefab in as argument
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
        if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            currentPrefab.GetComponent<Transform>().position = equipment.InitialPosition;
        }
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
