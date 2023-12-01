using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item", fileName = "ShopItem_")]
public class ShopItem : ScriptableObject
{
    public ShopItemCategory category;
    public string name;
    [TextArea(3, 5)] public string description;
}
