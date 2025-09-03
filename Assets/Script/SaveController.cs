using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveFilePath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void SaveGame()
    {
        GameData savedData = new GameData
        {
            position = GameObject.FindGameObjectWithTag("Player").transform.position,
            //sceneId = GameObject.
        };
    }
}
