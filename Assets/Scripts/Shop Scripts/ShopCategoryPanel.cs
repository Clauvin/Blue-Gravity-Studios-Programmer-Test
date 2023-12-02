using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopCategoryPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI categoryName;
    ShopItemCategory panelCategory;

    public void Bind(ShopItemCategory category)
    {
        panelCategory = category;
        categoryName.text = category.Name;
    }
}
