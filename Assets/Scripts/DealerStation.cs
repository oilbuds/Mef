using UnityEngine;
using UnityEngine.UI;

public class DealerStation : MonoBehaviour
{
    [Tooltip("Name shown in the interact prompt")]
    public string stationName = "Dealer";

    [Header("UI")]
    public GameObject dealerPanel;
    public Text dealerInfoText;

    [Header("Prices per item")]
    public int productPrice = 100;
    public int burntMixPrice = 20;

    private PlayerMoney playerMoney;

    void Start()
    {
        playerMoney = FindObjectOfType<PlayerMoney>();
    }

    public void ToggleDealerUI()
    {
        if (dealerPanel != null)
        {
            bool newState = !dealerPanel.activeSelf;
            dealerPanel.SetActive(newState);

            if (newState)
            {
                UpdateDealerInfo();
            }
        }
    }

    void UpdateDealerInfo()
    {
        if (dealerInfoText == null) return;

        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            dealerInfoText.text = "No inventory found.";
            return;
        }

        int productCount = inventory.CountItem("Product");
        int burntCount = inventory.CountItem("Burnt Mix");

        int totalValue = productCount * productPrice + burntCount * burntMixPrice;

        dealerInfoText.text =
            "You have:\n" +
            productCount + " x Product @ $" + productPrice + "\n" +
            burntCount + " x Burnt Mix @ $" + burntMixPrice + "\n\n" +
            "Total Value: $" + totalValue + "\n\n" +
            "Press 'Sell All' to sell everything.";
    }

    // Called by the Sell button
    public void SellAll()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory == null || playerMoney == null)
        {
            Debug.LogWarning("Missing Inventory or PlayerMoney.");
            return;
        }

        int productCount = inventory.CountItem("Product");
        int burntCount = inventory.CountItem("Burnt Mix");

        if (productCount == 0 && burntCount == 0)
        {
            if (dealerInfoText != null)
                dealerInfoText.text = "You have nothing to sell.";
            return;
        }

        int totalValue = productCount * productPrice + burntCount * burntMixPrice;

        // Remove items from inventory
        if (productCount > 0)
            inventory.RemoveItem("Product", productCount);

        if (burntCount > 0)
            inventory.RemoveItem("Burnt Mix", burntCount);

        // Add money
        playerMoney.AddMoney(totalValue);

        if (dealerInfoText != null)
        {
            dealerInfoText.text =
                "Sold everything for $" + totalValue + "!\n\n" +
                "Come back when you have more product.";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null)
        {
            inventory.currentDealerStation = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null && inventory.currentDealerStation == this)
        {
            inventory.currentDealerStation = null;

            if (dealerPanel != null)
                dealerPanel.SetActive(false);
        }
    }
}
