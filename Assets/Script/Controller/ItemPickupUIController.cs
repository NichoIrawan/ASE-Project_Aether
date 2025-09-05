using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupUIController : MonoBehaviour
{
    public static ItemPickupUIController Instance { get; private set; }

    public GameObject popupPrefab;
    public int maxPopup = 3;
    public float popupDuration = 3.0f;

    private Queue<GameObject> activePopups = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowItemPickup(string itemName, Sprite itemIcon)
    {
        // Create new popup
        GameObject newPrefab = Instantiate(popupPrefab, transform);

        // Change text to item name
        newPrefab.GetComponentInChildren<TMPro.TMP_Text>().text = itemName;

        // Change icon to item icon
        Image itemImage = newPrefab.transform.Find("Icon Item")?.GetComponent<Image>();

        if (itemImage != null)
        {
            itemImage.sprite = itemIcon;
        }

        // Add to active popups queue
        activePopups.Enqueue(newPrefab);
        if (activePopups.Count > maxPopup)
        {
            GameObject oldPopup = activePopups.Dequeue();
            Destroy(oldPopup);
        }

        // Destroy popup after duration
        StartCoroutine(FadeOutAndDestroy(newPrefab));
    }

    private IEnumerator FadeOutAndDestroy(GameObject popup)
    {
        // Wait for the popup duration
        yield return new WaitForSeconds(popupDuration);

        // Error handling if popup is destroyed before coroutine ends
        if (popup == null) 
        {
            Debug.LogWarning("Popup destroyed before countdown");
            yield break; 
        }

        // Fade out effect
        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for (float t = 0f; t < 1f; t += Time.deltaTime)
        {
            if (canvasGroup == null) yield break;
            canvasGroup.alpha = 1f - t;
            yield return null;
        }

        // Destroy popup after fully transparent
        Destroy(popup);
        Debug.Log("Popup destroyed");
    }
}
