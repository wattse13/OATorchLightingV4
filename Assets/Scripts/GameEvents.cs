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

    // public GameObject EquipmentPrefab; // Does this reference the actual equipmentPrefab object? And does this create a dependency?
    // private EquipmentClass equipmentClass;
    // private SendMessageDelegate sendMessageDelegate; // Does this create a dependency?

    private EquipmentFactory equipmentFactory; // Does this create a dependency?

    private void Awake()
    {
        // equipmentClass = GetComponent<EquipmentClass>();
        equipmentFactory = GetComponent<EquipmentFactory>();
    }

    void Start()
    {
        
    }

    private void OnEnable()
    {
        OnClickDelegate.OnClicked += MyFunction;
    }

    private void OnDisable()
    {
        OnClickDelegate.OnClicked -= MyFunction;
    }

    public delegate void MessageEvent(); // Does this need to return some sort of object id?
    public static event MessageEvent OnMessageSent; // This works but feels hacky?

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
        Debug.Log(equipmentFactory.equipmentPrefabs[0].equipmentPrefab.Name); // Currently unable to access list object names

        // Triggers Message Event which alerts Click Menu Controller
        OnMessageSent?.Invoke();
        // sendMessageDelegate.SendBroadcast(); 
    }
}
