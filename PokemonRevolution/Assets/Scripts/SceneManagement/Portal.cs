using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] private DestinationId _destinationPortalId;
    [SerializeField] private Transform _spawnPoint;
    
    private SceneFader sceneFader;

    public Transform SpawnPoint { get => _spawnPoint; }

    private void Start()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void OnPlayerTriggered(PlayerController playerController)
    {
        StartCoroutine(TeleportPlayer(playerController));
    }

    private IEnumerator TeleportPlayer(PlayerController player)
    {
        yield return sceneFader.FadeIn(0.5f);

        Portal destinationPortal = FindObjectsOfType<Portal>().First(x => x != this && x._destinationPortalId == _destinationPortalId);
        player.transform.parent.transform.position = destinationPortal.SpawnPoint.position;

        yield return sceneFader.FadeOut(0.5f);
    }
}


public enum DestinationId { A, B, C, D, E, F }