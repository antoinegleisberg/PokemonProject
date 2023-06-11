using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class FileDataHandler
{
    private static readonly string _dataDirectoryPath = Application.persistentDataPath;

    public static void SaveData(string filePath, GameData data)
    {
        string fullPath = Path.Combine(_dataDirectoryPath, filePath);
        
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
            Debug.Log($"Save data to {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error when saving data to file {fullPath}: {e.Message}");
        }
    }

    public static GameData LoadData(string filePath)
    {
        string fullPath = Path.Combine(_dataDirectoryPath, filePath);

        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                Debug.Log($"Read save data from {fullPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error when reading save from file {fullPath}: {e.Message}");
            }
        }
        return loadedData;
    }
}
