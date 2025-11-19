using UnityEngine;

public class HeldItemManager : MonoBehaviour
{
    [Header("References")]
    public Inventory inventory;
    public ItemPrefabDatabase prefabDatabase;
    public Transform handTransform;   // HandHolder under the camera

    private string currentItemName = "";
    private GameObject currentInstance;

    void Start()
    {
        if (inventory == null)
            inventory = FindObjectOfType<Inventory>();

        if (prefabDatabase == null)
            prefabDatabase = FindObjectOfType<ItemPrefabDatabase>();
    }

    void Update()
    {
        if (inventory == null || handTransform == null || prefabDatabase == null)
            return;

        string newItemName = GetSelectedHotbarItemName();

        if (newItemName != currentItemName)
        {
            UpdateHeldItem(newItemName);
        }
    }

    string GetSelectedHotbarItemName()
    {
        int index = inventory.selectedHotbarIndex;

        if (inventory.hotbar == null || index < 0 || index >= inventory.hotbar.Length)
            return "";

        var slot = inventory.hotbar[index];
        if (slot == null || string.IsNullOrEmpty(slot.itemName))
            return "";

        return slot.itemName;
    }

    void UpdateHeldItem(string newItemName)
    {
        // Destroy old
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }

        currentItemName = newItemName;

        // If nothing selected or empty slot, leave hand empty
        if (string.IsNullOrEmpty(newItemName))
            return;

        // Look up prefab
        GameObject prefab = prefabDatabase.GetPrefab(newItemName);
        if (prefab == null)
        {
            Debug.LogWarning("No held-item prefab found for: " + newItemName);
            return;
        }

        // Spawn new item as child of hand
        currentInstance = Instantiate(prefab, handTransform);
        currentInstance.transform.localPosition = Vector3.zero;
        currentInstance.transform.localRotation = Quaternion.identity;
        // currentInstance.transform.localScale = Vector3.one;
    }
}
