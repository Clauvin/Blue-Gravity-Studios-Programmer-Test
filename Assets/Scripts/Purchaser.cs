using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchaser
{
    float GetCurrentFunds();
    bool SpendFunds(int amount);
}
public class Purchaser : MonoBehaviour, IPurchaser
{

    [SerializeField] int currentFunds;

    public float GetCurrentFunds()
    {
        return currentFunds;
    }

    public bool SpendFunds(int amount)
    {
        if (currentFunds >= amount)
        {
            currentFunds -= amount;
            return true;
        }

        return false;
    }

    public bool RetrieveFunds(int amount)
    {
        if (amount < 0) return false;
        else {
            currentFunds += amount;
            return true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
