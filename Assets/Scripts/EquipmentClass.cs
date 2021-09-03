using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to the EquipmentPrefab GameObject prefab
// Stores data each object needs to function
public class EquipmentClass : MonoBehaviour 
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private Sprite _currentImage;
    [SerializeField]
    private Sprite _safeImage;
    [SerializeField]
    private Sprite _unsafeImage;
    [SerializeField]
    private Vector2 _position;
    [SerializeField] // Should later remove so that _descriptionCurrent is set by script only
    [TextArea(3, 10)]
    private string _descriptionCurrent;
    [SerializeField]
    [TextArea(3, 10)]
    private string _descriptionSafe;
    [SerializeField]
    [TextArea(3, 10)]
    private string _descriptionUnsafe;
    [SerializeField]
    [TextArea(3, 10)]
    private string _useCurrentDescr;
    [SerializeField]
    [TextArea(3, 10)]
    private string _useSafeDescr;
    [SerializeField]
    [TextArea(3, 10)]
    private string _useUnsafeDescr;
    [SerializeField] // Should later be removed so that _iD value is not accidentally changed?
    private int _iD;
    [SerializeField]
    private bool _isSafe;
    [SerializeField]
    private bool _isActive;
    //[SerializeField]
    //private int _initialScale;
    // private list _animations;
    // private list _sounds;

    #region Properties
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    public Sprite CurrentImage
    {
        get
        {
            return _currentImage;
        }
        set
        {
            _currentImage = value;
        }
    }

    public Sprite SafeImage
    {
        get
        {
            return _safeImage;
        }
        set
        {
            _safeImage = value;
        }
    }

    public Sprite UnsafeImage
    {
        get
        {
            return _unsafeImage;
        }
        set
        {
            _unsafeImage = value;
        }
    }

    public Vector2 InitialPosition
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
        }
    }

    public bool IsSafe
    {
        get
        {
            return _isSafe;
        }
        set
        {
            _isSafe = value;
        }
    }

    public bool IsActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;
        }
    }

    public string DescriptionCurrent
    {
        get
        {
            return _descriptionCurrent;
        }
        set
        {
            _descriptionCurrent = value;
        }
    }

    public string DescriptionSafe
    {
        get
        {
            return _descriptionSafe;
        }
        set
        {
            _descriptionSafe = value;
        }
    }

    public string DescriptionUnsafe
    {
        get
        {
            return _descriptionUnsafe;
        }
        set
        {
            _descriptionUnsafe = value;
        }
    }

    public string UseCurrentDescr
    {
        get
        {
            return _useCurrentDescr;
        }
        set
        {
            _useCurrentDescr = value;
        }
    }

    public string UseSafeDescr
    {
        get
        {
            return _useSafeDescr;
        }
        set
        {
            _useSafeDescr = value;
        }
    }

    public string UseUnsafeDescr
    {
        get
        {
            return _useUnsafeDescr;
        }
        set
        {
            _useUnsafeDescr = value;
        }
    }

    public int ID
    {
        get
        {
            return _iD;
        }
    }

    //public int InitialScale
    //{
    //    get
    //    {
    //        return _initialScale;
    //    }
    //    set
    //    {
    //        _initialScale = value;
    //    }
    //}
    #endregion

    private void Awake()
    {
        SetInitialValues();
    }

    private void OnEnable()
    {
        ClickMenuController.OnInspectClicked += DisableColliders;
        ClickMenuController.OnClickMenu += DisableColliders;
        InspectMenuController.OnInspectMenuDeactivate += EnableColliders;
    }

    private void OnDisable()
    {
        ClickMenuController.OnInspectClicked -= DisableColliders;
        ClickMenuController.OnClickMenu -= DisableColliders;
        InspectMenuController.OnInspectMenuDeactivate -= EnableColliders;
    }

    // Polygon Collider on one component stays active until second piece of equipment has been clicked
    // Probably has something to do with how my if statements are set up
    public void DisableColliders()
    {
        if (GetComponent<Collider2D>().enabled == false) // Not sure this check is necessary
        {
            return;
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
        }
        if (GetComponent<PolygonCollider2D>() != null)
        {
            if (GetComponent<PolygonCollider2D>().enabled == false)
            {
                return;
            }
            else
            {
                GetComponent<PolygonCollider2D>().enabled = false;
            }
        }
    }

    // Re-enables colliders after menu has been closed
    public void EnableColliders()
    {
        if (GetComponent<Collider2D>().enabled == true) // Not sure this is necessary
        {
            return;
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
        }
        if (GetComponent<PolygonCollider2D>() != null)
        {
            if (GetComponent<PolygonCollider2D>().enabled == true)
            {
                return;
            }
            else
            {
                GetComponent<PolygonCollider2D>().enabled = true;
            }
        }
    }

    // Sets each object's starting values
    private void SetInitialValues()
    {
        CurrentImage = UnsafeImage;
        GetComponent<SpriteRenderer>().sprite = CurrentImage;
        GetComponent<Transform>().position = InitialPosition;
        DescriptionCurrent = DescriptionUnsafe;
        UseCurrentDescr = UseUnsafeDescr;
        IsSafe = false;
        IsActive = false;
    }
}
