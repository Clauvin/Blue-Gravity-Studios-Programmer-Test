using System;
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
    [SerializeField] Image iconImage;
    [SerializeField] Color defaultColor;
    [SerializeField] Color selectedColor;
    UnityAction<ShopItem> onSelectedFunction;

    ShopItem panelItem;

    public ShopItem GetShopItem()
    {
        return panelItem;
    }

    public void BindOnInventory(ShopItem item, UnityAction<ShopItem> onSelectedFunctionDoThis)
    {
        BindOnBuying(item, onSelectedFunctionDoThis);
    }

    public void BindOnBuying(ShopItem item, UnityAction<ShopItem> onSelectedFunctionDoThis)
    {
        panelItem = item;

        itemName.text = item.name;
        description.text = item.description;
        price.text = item.cost.ToString();
        iconImage.sprite = item.icon;

        onSelectedFunction = onSelectedFunctionDoThis;

        SetIsSelected(false);
    }

    public void BindOnSelling(ShopItem item, UnityAction<ShopItem> onSelectedFunctionDoThis, float discountOnSelling)
    {
        panelItem = item;
        item.SetResaleVariableCost((int)(item.cost * discountOnSelling));

        itemName.text = item.name;
        description.text = item.description;
        price.text = item.GetResaleVariableCost().ToString();
        iconImage.sprite = item.icon;

        onSelectedFunction = onSelectedFunctionDoThis;

        SetIsSelected(false);
    }

    public void SetIsSelected(bool selected)
    {
        backgroundPanel.color = selected ? selectedColor : defaultColor;
    }

    public void SetCanAfford(bool canAfford)
    {
        price.fontStyle = canAfford ? FontStyles.Normal : FontStyles.Strikethrough;
        price.color = canAfford ? Color.white : Color.red;
    }

    public void OnClicked()
    {
        onSelectedFunction.Invoke(panelItem);
    }


}
