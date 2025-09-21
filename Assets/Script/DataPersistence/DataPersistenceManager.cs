using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called");
        dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded called");

        // Save to cache
    }

    public void NewGame()
    {
        // Create new game data
        gameData = new GameData();
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogError("No game data to save. A new game must be started before data can be saved.");
            return;
        }

        // Pass data to gameData
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // Save to a file using data handler
        dataHandler.Save(gameData);
    }

    public void LoadGame()
    {
        // Load from a file using data handler
        gameData = dataHandler.Load();

        // if no new game data, create new game data
        if (gameData == null)
        {
            Debug.Log("No game data found. Creating new game data.");
            NewGame();
        }

        // Pass loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>
            (
                FindObjectsSortMode.None
            ).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
