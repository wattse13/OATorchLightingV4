using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectMenuController : MonoBehaviour
{
    private Transform equipTransform;
    private GameObject currentPrefab;

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
   
    public void SetCurrentPrefab(GameObject myClickedPrefab)
    {
        // Debug.Log(equipment.Name);
        GetCurrentPrefab(myClickedPrefab);
    }

    public GameObject GetCurrentPrefab(GameObject myClickedPrefab)
    {
        myClickedPrefab = currentPrefab;
        Debug.Log("Made it to GetCurrentPrefab");
        return currentPrefab;
    }

    public void CenterPrefab(GameObject currentPrefab)
    {
        this.currentPrefab = currentPrefab;
        if (currentPrefab.TryGetComponent(out EquipmentClass equipment))
        {
            // currentPrefab = myClickedPrefab;
            Debug.Log(equipment.Name);
            Debug.Log("Hi CurrentPrefab");
        }
        // GetCurrentPrefab(currentPrefab);
        // Debug.Log(currentPrefab.Name); // Scope issue
        // equipTransform = equipment.GetComponent<Transform>();
        // equipTransform.position = new Vector2(0, 0);
        // equipment.InitialPosition = new Vector2(0, 0); // But how does it move back?
    }
}
