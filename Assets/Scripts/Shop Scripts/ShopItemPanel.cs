using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] Image backgroundPanel;
    [SerializeField] Color defaultColor;
    [SerializeField] Color selectedColor;
    UnityAction<ShopItem> onSelectedFunction;

    ShopItem panelItem;

    public void Bind(ShopItem item, UnityAction<ShopItem> onSelectedFunctionDoThis)
    {
        panelItem = item;

        itemName.text = item.name;
        description.text = item.description;
        price.text = item.cost.ToString();

        onSelectedFunction = onSelectedFunctionDoThis;

        SetIsSelected(false);
    }

    public void SetIsSelected(bool selected)
    {
        backgroundPanel.color = selected ? selectedColor : defaultColor;
    }

    public void OnClicked()
    {
        onSelectedFunction.Invoke(panelItem);
    }

}
