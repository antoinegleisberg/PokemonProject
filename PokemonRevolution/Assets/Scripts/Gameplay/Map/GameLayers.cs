using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    public static GameLayers Instance { get; private set; }

    [SerializeField] private LayerMask _solidObjectsCollidersLayer;
    [SerializeField] private LayerMask _tallGrassLayer;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private LayerMask _fovLayer;
    [SerializeField] private LayerMask _portalLayer;

    public LayerMask SolidObjectsCollidersLayer { get => _solidObjectsCollidersLayer; }
    public LayerMask TallGrassLayer { get => _tallGrassLayer; }
    public LayerMask InteractableLayer { get => _interactableLayer; }
    public LayerMask FovLayer { get => _fovLayer; }
    public LayerMask PortalLayer { get => _portalLayer; }

    public LayerMask TriggerableLayers {
        get => _tallGrassLayer | _fovLayer | _portalLayer;
    }

    private void Awake() => Instance = this;
}
