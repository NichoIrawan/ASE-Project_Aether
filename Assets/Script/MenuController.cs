using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!menu.activeSelf)
            {
                // Pause the game
                Time.timeScale = 0f;

                // Show the menu
                menu.SetActive(true);
            }
            else
            {
                // Hide the menu
                menu.SetActive(false);

                // Resume the game
                Time.timeScale = 1f; 
            }
        }
    }
}
