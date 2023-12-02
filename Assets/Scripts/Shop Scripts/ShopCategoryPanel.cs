using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopCategoryPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI categoryName;
    [SerializeField] Image backgroundPanel;
    [SerializeField] UnityEngine.Color defaultColor;
    [SerializeField] UnityEngine.Color selectedColor;
    ShopItemCategory panelCategory;
    UnityAction<ShopItemCategory> onSelectFunction;

    public void Bind(ShopItemCategory category, UnityAction<ShopItemCategory> onSelectedFunctionDoThis)
    {
        panelCategory = category;
        categoryName.text = category.Name;
        onSelectFunction = onSelectedFunctionDoThis;

        SetIsSelected(false);
    }

    public void SetIsSelected(bool selected)
    {
        backgroundPanel.color = selected ? selectedColor : defaultColor;
    }

    public void OnClicked()
    {
        onSelectFunction.Invoke(panelCategory);
    }
}
