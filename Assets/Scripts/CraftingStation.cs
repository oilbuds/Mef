using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    [Tooltip("Name shown in the interact prompt")]
    public string stationName = "Crafting Table";

    [Tooltip("UI panel that opens when using this station")]
    public GameObject craftingPanel;

    public void ToggleCraftingUI()
    {
        if (craftingPanel != null)
        {
            bool newState = !craftingPanel.activeSelf;
            craftingPanel.SetActive(newState);
        }
    }

    // Called by the Craft button in the UI
    public void CraftSulfuricAcid()
    {
        Inventory inventory = Object.FindAnyObjectByType<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("No Inventory found in scene.");
            return;
        }

        // Check if player has required ingredients
        bool hasAcid = inventory.HasItem("Acid", 1);
        bool hasPhosphorus = inventory.HasItem("Sulfur", 1);

        if (hasAcid && hasPhosphorus)
        {
            // Remove ingredients
            inventory.RemoveItem("Acid", 1);
            inventory.RemoveItem("Sulfur", 1);

            // Add result
            bool added = inventory.AddItem("Sulfuric Acid");

            if (added)
            {
                Debug.Log("Crafted: Sulfuric Acid");
            }
            else
            {
                Debug.Log("Could not add Sulfuric Acid to inventory (inventory full).");
            }
        }
        else
        {
            Debug.Log("Need 1 Acid and 1 Phosphorus to craft Sulfuric Acid.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null)
        {
            inventory.currentCraftingStation = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null && inventory.currentCraftingStation == this)
        {
            inventory.currentCraftingStation = null;

            // Hide UI when leaving range
            if (craftingPanel != null)
                craftingPanel.SetActive(false);
        }
    }
}