using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Attatched to the Main Camera
public class OnClickDelegate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ClickMenuController is subscribed to this delegate event
    // EquipmentController is subscribed to this delegate event
    // InspectMenuContrller is subscribed to this delegate event
    public delegate void ClickEvent(GameObject equipment);
    public static event ClickEvent OnClicked;

    public delegate void HoverEvent(GameObject equipment);
    public static event HoverEvent OnHovered;
    public static event HoverEvent OnHoverExit;

    [SerializeField] private Camera _camera;

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
            Vector2 worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.TryGetComponent(out EquipmentClass equipment))
            {
                OnClicked?.Invoke(hit.collider.gameObject);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("I'm hovering!");

        //Vector2 worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        //if (hit.collider != null && hit.collider.TryGetComponent(out EquipmentClass equipment))
        //{
        //    Debug.Log("I'm hovering!");
        //    OnHovered?.Invoke(hit.collider.gameObject);
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Vector2 worldPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        //if (hit.collider != null && hit.collider.TryGetComponent(out EquipmentClass equipment))
        //{
        //    Debug.Log("I'm not hovering anymore!");
        //    OnHoverExit?.Invoke(hit.collider.gameObject);
        //}
    }
}
