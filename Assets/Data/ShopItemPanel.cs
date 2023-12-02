using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI price;

    ShopItem panelItem;

    public void Bind(ShopItem item)
    {
        panelItem = item;

        itemName.text = item.name;
        description.text = item.description;
        price.text = item.cost.ToString();
    }


}
