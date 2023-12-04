using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class BuyingShopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI availableFunds;
    [SerializeField] Transform categoryUIRoot;
    [SerializeField] Transform itemUIRoot;

    [SerializeField] Button PurchaseButton;

    [SerializeField] GameObject categoryUIPrefab;
    [SerializeField] GameObject itemUIPrefab;

    [SerializeField] List<ShopItem> availableItems;

    public GameObject shopCanvas;

    public GameObject currentInteracter;
    public string interacterNodeOnExit;

    IPurchaser currentPurchaser;
    InventoryInterface currentPurchaserInventory;

    ShopItemCategory selectedCategory;
    ShopItem selectedItem;

    List<ShopItemCategory> shopCategories;
    Dictionary<ShopItemCategory, ShopCategoryPanel> shopCategoryToUIMap;
    Dictionary<ShopItem, ShopItemPanel> shopItemToUIMap;

    // Start is called before the first frame update
    void Start()
    {
        currentPurchaser = FindObjectOfType<Purchaser>();
        currentPurchaserInventory = FindObjectOfType<Inventory>();

        RefreshShopUICommons();
        RefreshShopUICategories();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshShopUICommons()
    {
        if (currentPurchaser != null)
        {
            availableFunds.text = currentPurchaser.GetCurrentFunds().ToString();
        }
        else
        {
            availableFunds.text = string.Empty;
        }

        if (currentPurchaser != null && selectedItem != null &&
                currentPurchaser.GetCurrentFunds() >= selectedItem.cost)
        {
            PurchaseButton.interactable = true;
        }
        else
        {
            PurchaseButton.interactable = false;
        }

        if (shopItemToUIMap != null)
        {
            foreach (KeyValuePair<ShopItem, ShopItemPanel> kvp in shopItemToUIMap)
            {

                ShopItem item = kvp.Key;
                ShopItemPanel itemUI = kvp.Value;

                if (currentPurchaser != null)
                {
                    itemUI.SetCanAfford(item.cost <= currentPurchaser.GetCurrentFunds());
                }
                else
                {
                    itemUI.SetCanAfford(false);
                }
            }
        }
    }

    void RefreshShopUICategories()
    {
        for (int childIndex = categoryUIRoot.childCount - 1; childIndex >= 0; childIndex--)
        {
            GameObject childGO = categoryUIRoot.GetChild(childIndex).gameObject;

            Destroy(childGO);
        }

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
        if (selectedCategory != null && newlySelectedCategory != null && selectedCategory != newlySelectedCategory)
        {
            selectedItem = null;
        }

        selectedCategory = newlySelectedCategory;
        foreach(ShopItemCategory category in shopCategories)
        {
            shopCategoryToUIMap[category].SetIsSelected(category == selectedCategory);
        }

        RefreshShopUIItems();
    }

    void RefreshShopUIItems()
    {
        for (int childIndex = itemUIRoot.childCount - 1; childIndex >= 0; childIndex--)
        {
            GameObject childGO = itemUIRoot.GetChild(childIndex).gameObject;

            Destroy(childGO);
        }

        shopItemToUIMap = new Dictionary<ShopItem, ShopItemPanel>();

        foreach(ShopItem item in availableItems)
        {
            if (item.category != selectedCategory)
            {
                continue;
            }

            GameObject itemGo = Instantiate(itemUIPrefab, itemUIRoot);
            ShopItemPanel itemUI = itemGo.GetComponent<ShopItemPanel>();

            itemUI.BindOnBuying(item, OnItemSelected);
            shopItemToUIMap[item] = itemUI;
        }

        RefreshShopUICommons();
    }

    void OnItemSelected(ShopItem newlySelectedItem)
    {
        selectedItem = newlySelectedItem;
        foreach(var kvp in shopItemToUIMap)
        {
            ShopItem item = kvp.Key;
            ShopItemPanel itemUI = kvp.Value;

            itemUI.SetIsSelected(item == selectedItem);
        }

        RefreshShopUICommons();
    }

    public void OnClickedPurchase()
    {
        if (currentPurchaser.SpendFunds(selectedItem.cost))
        {
            //debt: check if adding the Item to the Inventory adds it or a copy of it.
            currentPurchaserInventory.ReceiveItem(selectedItem);
        }
        RefreshShopUICommons();
    }

    public void OnClickedExit()
    {
        IInteracter interacter = currentInteracter.GetComponent<IInteracter>();
        MainCharacterControl mainCharControl = GameObject.FindGameObjectWithTag("Main Character").GetComponent<MainCharacterControl>();

        CloseShopInterface();

        interacter.TryToContinueConversationWithPlayerCharacter(mainCharControl, interacterNodeOnExit);
    }

    [YarnCommand("OpenBuyingShopInterface")]
    public void OpenShopInterface()
    {
        shopCanvas.SetActive(true);
    }

    [YarnCommand("CloseBuyingShopInterface")]
    public void CloseShopInterface()
    {
        shopCanvas.SetActive(false);
    }
}