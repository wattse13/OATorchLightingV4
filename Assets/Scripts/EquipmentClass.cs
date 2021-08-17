using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to the EquipmentPrefab GameObject prefab
// Inherits from MonoBehaviour so that it can be attatched to GameObjects
// I don't know if that's a good idea or not.
public class EquipmentClass : MonoBehaviour 
{
    private Vector2 centeredPosition = new Vector2(0, 0);
    private Vector2 currentPosition;

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
    [SerializeField]
    private int _initialScale;
    [SerializeField] // Should later be removed so that _iD value is not accidentally changed
    private int _iD;
    [SerializeField]
    private bool _isSafe;
    [SerializeField]
    private bool _isActive;
    // private list _animations;
    // private list _sounds;
    // private sortingLayer _sortingLayer;
    //[SerializeField]
    //private Component _newCollider; // I don't think this is correct

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

    //public Component NewCollider
    //{
    //    get
    //    {
    //        return _newCollider;
    //    }
    //    set
    //    {
    //        _newCollider = value;
    //    }
    //}

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

    public int InitialScale
    {
        get
        {
            return _initialScale;
        }
        set
        {
            _initialScale = value;
        }
    }
    #endregion

    // This will probably be moved to EquipmentFactory at some point
    private void Awake()
    {
        SetInitialValues();
    }

    private void SetInitialValues()
    {
        CurrentImage = UnsafeImage;
        GetComponent<SpriteRenderer>().sprite = CurrentImage;
        GetComponent<Transform>().position = InitialPosition;
        DescriptionCurrent = DescriptionUnsafe;
        UseCurrentDescr = UseUnsafeDescr;
        IsSafe = false;
        IsActive = false;
        // this.gameObject.GetComponent<Transform>().localScale = InitialScale;
    }

    /* public EquipmentClass(string Name, Component NewCollider, Vector2 InitialPosition)
    {
        this.Name = Name;
        // this.Image = Image;
        this.NewCollider = NewCollider;
        this.InitialPosition = InitialPosition;
    }*/
}
