using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Button")]
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button ExitButton;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            ContinueButton.interactable = false;
        }
    }

    public void OnContinueClick()
    {
        DisableButton();
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.gameData.currentSceneName);
    }

    public void OnNewGameClick()
    {
        DisableButton();
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    private void DisableButton()
    {
        ContinueButton.interactable = false;
        NewGameButton.interactable = false;
        ExitButton.interactable = false;
    }
}
