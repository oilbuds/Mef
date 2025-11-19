using UnityEngine;

[System.Serializable]
public class ItemIconEntry
{
    public string itemName;
    public Sprite icon;
}

public class ItemIconDatabase : MonoBehaviour
{
    [Tooltip("List of item names and their associated icons.")]
    public ItemIconEntry[] entries;

    public Sprite GetIcon(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
            return null;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] != null && entries[i].itemName == itemName)
                return entries[i].icon;
        }

        return null;
    }
}
