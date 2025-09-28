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
        DataPersistenceManager.instance.LoadGame();

        if (!DataPersistenceManager.instance.HasGameData())
        {
            ContinueButton.interactable = false;
        }
    }

    public void OnContinueClick()
    {
        DisableButton();
        DataPersistenceManager.instance.LoadGame();

        var scene = DataPersistenceManager.instance.gameData.currentSceneName;

        if (scene.Equals("MainMenu"))
        {
            OnNewGameClick();
        }
        else
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }

    public void OnNewGameClick()
    {
        DisableButton();
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("LevelScene");
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
