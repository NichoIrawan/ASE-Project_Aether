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

    public GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene. Destoying the new one");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    private void Start()
    {
        LoadGame();
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
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
        Debug.Log("OnSceneLoaded called");
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded called");

        // Save to cache
        SaveGameCache();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public void NewGame()
    {
        // Create new game data
        gameData = new GameData();
    }

    public void SaveGameCache()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects.ToList())
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogError("No game data to save. A new game must be started before data can be saved.");
            return;
        }

        // Add Active Scene
        this.gameData.currentSceneName = SceneManager.GetActiveScene().name;

        // Ensure the data is up to date
        SaveGameCache();

        // Save to a file using data handler
        dataHandler.Save(gameData);
    }

    public void LoadGame()
    {
        // Load from a file using data handler
        gameData = dataHandler.Load();

        // if no new game data, Don't load
        if (gameData == null)
        {
            Debug.Log("No game data found. Creating new game data.");
            return;
        }

        // Pass loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects.ToList())
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    // List automatically all class that implement IDataPersistence (Called when scene loaded)
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>
            (
                FindObjectsSortMode.None
            ).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    // Method to manually register object to memory (For dynamic state of scene)
    public void RegisterDataPersistenceObject(IDataPersistence obj)
    {
        if (!dataPersistenceObjects.Contains(obj))
        {
            dataPersistenceObjects.Add(obj);
        }
    }

    public void UnregisterDataPersistenceObject(IDataPersistence obj)
    {
        dataPersistenceObjects.Remove(obj);
    }
}
