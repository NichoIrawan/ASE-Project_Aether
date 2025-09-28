using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
    private InventoryController inventoryController;
    [SerializeField]private GameObject hotbarItem;

    public static GameObject EquippedItem;

    public GameObject HotbarPanel;
    public GameObject HotbarPrefab;
    public int slotCount = 2;

    private List<Image> hotbarSlotImages = new List<Image>();
    private Key[] hotbarKeys;

    private void Awake()
    {
        inventoryController = FindFirstObjectByType<InventoryController>();

        foreach(Transform child in HotbarPanel.transform)
        {
            DestroyImmediate(child.gameObject, true);
        }

        for (int i = 0; i < slotCount; i++)
        {
            // Initialize hotbar slots
            GameObject newSlot = Instantiate(HotbarPrefab, HotbarPanel.transform);

            // Get the Image component in the slot to set item sprite later
            Image itemImage = newSlot.transform.Find("HotbarSlot").Find("ItemIcon").GetComponent<Image>();
            itemImage.enabled = false;

            // Add to list for easy access
            hotbarSlotImages.Add(itemImage);
        }

        // Define hotbar keys (1-9,0)
        hotbarKeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            hotbarKeys[i] = i < 9? Key.Digit1 + i : Key.Digit0;
        }
    }

    private void Start()
    {
        UpdateHotbarUI();
    }

    private void Update()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                // Disable equip border in all slots
                foreach (Transform child in HotbarPanel.transform)
                {
                    CanvasGroup border = child.Find("HotbarBorder").GetComponent<CanvasGroup>();
                    border.alpha = 0.0f;
                }

                GameObject selectedHotbar = HotbarPanel.transform.GetChild(i).gameObject;

                // Enable equip border in selected slot
                CanvasGroup selectedBorder = selectedHotbar.GetComponentInChildren<CanvasGroup>();
                selectedBorder.alpha = 1.0f;

                // Equip the item in the selected hotbar slot
                var item = inventoryController.GetItemInSlot(i);

                if (item != null)
                {
                    var equippedItem = hotbarItem.transform.GetChild(i);
                    if (equippedItem != null)
                    {
                        EquippedItem = equippedItem.gameObject;
                        equippedItem.gameObject.SetActive(true);
                    }
                }

                Debug.Log($"Equipping {EquippedItem}");
            }
        }
    }

    private void OnEnable()
    {
        // Subscribe to inventory change event
        InventoryController.OnInventoryChanged += UpdateHotbarUI;
    }

    private void OnDisable()
    {
        // Unsubscribe from inventory change event
        InventoryController.OnInventoryChanged -= UpdateHotbarUI;
    }

    void UpdateHotbarUI()
    {
        Debug.Log($"Loaded item data to hotbar.");

        var items = hotbarItem.GetComponentsInChildren<Item>(true);
        foreach(Item n in items)
        {
            Destroy(n.gameObject);
        }

        for (int i = 0; i < slotCount; i++)
        {
            GameObject item = inventoryController.GetItemInSlot(i);

            if (item != null)
            {
                // Set item image in hotbar slot
                hotbarSlotImages[i].sprite = item.GetComponent<Image>().sprite;
                hotbarSlotImages[i].enabled = true;

                // Instantiate the item prefab as a child of the player (for usage)
                Instantiate(item, hotbarItem.transform);
            }
            else
            {
                // Clear hotbar slot if no item
                hotbarSlotImages[i].sprite = null;
                hotbarSlotImages[i].enabled = false;
            }

            items = hotbarItem.GetComponentsInChildren<Item>(true);
            foreach (Item n in items)
            {
                n.GetComponentInParent<SpriteRenderer>().enabled = false;
                n.GetComponentInParent<CircleCollider2D>().enabled = false;
            }
        }

        Debug.Log($"Updated hotbar UI.");
    }
}
