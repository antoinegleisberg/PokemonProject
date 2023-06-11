using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string _filePath;

    private GameData _gameData;
    private Dictionary<string, List<ISaveable>> _saveableObjects;

    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _saveableObjects = new Dictionary<string, List<ISaveable>>();
        AddSaveableObjects("Gameplay");

        LoadSave();

        SceneEvents.Instance.OnSceneLoaded += OnSceneLoaded;
        SceneEvents.Instance.OnBeforeSceneUnloaded += OnSceneUnloaded;
    }

    private void OnDestroy()
    {
        SceneEvents.Instance.OnSceneLoaded -= OnSceneLoaded;
        SceneEvents.Instance.OnBeforeSceneUnloaded -= OnSceneUnloaded;
    }

    public void NewSave()
    {
        _gameData = new GameData(null);
    }

    public void LoadSave()
    {
        _gameData = FileDataHandler.LoadData(_filePath);

        if (_gameData == null)
        {
            // No save is available at given location, load default save
            NewSave();
        }

        int loadedScenesCount = SceneManager.sceneCount;
        for (int i=0; i < loadedScenesCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                LoadSceneData(scene.name);
            }
        }
    }

    public void SaveGame()
    {
        if (_gameData == null)
        {
            // Create save to store the data to
            NewSave();
        }

        foreach (KeyValuePair<string, List<ISaveable>> kvp in _saveableObjects)
        {
            string sceneName = kvp.Key;
            SaveSceneData(sceneName);
        }

        FileDataHandler.SaveData(_filePath, _gameData);
    }

    private void OnSceneLoaded(SceneDetails sceneDetails)
    {
        string sceneName = sceneDetails.gameObject.name;
        AddSaveableObjects(sceneName);
        LoadSceneData(sceneName);
    }

    private void OnSceneUnloaded(SceneDetails sceneDetails)
    {
        string sceneName = sceneDetails.gameObject.name;
        SaveSceneData(sceneName);
        RemoveSaveableObjects(sceneName);
    }

    private void LoadSceneData(string sceneName)
    {
        foreach (ISaveable saveableObject in _saveableObjects[sceneName])
        {
            saveableObject.LoadData(_gameData);
        }
    }

    private void SaveSceneData(string sceneName)
    {
        foreach (ISaveable saveableObject in _saveableObjects[sceneName])
        {
            saveableObject.SaveData(ref _gameData);
        }
    }

    private void AddSaveableObjects(string sceneName)
    {
        RemoveSaveableObjects(sceneName);
        _saveableObjects.Add(sceneName, FindSaveableObjects(sceneName));
    }
    
    private void RemoveSaveableObjects(string sceneName)
    {
        if (_saveableObjects.ContainsKey(sceneName))
            _saveableObjects.Remove(sceneName);
    }

    private List<ISaveable> FindSaveableObjects(string sceneName)
    {
        List<ISaveable> saveables =
            FindObjectsOfType<MonoBehaviour>()
            .Where(x => x.gameObject.scene.name == sceneName)
            .OfType<ISaveable>()
            .ToList();

        //if (saveables.Count > 0)
        //{
        //    StringBuilder builder = new StringBuilder($"Found {saveables.Count} saveables in scene {sceneName}: ");
        //    foreach (ISaveable saveableObject in saveables)
        //    {
        //        builder.AppendLine(saveableObject.ToString());
        //    }
        //    Debug.Log(builder.ToString());
        //}

        return saveables;
    }
}
