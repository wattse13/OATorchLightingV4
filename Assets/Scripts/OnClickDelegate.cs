using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to the EquipmentPrefab GameObject prefab
// Was previously a part of the EquipmentClass
public class OnClickDelegate : MonoBehaviour
{
    public delegate void ClickEvent(); // Does this need to return some sort of object id?
    public static event ClickEvent OnClicked;


    // If an object with a collider is clicked on, the event OnClicked is invoked
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                OnClicked?.Invoke();
            }
        }
    }

    public void BroadcastMessage()
    {
        // Debug.Log("hi MyFunction");
        // What does this actually do? Nothing, currently
        // I think I want it to send a message to other controller scripts
    }
}
