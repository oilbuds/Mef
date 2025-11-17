using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;

    [Header("Hotbar")]
    public Text[] hotbarTexts;          // 5 elements
    public Image[] hotbarSlotImages;    // 5 elements for highlight
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    [Header("Backpack")]
    public Text[] backpackTexts;        // 10 elements
    public GameObject backpackPanel;    // The backpack Panel

    [Header("Interaction")]
    public Text interactText;           // "Press E to pick up ___" or "Press E to use ___"

    void Start()
    {
        if (inventory == null)
            inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        RefreshUI();

        // Toggle backpack on/off
        if (Input.GetKeyDown(KeyCode.B) && backpackPanel != null)
        {
            backpackPanel.SetActive(!backpackPanel.activeSelf);
        }

        HandleCursorLock();
    }

    void RefreshUI()
    {
        if (inventory == null) return;

        // ---------------- HOTBAR ----------------
        for (int i = 0; i < hotbarTexts.Length; i++)
        {
            string name = "";

            if (inventory.hotbar != null && i < inventory.hotbar.Length)
            {
                var slot = inventory.hotbar[i];
                if (slot != null && !string.IsNullOrEmpty(slot.itemName))
                    name = slot.itemName;
            }

            if (hotbarTexts[i] != null)
                hotbarTexts[i].text = name;

            if (hotbarSlotImages != null && i < hotbarSlotImages.Length && hotbarSlotImages[i] != null)
            {
                hotbarSlotImages[i].color =
                    (i == inventory.selectedHotbarIndex) ? selectedColor : normalColor;
            }
        }

        // ---------------- BACKPACK ----------------
        for (int i = 0; i < backpackTexts.Length; i++)
        {
            string name = "";

            if (inventory.backpack != null && i < inventory.backpack.Length)
            {
                var slot = inventory.backpack[i];
                if (slot != null && !string.IsNullOrEmpty(slot.itemName))
                    name = slot.itemName;
            }

            if (backpackTexts[i] != null)
                backpackTexts[i].text = name;
        }

        // ---------------- INTERACTION PROMPT ----------------
        if (interactText != null)
        {
            if (inventory.currentPickup != null)
            {
                interactText.text = "Press E to pick up " + inventory.currentPickup.itemName;
            }
            else if (inventory.currentCraftingStation != null)
            {
                interactText.text = "Press E to use " + inventory.currentCraftingStation.stationName;
            }
            else
            {
                interactText.text = "";
            }
        }
    }

    void HandleCursorLock()
    {
        bool anyUIOpen = false;

        // Backpack open?
        if (backpackPanel != null && backpackPanel.activeSelf)
            anyUIOpen = true;

        // Crafting panel open?
        if (!anyUIOpen && inventory != null && inventory.currentCraftingStation != null)
        {
            var station = inventory.currentCraftingStation;
            if (station.craftingPanel != null && station.craftingPanel.activeSelf)
                anyUIOpen = true;
        }

        if (anyUIOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
