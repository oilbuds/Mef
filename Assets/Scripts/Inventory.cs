using UnityEngine;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class InventorySlot
    {
        public string itemName;
    }

    // 5-slot hotbar, 10-slot backpack
    public InventorySlot[] hotbar = new InventorySlot[5];
    public InventorySlot[] backpack = new InventorySlot[10];

    public int selectedHotbarIndex = 0;

    // Item the player is currently standing near
    [HideInInspector] public ItemPickup currentPickup;

    // Crafting station the player is near
    [HideInInspector] public CraftingStation currentCraftingStation;

    // Cooking station the player is near
    [HideInInspector] public CookingStation currentCookingStation;

    // Dealer station the player is near
    [HideInInspector] public DealerStation currentDealerStation;

    void Start()
    {
        // Initialize slots (no default items)
        for (int i = 0; i < hotbar.Length; i++)
        {
            if (hotbar[i] == null)
                hotbar[i] = new InventorySlot();
        }

        for (int i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] == null)
                backpack[i] = new InventorySlot();
        }

        SelectHotbarSlot(0);
    }

    void Update()
    {
        HandleHotbarInput();
        HandleInteraction();
    }

    void HandleHotbarInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectHotbarSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectHotbarSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectHotbarSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectHotbarSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectHotbarSlot(4);
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Priority: pick up item if one is in range
            if (currentPickup != null)
            {
                currentPickup.Pickup(this);
            }
            // Then cook if at a cooking station
            else if (currentCookingStation != null)
            {
                currentCookingStation.ToggleCookingUI();
            }
            // Then craft if at a crafting station
            else if (currentCraftingStation != null)
            {
                currentCraftingStation.ToggleCraftingUI();
            }
            // Then dealer if at a dealer station
            else if (currentDealerStation != null)
            {
                currentDealerStation.ToggleDealerUI();
            }
        }
    }

    void SelectHotbarSlot(int index)
    {
        if (index < 0 || index >= hotbar.Length) return;

        selectedHotbarIndex = index;

        string name = string.IsNullOrEmpty(hotbar[index].itemName)
            ? "(empty)"
            : hotbar[index].itemName;

        Debug.Log($"Selected hotbar slot {index + 1}: {name}");
    }

    // ---------------------- INVENTORY HELPERS ----------------------

    public bool AddItem(string itemName)
    {
        // Try hotbar first
        for (int i = 0; i < hotbar.Length; i++)
        {
            if (string.IsNullOrEmpty(hotbar[i].itemName))
            {
                hotbar[i].itemName = itemName;
                Debug.Log($"Added {itemName} to hotbar slot {i + 1}");
                return true;
            }
        }

        // Then backpack
        for (int i = 0; i < backpack.Length; i++)
        {
            if (string.IsNullOrEmpty(backpack[i].itemName))
            {
                backpack[i].itemName = itemName;
                Debug.Log($"Added {itemName} to backpack slot {i + 1}");
                return true;
            }
        }

        Debug.Log("Inventory full! Could not add: " + itemName);
        return false;
    }

    public bool HasItem(string itemName, int count)
    {
        int total = CountItem(itemName);
        return total >= count;
    }

    public int CountItem(string itemName)
    {
        int total = 0;

        // Count in hotbar
        for (int i = 0; i < hotbar.Length; i++)
        {
            if (hotbar[i] != null && hotbar[i].itemName == itemName)
                total++;
        }

        // Count in backpack
        for (int i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] != null && backpack[i].itemName == itemName)
                total++;
        }

        return total;
    }

    public bool RemoveItem(string itemName, int count)
    {
        int remaining = count;

        // Remove from hotbar first
        for (int i = 0; i < hotbar.Length; i++)
        {
            if (remaining <= 0) break;

            if (hotbar[i] != null && hotbar[i].itemName == itemName)
            {
                hotbar[i].itemName = "";
                remaining--;
            }
        }

        // Then from backpack
        for (int i = 0; i < backpack.Length; i++)
        {
            if (remaining <= 0) break;

            if (backpack[i] != null && backpack[i].itemName == itemName)
            {
                backpack[i].itemName = "";
                remaining--;
            }
        }

        return remaining <= 0;
    }
}
