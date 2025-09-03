using UnityEngine;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        // Combine directory path and file name to get full file path
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // Read the JSON string from the file
                // Use 'using' statements to ensure the file is properly closed after reading
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Convert the JSON string to a GameData object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading data from file: " + fullPath + "\n" + e);
            }
        } 
        else
        {
            Debug.LogWarning("No data file found at: " + fullPath);
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        // Combine directory path and file name to get full file path
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            // Create directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Convert the game data to JSON format and format with indentation
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write the JSON string to the file
            // Use 'using' statements to ensure the file is properly closed after writing
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving data to file: " + fullPath + "\n" + e);
        }
    }
}
