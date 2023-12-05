using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI availableFunds;
    [SerializeField] Transform categoryUIRoot;
    [SerializeField] Transform itemUIRoot;

    [SerializeField] Button equipUnequipButton;

    [SerializeField] GameObject categoryUIPrefab;
    [SerializeField] GameObject itemUIPrefab;

    [SerializeField] List<ShopItem> availableItems;

    public GameObject inventoryCanvas;

    public GameObject currentInteracter;

    IPurchaser currentOwner;
    InventoryInterface currentOwnerInventory;

    ShopItemCategory selectedCategory;
    ShopItem selectedItem;

    List<ShopItemCategory> shopCategories;
    Dictionary<ShopItemCategory, ShopCategoryPanel> shopCategoryToUIMap;
    Dictionary<ShopItem, ShopItemPanel> shopItemToUIMap;

    // Start is called before the first frame update
    void Start()
    {
        currentOwner = FindObjectOfType<Purchaser>();
        currentOwnerInventory = FindObjectOfType<Inventory>();

        LoadItemsToEquip();
        RefreshInventoryUICommons();
        RefreshInventoryUICategories();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadItemsToEquip()
    {
        for (int childIndex = itemUIRoot.childCount - 1; childIndex >= 0; childIndex--)
        {
            GameObject childGO = itemUIRoot.GetChild(childIndex).gameObject;

            Destroy(childGO);
        }

        availableItems = new List<ShopItem>();

        List<ShopItem> ownerInventory = currentOwnerInventory.getShopItemList();

        foreach (ShopItem item in ownerInventory)
        {
            availableItems.Add(item);
        }
    }

    void RefreshInventoryUICommons()
    {
        if (currentOwner != null)
        {
            availableFunds.text = currentOwner.GetCurrentFunds().ToString();
        }
        else
        {
            availableFunds.text = string.Empty;
        }

        equipUnequipButton.interactable = true;
       
        if (shopItemToUIMap != null)
        {
            foreach (KeyValuePair<ShopItem, ShopItemPanel> kvp in shopItemToUIMap)
            {
                ShopItem item = kvp.Key;
                ShopItemPanel itemUI = kvp.Value;

                if (currentInteracter.GetComponent<Inventory>().IsEquipped(item))
                {
                    for (int childIndex = itemUIRoot.childCount - 1; childIndex >= 0; childIndex--)
                    {
                        GameObject childGO = itemUIRoot.GetChild(childIndex).gameObject;
                        ShopItemPanel instantiatedShopItemPanel = childGO.GetComponent<ShopItemPanel>();
                        if (instantiatedShopItemPanel.GetShopItem() == item)
                        {
                            //debt: we should not count that component 0 is always the name
                            instantiatedShopItemPanel.GetComponentsInChildren<TMP_Text>()[0].text = item.name + " - Equipped";
                        }
                        else
                        {
                            instantiatedShopItemPanel.GetComponentsInChildren<TMP_Text>()[0].text = item.name;
                        }
                    }

                }
            }
        }
    }

    void RefreshInventoryUICategories()
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

            itemUI.BindOnInventory(item, OnItemSelected);
            shopItemToUIMap[item] = itemUI;
        }

        RefreshInventoryUICommons();
    }

    void OnItemSelected(ShopItem newlySelectedItem)
    {
        selectedItem = newlySelectedItem;
        foreach(var kvp in shopItemToUIMap)
        {
            ShopItem item = kvp.Key;
            ShopItemPanel itemUI = kvp.Value;

            itemUI.SetIsSelected(item == selectedItem);

            if (currentInteracter.GetComponent<Inventory>().IsEquipped(item))
            {
                equipUnequipButton.GetComponentInChildren<TMP_Text>().text = "Unequip";
            }
            else
            {
                equipUnequipButton.GetComponentInChildren<TMP_Text>().text = "Equip";
            }
        }

        RefreshInventoryUICommons();
    }

    public void OnClickedToEquip()
    {
        if (selectedItem != null)
        {
            if (currentInteracter.GetComponent<Inventory>().IsEquipped(selectedItem))
            {
                currentInteracter.GetComponent<Inventory>().UnequipItem(selectedItem);
            }
            else
            {
                currentInteracter.GetComponent<Inventory>().EquipItem(selectedItem);
            }
        }

        OnItemSelected(selectedItem);
        RefreshInventoryUICommons();
        RefreshInventoryUICategories();
    }

    public void OnClickedExit()
    {
        MainCharacterControl mainCharControl = GameObject.FindGameObjectWithTag("Main Character").GetComponent<MainCharacterControl>();

        CloseInventoryInterface();

        mainCharControl.TryToEndConversationWithPlayerCharacter();
    }

    [YarnCommand("OpenInventoryInterface")]
    public void OpenInventoryInterface()
    {
        inventoryCanvas.SetActive(true);
        Start();
    }

    [YarnCommand("CloseInventoryInterface")]
    public void CloseInventoryInterface()
    {
        inventoryCanvas.SetActive(false);
    }
}