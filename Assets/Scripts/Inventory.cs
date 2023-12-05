using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//called like this because IInventory would be a horrible name for an interface.
//Too many potential mistakes with that name
public interface InventoryInterface
{
    void ReceiveItem(ShopItem item);

    bool RemoveItem(ShopItem item);

    List<ShopItem> getShopItemList();

    int GetAmountOfEquippedItems();

    bool IsEquipped(ShopItem equipment);

    List<ShopItem> GetEquippedItems();

    List<ShopItemCategory> GetEquippedCategories();
}

public class Inventory : MonoBehaviour, InventoryInterface
{
    List<ShopItem> shopItemsInInventory;
    List<ShopItem> shopItemsEquipped;

    void Awake()
    {
        shopItemsInInventory = new List<ShopItem>();
        shopItemsEquipped = new List<ShopItem>();
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

    public List<ShopItem> getShopItemList()
    {
        return shopItemsInInventory;
    }

    public int GetAmountOfEquippedItems()
    {
        return shopItemsEquipped.Count;
    }

    public bool EquipItem(ShopItem equipment)
    {
        if (equipment == null) return false;

        foreach (ShopItem equip in shopItemsEquipped)
        {
            if (equip.category == equipment.category) return false;
        }

        if (!shopItemsEquipped.Contains(equipment))
        {
            shopItemsEquipped.Add(equipment);
            Debug.Log("Equipped " + equipment.name);
            return true;
        }
        else return false;
    }

    public bool UnequipItem(ShopItem equipment)
    {
        if (equipment == null) return false;

        if (shopItemsEquipped.Contains(equipment))
        {
            shopItemsEquipped.Remove(equipment);
            Debug.Log("Unequipped " + equipment.name);
            return true;
        }
        else return false;
    }

    public bool IsEquipped(ShopItem equipment)
    {
        return shopItemsEquipped.Contains(equipment);
    }

    public List<ShopItem> GetEquippedItems()
    {
        return shopItemsEquipped;
    }

    public List<ShopItemCategory> GetEquippedCategories()
    {
        List<ShopItemCategory> equippedCategories = new List<ShopItemCategory>();

        foreach (ShopItem item in shopItemsEquipped)
        {
            equippedCategories.Add(item.category);
        }

        return equippedCategories;
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
