using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waypoint : MonoBehaviour, IInteractable
{
    public SceneAsset SceneToGo;
    public bool isEnabled;

    public GameObject RequiredItem;
    public int RequiredCandy = 0;

    public bool CanInteract()
    {
        return isEnabled;
    }

    public void Interact()
    {
        if (!isEnabled) return;

        // Check if the player has the required item
        if (RequiredItem != null)
        {
            var equippedItem = HotbarController.EquippedItem ? HotbarController.EquippedItem.GetComponent<Item>() : null;
            var requiredItem = RequiredItem ? RequiredItem.GetComponent<Item>() : null;

            // If player has no item equipped
            if (!equippedItem || !requiredItem)
            {
                Debug.Log("You need a " + requiredItem.itemName + " to use this waypoint.");
                return;
            }

            // If the equipped item is not the required item
            if (equippedItem.id != requiredItem.id)
            {
                Debug.Log("You need a " + requiredItem.itemName + " to use this waypoint.");
                return;
            }
        }

        // Check if the required candy fulfilled
        if (PlayerScript.CollectedCandy < RequiredCandy)
        {
            Debug.Log("You need a total of " + RequiredCandy + " candy to use this waypoint.");
            return;
        }

        // Save to cache
        DataPersistenceManager.instance.SaveGameCache();

        // Load the scene
        if (SceneToGo != null)
        {
            SceneManager.LoadSceneAsync(SceneToGo.name);
        }
        else
        {
            Debug.LogError("SceneToGo is not assigned in the Waypoint.");
        }
    }
}
