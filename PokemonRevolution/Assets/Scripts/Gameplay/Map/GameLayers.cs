using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    public static GameLayers Instance { get; private set; }

    [SerializeField] private LayerMask solidObjectsCollidersLayer;
    [SerializeField] private LayerMask tallGrassLayer;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask fovLayer;
    [SerializeField] private LayerMask portalLayer;

    public LayerMask SolidObjectsCollidersLayer { get => solidObjectsCollidersLayer; }
    public LayerMask TallGrassLayer { get => tallGrassLayer; }
    public LayerMask InteractableLayer { get => interactableLayer; }
    public LayerMask FovLayer { get => fovLayer; }
    public LayerMask PortalLayer { get => portalLayer; }

    public LayerMask TriggerableLayers {
        get => tallGrassLayer | fovLayer | portalLayer;
    }

    private void Awake() => Instance = this;
}
