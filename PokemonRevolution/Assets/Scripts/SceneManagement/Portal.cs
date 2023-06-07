using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] private DestinationId destinationPortalId;
    [SerializeField] private Transform spawnPoint;

    private PlayerController player;
    private SceneFader sceneFader;

    public Transform SpawnPoint { get => spawnPoint; }

    private void Start()
    {
        sceneFader = FindObjectOfType<SceneFader>();
    }

    public void OnPlayerTriggered(PlayerController playerController)
    {
        player = playerController;
        StartCoroutine(TeleportPlayer());
    }

    private IEnumerator TeleportPlayer()
    {
        yield return sceneFader.FadeIn(0.5f);

        Portal destinationPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortalId == destinationPortalId);
        player.transform.parent.transform.position = destinationPortal.SpawnPoint.position;

        yield return sceneFader.FadeOut(0.5f);
    }
}


public enum DestinationId { A, B, C, D, E, F }