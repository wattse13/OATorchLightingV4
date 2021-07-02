﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentFactory : MonoBehaviour // Should this inherit from EquipmentClass? Trying to do so gives me a constructor error
{
    public GameObject EquipmentPrefab; // Does this reference the actual equipmentPrefab object? And does this create a dependency?
    // private EquipmentClass equipmentClass;
    public List<GameObject> equipmentPrefabs; // Needs to be public or line 26 returns Null Reference Exemption

    /* public string myName;
    public Sprite myImage;
    public Component newCollider;
    public Vector2 myPosition; */

    private void Awake()
    {
        List<GameObject> equipmentPrefabs = new List<GameObject>(); // Should this be of type GameObject?

        // equipmentClass = EquipmentPrefab.GetComponent<EquipmentClass>();
    }

    void Start()
    {
        GameObject newEquipmentPrefab = (GameObject)Instantiate(EquipmentPrefab, transform.position, Quaternion.identity);
        equipmentPrefabs.Add(newEquipmentPrefab);

        Debug.Log(equipmentPrefabs.Count);
        // equipmentPrefabs.Add(new EquipmentClass(myName, myImage, newCollider, myPosition)); // how do I access class properties here?
    }
}
