using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;

    [Header("Hotbar")]
    public Text[] hotbarTexts;              // 5 elements (optional, can be left blank)
    public Image[] hotbarSlotImages;        // 5 elements for highlight frame
    public Image[] hotbarIconImages;        // 5 elements for item icons
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    [Header("Backpack")]
    public Text[] backpackTexts;            // 10 elements (optional)
    public Image[] backpackIconImages;      // 10 elements for item icons
    public GameObject backpackPanel;        // The backpack Panel

    [Header("Interaction")]
    public Text interactText;               // "Press E to pick up ___" or "Press E to use ___"

    [Header("Money")]
    public Text moneyText;                  // "Money: $0"

    [Header("Icons")]
    public ItemIconDatabase itemIconDatabase;

    public bool IsUIOpen { get; private set; }

    private PlayerMoney playerMoney;

    void Start()
    {
        if (inventory == null)
            inventory = FindObjectOfType<Inventory>();

        playerMoney = FindObjectOfType<PlayerMoney>();
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

            // Text label (optional – you can clear this if you want icon-only)
            if (hotbarTexts[i] != null)
                hotbarTexts[i].text = name;

            // Highlight color
            if (hotbarSlotImages != null && i < hotbarSlotImages.Length && hotbarSlotImages[i] != null)
            {
                hotbarSlotImages[i].color =
                    (i == inventory.selectedHotbarIndex) ? selectedColor : normalColor;
            }

            // Icon image
            if (hotbarIconImages != null && i < hotbarIconImages.Length && hotbarIconImages[i] != null)
            {
                Sprite icon = (itemIconDatabase != null && !string.IsNullOrEmpty(name))
                    ? itemIconDatabase.GetIcon(name)
                    : null;

                hotbarIconImages[i].sprite = icon;
                hotbarIconImages[i].enabled = icon != null;
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

            // Text label (optional)
            if (backpackTexts[i] != null)
                backpackTexts[i].text = name;

            // Icon image
            if (backpackIconImages != null && i < backpackIconImages.Length && backpackIconImages[i] != null)
            {
                Sprite icon = (itemIconDatabase != null && !string.IsNullOrEmpty(name))
                    ? itemIconDatabase.GetIcon(name)
                    : null;

                backpackIconImages[i].sprite = icon;
                backpackIconImages[i].enabled = icon != null;
            }
        }

        // ---------------- INTERACTION PROMPT ----------------
        if (interactText != null)
        {
            if (inventory.currentPickup != null)
            {
                interactText.text = "Press E to pick up " + inventory.currentPickup.itemName;
            }
            else if (inventory.currentCookingStation != null)
            {
                interactText.text = "Press E to use " + inventory.currentCookingStation.stationName;
            }
            else if (inventory.currentCraftingStation != null)
            {
                interactText.text = "Press E to use " + inventory.currentCraftingStation.stationName;
            }
            else if (inventory.currentDealerStation != null)
            {
                interactText.text = "Press E to talk to " + inventory.currentDealerStation.stationName;
            }
            else
            {
                interactText.text = "";
            }
        }

        // ---------------- MONEY DISPLAY ----------------
        if (moneyText != null)
        {
            if (playerMoney == null)
                playerMoney = FindObjectOfType<PlayerMoney>();

            if (playerMoney != null)
                moneyText.text = "Money: $" + playerMoney.money;
        }
    }

    void HandleCursorLock()
    {
        bool anyUIOpen = false;

        // Backpack
        if (backpackPanel != null && backpackPanel.activeSelf)
            anyUIOpen = true;

        if (inventory != null)
        {
            // Crafting
            if (inventory.currentCraftingStation != null)
            {
                var craft = inventory.currentCraftingStation;
                if (craft.craftingPanel != null && craft.craftingPanel.activeSelf)
                    anyUIOpen = true;
            }

            // Cooking
            if (inventory.currentCookingStation != null)
            {
                var cook = inventory.currentCookingStation;
                if (cook.cookingPanel != null && cook.cookingPanel.activeSelf)
                    anyUIOpen = true;
            }

            // Dealer
            if (inventory.currentDealerStation != null)
            {
                var dealer = inventory.currentDealerStation;
                if (dealer.dealerPanel != null && dealer.dealerPanel.activeSelf)
                    anyUIOpen = true;
            }
        }

        IsUIOpen = anyUIOpen;

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
