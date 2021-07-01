using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentFactory : MonoBehaviour // should this inherit from EquipmentClass?
{
    public string myName;
    public Sprite myImage;
    public Component newCollider;

    void Start()
    {
        List<EquipmentClass> equipmentClassList = new List<EquipmentClass>(); // Should this be a dictionary?

        // equipmentClassList.Add(new EquipmentClass(myName, myImage, newCollider)); // how do I access class properties here?
        // equipmentClassList.Add(new EquipmentClass(myName, myImage, newCollider));
    }
}
