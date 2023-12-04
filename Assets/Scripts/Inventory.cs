using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<ShopItem> shopItemsInInventory;

    void Awake()
    {
        shopItemsInInventory = new List<ShopItem>();
    }

    public void ReceiveItem(ShopItem item)
    {
        shopItemsInInventory.Add(item);
        shopItemsInInventory.Sort((left, right) => left.name.CompareTo(right.name));
        Debug.Log(shopItemsInInventory);
    }

    public bool RemoveItem(ShopItem item)
    {
        if (shopItemsInInventory.Contains(item))
        {
            shopItemsInInventory.Remove(item);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
