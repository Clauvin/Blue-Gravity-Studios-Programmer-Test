using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI availableFunds;
    [SerializeField] Transform categoryUIRoot;
    [SerializeField] Transform itemUIRoot;

    [SerializeField] GameObject categoryUIPrefab;
    [SerializeField] GameObject itemUIPrefab;

    [SerializeField] List<ShopItem> availableItems;

    ShopItemCategory selectedCategory;
    ShopItem selectedItem;
    List<ShopItemCategory> shopCategories;
    Dictionary<ShopItemCategory, ShopCategoryPanel> shopCategoryToUIMap;
    Dictionary<ShopItem, ShopItemPanel> shopItemToUIMap;

    // Start is called before the first frame update
    void Start()
    {
        RefreshShopUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshShopUI()
    {
        shopCategories = new List<ShopItemCategory>();
        shopCategoryToUIMap = new Dictionary<ShopItemCategory, ShopCategoryPanel>();
        foreach(var item in availableItems)
        {
            if (!shopCategories.Contains(item.category))
            {
                shopCategories.Add(item.category);
            }
        }

        shopCategories.Sort((left, right) => left.Name.CompareTo(right.Name));

        foreach(var category in shopCategories)
        {
            GameObject categoryGo = Instantiate(categoryUIPrefab, categoryUIRoot);
            ShopCategoryPanel categoryUI = categoryGo.GetComponent<ShopCategoryPanel>();

            categoryUI.Bind(category, OnCategorySelected);
            shopCategoryToUIMap[category] = categoryUI; 
        }

        if (!shopCategories.Contains(selectedCategory))
        {
            selectedCategory = null;
        }

        OnCategorySelected(selectedCategory);
    }

    void OnCategorySelected(ShopItemCategory newlySelectedCategory)
    {
        selectedCategory = newlySelectedCategory;
        foreach(ShopItemCategory category in shopCategories)
        {
            shopCategoryToUIMap[category].SetIsSelected(category == selectedCategory);
        }

        RefreshShopUIItems();
    }

    void RefreshShopUIItems()
    {
        shopItemToUIMap = new Dictionary<ShopItem, ShopItemPanel>();

        foreach(ShopItem item in availableItems)
        {
            if (item.category != selectedCategory)
            {
                continue;
            }

            GameObject itemGo = Instantiate(itemUIPrefab, itemUIRoot);
            ShopItemPanel itemUI = itemGo.GetComponent<ShopItemPanel>();

            itemUI.Bind(item, OnItemSelected);
            shopItemToUIMap[item] = itemUI;
        }
    }

    void OnItemSelected(ShopItem newlySelectedItem)
    {
        selectedItem = newlySelectedItem;
        foreach(var kvp in shopItemToUIMap)
        {
            ShopItemPanel item = kvp.Value;
            ShopItem itemUI = kvp.Key;

            item.SetIsSelected(item == selectedItem);
        }
    }

    public void OnClickedPurchase()
    {

    }
}
