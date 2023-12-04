using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//called like this because IInventory would be a horrible name for an interface.
//Too many potential mistakes with that name
public interface InventoryInterface
{
    void ReceiveItem(ShopItem item);

    bool RemoveItem(ShopItem item);
}

public class Inventory : MonoBehaviour, InventoryInterface
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
    }

    public bool RemoveItem(ShopItem item)
    {
        if (shopItemsInInventory.Contains(item))
        {
            shopItemsInInventory.Remove(item);
            shopItemsInInventory.Sort((left, right) => left.name.CompareTo(right.name));
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
