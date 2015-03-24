using System.Collections.Generic;
using UnityEngine;

public class StoreObjectManager : MonoBehaviour
{
    public static StoreObjectManager Singleton;

    //References
    public Transform GroundGroup;

    //Public
    public List<GameObject> StoreObjects = new List<GameObject>();

    public GameObject ShelvesPrefab;
    public GameObject FridgePrefab;
    public GameObject IceCreamMachinePrefab;

    //Private

    void Awake()
    {
        Singleton = this;
    }

    public void PlaceObject(PlaceableObject.ObjectType objectType, Vector3 position, Quaternion rotation, StockItem containedStockItem, string nameOfParent)
    {
        GameObject newObject = null;

        switch (objectType)
        {
            case PlaceableObject.ObjectType.Shelves:
                newObject = Instantiate(ShelvesPrefab) as GameObject;
                break;

            case PlaceableObject.ObjectType.Fridge:
                newObject = Instantiate(FridgePrefab) as GameObject;
                break;

            case PlaceableObject.ObjectType.IceCreamMachine:
                newObject = Instantiate(IceCreamMachinePrefab) as GameObject;
                break;
        }

        newObject.name = objectType.ToString();
        newObject.transform.SetParent(GroundGroup.FindChild(nameOfParent));
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;
        newObject.GetComponent<PlaceableObject>().OnPlaced();
        newObject.GetComponent<ItemStockContainer>().SetItem(containedStockItem);
        //newObject.GetComponent<ItemStockContainer>().InventoryItem = containedStockItem;

        StoreObjects.Add(newObject);
    }
}