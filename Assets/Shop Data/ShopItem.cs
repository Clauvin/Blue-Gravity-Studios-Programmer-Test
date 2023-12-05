using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item", fileName = "ShopItem_")]
public class ShopItem : ScriptableObject
{
    public ShopItemCategory category;
    public string name;
    [TextArea(3, 5)] public string description;

    public int cost;
    //debt: works, but honestly we don't need this variable to have a resale cost, the SellingShopUI can take care of it as long as it does not
    //  changes the cost variable value
    private int resaleVariableCost;

    public int GetResaleVariableCost()
    {
        return resaleVariableCost;
    }

    public void SetResaleVariableCost(int newResaleVariableCost)
    {
        resaleVariableCost = newResaleVariableCost;
    }
}
