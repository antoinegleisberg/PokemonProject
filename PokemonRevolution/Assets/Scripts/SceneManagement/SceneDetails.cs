using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] private List<SceneDetails> connectedScenes;

    public bool IsLoaded { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SwitchSceneCoroutine());
        }
    }

    private IEnumerator SwitchSceneCoroutine()
    {
        Coroutine loadActiveScene = StartCoroutine(LoadScene());
        GameManager.Instance.SetCurrentScene(this);
        yield return loadActiveScene;
        SceneEvents.Instance.LoadedCurrentScene(this);

        StartCoroutine(LoadConnectedScenes());
        StartCoroutine(UnloadPreviousScenes());
    }

    private IEnumerator LoadScene()
    {
        if (!IsLoaded)
        {
            IsLoaded = true;
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            while (!loadingOperation.isDone)
            {
                yield return null;
            }
            SceneEvents.Instance.LoadedScene(this);
        }
    }

    private IEnumerator LoadConnectedScenes()
    {
        List<Coroutine> loadingCoroutines = new List<Coroutine>();
        foreach (SceneDetails connectedScene in connectedScenes)
        {
            loadingCoroutines.Add(StartCoroutine(connectedScene.LoadScene()));
        }
        foreach (Coroutine coroutine in loadingCoroutines)
        {
            yield return coroutine;
        }
        SceneEvents.Instance.LoadedConnectedScenes(connectedScenes);
    }
    
    private IEnumerator UnloadScene()
    {
        if (IsLoaded)
        {
            SceneEvents.Instance.UnloadingScene(this);
            IsLoaded = false;
            yield return SceneManager.UnloadSceneAsync(gameObject.name);
        }
    }

    private IEnumerator UnloadPreviousScenes()
    {
        if (GameManager.Instance.PreviousScene == null)
        {
            yield break;
        }

        SceneDetails previousScene = GameManager.Instance.PreviousScene;
        List<SceneDetails> previouslyLoadedScenes = previousScene.connectedScenes;
        List<Coroutine> unloadingCoroutines = new List<Coroutine>();
        foreach (SceneDetails scene in previouslyLoadedScenes)
        {
            if (scene != this && !connectedScenes.Contains(scene))
            {
                unloadingCoroutines.Add(StartCoroutine(scene.UnloadScene()));
            }
        }
        if (previousScene != this && !connectedScenes.Contains(previousScene))
        {
            unloadingCoroutines.Add(StartCoroutine(previousScene.UnloadScene()));
        }
        foreach (Coroutine coroutine in unloadingCoroutines)
        {
            yield return coroutine;
        }
    }
}
