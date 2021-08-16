using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to the GameEvents GameObject
// Was previously a part of the EquipmentClass
public class OnClickDelegate : MonoBehaviour
{

    // ClickMenuController is subscribed to this delegate event
    // EquipmentController is subscribed to this delegate event
    // InspectMenuContrller is subscribed to this delegate event
    public delegate void ClickEvent(GameObject e);
    public static event ClickEvent OnClicked;

    /* void OnMouseOver()
    {
        Debug.Log("Hi");
        OnClicked?.Invoke(this.gameObject);
    }*/ // Wishful Thinking

    // If an object with a collider is clicked on, the event OnClicked is invoked
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.TryGetComponent(out EquipmentClass equipment))
            {
                OnClicked?.Invoke(hit.collider.gameObject);
            }
        }
    }
}
