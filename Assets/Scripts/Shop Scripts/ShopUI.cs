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

    List<ShopItemCategory> shopCategories;
    Dictionary<ShopItemCategory, ShopCategoryPanel> shopCategoryToUIMap;

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
    }

    void OnCategorySelected(ShopItemCategory category)
    {

    }

    public void OnClickedPurchase()
    {

    }
}
