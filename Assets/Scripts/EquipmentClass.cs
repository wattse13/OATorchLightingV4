using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentClass : MonoBehaviour 
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private Sprite _image;
    [SerializeField]
    private Vector2 _position;
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

    public Sprite Image
    {
        get
        {
            return _image;
        }
        set
        {
            _image = value;
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
    #endregion

    /* public int Id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    } */

    public EquipmentClass(string Name, Sprite Image, Component NewCollider, Vector2 InitialPosition)
    {
        this.Name = Name;
        this.Image = Image;
        this.NewCollider = NewCollider;
        this.InitialPosition = InitialPosition;
    }

    public delegate void ClickEvent(); // Does this need to return some sort of object id?
    public static event ClickEvent OnClicked; // This probably needs to be non-static if I want specific object instances to trigger an event?

    public void BroadcastMessage()
    {
        OnClicked?.Invoke();
    }
}
