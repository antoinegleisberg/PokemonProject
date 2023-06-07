using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEvents : MonoBehaviour
{
    public static SceneEvents Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    public event Action<SceneDetails> OnSceneLoaded;
    public event Action<SceneDetails> OnCurrentSceneLoaded;
    public event Action<List<SceneDetails>> OnConnectedScenesLoaded;
    public event Action<SceneDetails> OnBeforeSceneUnloaded;

    public void LoadedScene(SceneDetails sceneDetails) => OnSceneLoaded?.Invoke(sceneDetails);
    public void LoadedCurrentScene(SceneDetails sceneDetails) => OnCurrentSceneLoaded?.Invoke(sceneDetails);
    public void LoadedConnectedScenes(List<SceneDetails> sceneDetails) => OnConnectedScenesLoaded?.Invoke(sceneDetails);
    public void UnloadingScene(SceneDetails sceneDetails) => OnBeforeSceneUnloaded?.Invoke(sceneDetails);
}
