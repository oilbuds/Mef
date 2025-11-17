using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Tooltip("Name that will be stored in the inventory")]
    public string itemName = "Item";

    // Called by the Inventory when player presses E
    public void Pickup(Inventory inventory)
    {
        if (inventory == null) return;

        bool added = inventory.AddItem(itemName);

        if (added)
        {
            Debug.Log("Picked up: " + itemName);

            if (inventory.currentPickup == this)
                inventory.currentPickup = null;

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Could not pick up " + itemName + " (inventory full)");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null)
        {
            // Mark this as the pickup the player can interact with
            inventory.currentPickup = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null && inventory.currentPickup == this)
        {
            inventory.currentPickup = null;
        }
    }
}
