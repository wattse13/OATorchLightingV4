using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to GameEvents GameObject
// Subscribed to OnClickDelegate
public class GameEvents : MonoBehaviour
{
    private EquipmentFactory equipmentFactory; // Does this create a dependency?
    // private EquipmentClass equipmentClass;

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
        OnClickDelegate.OnClicked += WhoCalled;
    }

    private void OnDisable()
    {
        OnClickDelegate.OnClicked -= WhoCalled;
    }

    // ClickMenuController is currently subscribed to this delegate event
    public delegate void MessageEvent(GameObject e);
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

    // When GameEvents recieves a message from OnClickDelegate, it triggers its own delegate event, OnMessageSent
    // Passes clicked on GameObject as reference
    public void WhoCalled(GameObject myClickedPrefab)
    {
        // Debug.Log("Hi MyFunction");
        // Debug.Log(myClickedPrefab.equipmentClass.Name);
        // Debug.Log(equipmentFactory.equipmentPrefabs[0].gameObject.name); // Changing this to Name as is in the EquipmentClass throws an error

        // Triggers Message Event which alerts Click Menu Controller
        OnMessageSent?.Invoke(myClickedPrefab);

        /*if (myClickedPrefab.TryGetComponent(out EquipmentClass equipment))
            OnMessageSent?.Invoke(myClickedPrefab); */
    }

    #region Code Graveyard

    // public string myName;
    // public Sprite myImage;
    // public Component newCollider;
    // public Vector2 myPosition;
    // public EquipmentClass myEquipment;

    // public GameObject EquipmentPrefab; // Does this reference the actual equipmentPrefab object? And does this create a dependency?
    // private EquipmentClass equipmentClass;
    // private SendMessageDelegate sendMessageDelegate; // Does this create a dependency?

    #endregion
}
