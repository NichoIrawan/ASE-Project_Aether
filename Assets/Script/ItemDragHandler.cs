using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;
    InventoryController inventoryController;

    public float MinDropDistance = 2f;
    public float MaxDropDistance = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryController = FindFirstObjectByType<InventoryController>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Save original parent to return to it later
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the item with the cursor
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            // Swap items if dropped on a valid slot
            inventoryController.SwapItem(originalSlot, dropSlot);
        }
        else
        {
            if (isWithinInventory(eventData.position))
            {
                // If dropped within inventory but not on a slot, return to original slot
                transform.SetParent(originalParent);
            }
            else
            {
                // If dropped outside inventory, drop the item (implement DropItem method as needed)
                DropItem(originalSlot);
            }
        }

        // Center the item in the slot
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    bool isWithinInventory(Vector2 position)
    {
        RectTransform inventoryRect = originalParent.parent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, position);
    }

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

        // Find Player 
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
            return;
        }

        // Randomize Drop Position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(MinDropDistance, MaxDropDistance);
        Vector2 dropPosition = (Vector2)player.position + dropOffset;

        // Instantiate Dropped Item in the World
        Instantiate(gameObject, dropPosition, Quaternion.identity);

        // Destroy the item in the inventory
        Destroy(gameObject);
    }
}
