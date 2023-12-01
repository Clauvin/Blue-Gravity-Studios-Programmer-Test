using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ShopFunctions : MonoBehaviour
{
    public GameObject shopCanvas;

    [YarnCommand("OpenShopInterface")]
    public void OpenShopInterface()
    {
        shopCanvas.SetActive(true);
    }

    [YarnCommand("CloseShopInterface")]
    public void CloseShopInterface()
    {
        shopCanvas.SetActive(false);
    }
}
