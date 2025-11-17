using UnityEngine;
using UnityEngine.UI;

public class CookingStation : MonoBehaviour
{
    [Tooltip("Name shown in the interact prompt")]
    public string stationName = "Cook Station";

    [Header("UI")]
    public GameObject cookingPanel;
    public Slider heatSlider;
    public Text statusText;

    [Tooltip("Green area that shows the 'good' hit zone")]
    public Image goodZoneImage;

    [Header("Minigame Settings")]
    public float heatSpeed = 1.5f;   // how fast the bar moves
    [Range(0f, 1f)] public float goodMin = 0.4f;
    [Range(0f, 1f)] public float goodMax = 0.6f;

    private bool isCooking = false;

    void Update()
    {
        if (cookingPanel != null && cookingPanel.activeSelf)
        {
            UpdateGoodZoneVisual();

            if (isCooking && heatSlider != null)
            {
                // Oscillate slider between 0 and 1
                float t = Mathf.PingPong(Time.time * heatSpeed, 1f);
                heatSlider.value = t;
            }
        }
    }

    void UpdateGoodZoneVisual()
    {
        if (goodZoneImage == null)
            return;

        RectTransform rt = goodZoneImage.rectTransform;

        // Clamp values for safety
        float min = Mathf.Clamp01(goodMin);
        float max = Mathf.Clamp01(goodMax);

        if (max < min)
        {
            float tmp = min;
            min = max;
            max = tmp;
        }

        // Anchor horizontally between min and max, full height
        rt.anchorMin = new Vector2(min, 0f);
        rt.anchorMax = new Vector2(max, 1f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    public void ToggleCookingUI()
    {
        if (cookingPanel != null)
        {
            bool newState = !cookingPanel.activeSelf;
            cookingPanel.SetActive(newState);

            if (!newState)
            {
                isCooking = false;
                if (statusText != null)
                    statusText.text = "";
            }
        }
    }

    // Called by Start Cook button
    public void StartCook()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("No Inventory found in scene.");
            return;
        }

        // Require 1 Sulfuric Acid to attempt a cook
        if (!inventory.HasItem("Sulfuric Acid", 1))
        {
            if (statusText != null)
                statusText.text = "You need 1 Sulfuric Acid to cook.";
            return;
        }

        isCooking = true;

        if (statusText != null)
            statusText.text = "Watch the bar, then hit Lock In!";
    }

    // Called by Lock In button
    public void LockInCook()
    {
        if (!isCooking || heatSlider == null)
            return;

        isCooking = false;

        Inventory inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogWarning("No Inventory found in scene.");
            return;
        }

        float v = heatSlider.value;

        // Remove the ingredient no matter what
        inventory.RemoveItem("Sulfuric Acid", 1);

        if (v >= goodMin && v <= goodMax)
        {
            // Success, give the good product
            bool added = inventory.AddItem("Product");
            if (statusText != null)
                statusText.text = added ? "Cook success! You made Product."
                                        : "Cook success, but inventory is full!";
        }
        else
        {
            // Fail, give a bad item
            bool added = inventory.AddItem("Burnt Mix");
            if (statusText != null)
                statusText.text = added ? "You messed up the cook. Burnt mix..."
                                        : "Bad cook, and inventory is full.";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null)
        {
            inventory.currentCookingStation = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Inventory inventory = other.GetComponent<Inventory>();

        if (inventory != null && inventory.currentCookingStation == this)
        {
            inventory.currentCookingStation = null;

            if (cookingPanel != null)
                cookingPanel.SetActive(false);

            isCooking = false;

            if (statusText != null)
                statusText.text = "";
        }
    }
}
