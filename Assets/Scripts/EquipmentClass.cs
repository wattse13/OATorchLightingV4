﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attatched to the EquipmentPrefab GameObject prefab
// Inherits from MonoBehaviour so that it can be attatched to GameObjects
// I don't know if that's a good idea or not.
public class EquipmentClass : MonoBehaviour 
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private Sprite _safeImage;
    [SerializeField]
    private Sprite _unsafeImage;
    [SerializeField]
    private Vector2 _position;
    [SerializeField]
    private string _descriptionSafe;
    [SerializeField]
    private string _descriptionUnsafe;
    [SerializeField]
    private int _initialScale;
    // private int _id;
    // private bool _isSafe;
    // private bool _isActive;
    // private list _animations;
    // private list _sounds;
    [SerializeField]
    private Component _newCollider; // I don't think this is correct

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

    public Component NewCollider
    {
        get
        {
            return _newCollider;
        }
        set
        {
            _newCollider = value;
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
        this.gameObject.GetComponent<SpriteRenderer>().sprite = UnsafeImage;
        this.gameObject.GetComponent<Transform>().position = InitialPosition;
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
