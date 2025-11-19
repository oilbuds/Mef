using UnityEngine;

[System.Serializable]
public class ItemPrefabEntry
{
    public string itemName;     // Must match the Inventory itemName
    public GameObject prefab;   // Prefab to show in hand
}

public class ItemPrefabDatabase : MonoBehaviour
{
    [Tooltip("Map item names (as used in Inventory) to held-item prefabs.")]
    public ItemPrefabEntry[] entries;

    public GameObject GetPrefab(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
            return null;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] != null && entries[i].itemName == itemName)
            {
                return entries[i].prefab;
            }
        }

        return null;
    }
}
