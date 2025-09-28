using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    [Header("Buttons")]
    [SerializeField] private GameObject ResumeButton;
    [SerializeField] private GameObject LoadButton;
    [SerializeField] private GameObject ExitButton;

    public void OnResumeClick()
    {
        DisableButton();
        Time.timeScale = 1f;
        menu.SetActive(false);
    }

    public void OnLoadClick()
    {
        DisableButton();
        Time.timeScale = 1f; 
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.gameData.currentSceneName);
    }

    public void OnExitClick()
    {
        DisableButton();
        Time.timeScale = 1f;
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void DisableButton()
    {
        ResumeButton.GetComponent<Button>().interactable = false;
        LoadButton.GetComponent<Button>().interactable = false;
        ExitButton.GetComponent<Button>().interactable = false;
    }

    private void OnDisable()
    {
        ResumeButton.GetComponent<Button>().interactable = true;
        LoadButton.GetComponent<Button>().interactable = true;
        ExitButton.GetComponent<Button>().interactable = true;
    }
}
