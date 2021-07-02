using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    // public string myName;
    // public Sprite myImage;
    // public Component newCollider;
    // public Vector2 myPosition;
    // public EquipmentClass myEquipment;

    public GameObject EquipmentPrefab; // Does this reference the actual equipmentPrefab object?
    private EquipmentClass equipmentClass;
    // private Component equipmentClassScript; // How do I reference a script component?

    // EquipmentClass myEquipment = new EquipmentClass(string Name, Sprite Image, Component NewCollider);

    private void Awake()
    {
        equipmentClass = EquipmentPrefab.GetComponent<EquipmentClass>();
    }

    void Start()
    {
        
        /* EquipmentClass myEquipment = new EquipmentClass(myName, myImage, newCollider, myPosition);
        //myEquipment(myName, myImage, newCollider);

        myEquipment.Name = myName;
        myEquipment.Image = myImage;
        myEquipment.NewCollider = newCollider;
        myEquipment.InitialPosition = myPosition; */

        Instantiate(EquipmentPrefab, transform.position, Quaternion.identity);
        // Instantiate(EquipmentPrefab, transform.position, Quaternion.identity);

        // (myEquipment)Instantiate(myEquipment, new Vector2(0, 0), Quaternion.identity);

        // equipmentObject = (myEquipment)Instantiate(myEquipment, myPosition, Quaternion.identity); 

        // Instantiate(myEquipment, new Vector2(0, 0), Quaternion.identity);

        // add to equipment factory (later)
    }

    private void OnEnable()
    {
        OnClickDelegate.OnClicked += MyFunction;
    }

    private void OnDisable()
    {
        OnClickDelegate.OnClicked -= MyFunction;
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                // EquipmentPrefab = this.EquipmentPrefab;
                Debug.Log("hi click input");
                // Debug.Log(this.equipmentClass.Name); // The 'this' is wishful thinking probably 
                // MyFunction();
            }
        } */
    } // Not currently needed

    public void MyFunction()
    {
        Debug.Log("Hi MyFunction");
        Debug.Log(this.equipmentClass.Name); // The 'this' is wishful thinking probably 
        // OnClickDelegate.BroadcastMessage();
        // Alert other Controllers
    }
}
