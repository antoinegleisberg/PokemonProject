using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISaveable
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Character _character;
    
    [SerializeField] private PokemonPartyManager _playerPartyManager;
    
    private Direction _facing;

    private void Start()
    {
        _facing = Direction.Down;
    }

    private void Update()
    {
        Move();
    }

    public void CheckForInteraction()
    {
        Vector2 interactPos = new Vector2(_playerTransform.position.x, _playerTransform.position.y) + DirectionUtils.ToVec2(_facing);

        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.2f, GameLayers.Instance.InteractableLayer);
        if (collider != null)
        {
            IInteractable interactable = collider.GetComponentInParent<Transform>().GetComponentInChildren<IInteractable>();
            interactable.Interact(_playerTransform);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.PlayerData.PlayerPosition = _playerTransform.position;

        data.PlayerData.PokemonsSaveData = new List<PokemonSaveData>();
        foreach (Pokemon pokemon in _playerPartyManager.PokemonParty.Pokemons)
        {
            data.PlayerData.PokemonsSaveData.Add(pokemon.GetSaveData());
        }
    }

    public void LoadData(GameData data)
    {
        _playerTransform.position = data.PlayerData.PlayerPosition;

        _playerPartyManager.PokemonParty.Pokemons.Clear();
        foreach (PokemonSaveData pokemonSaveData in data.PlayerData.PokemonsSaveData)
        {
            Pokemon pokemon = new Pokemon(pokemonSaveData);
            _playerPartyManager.PokemonParty.Pokemons.Add(pokemon);
        }
    }

    private void Move()
    {
        Vector2Int lastInput = InputManager.Instance.MovementInput;

        if (lastInput != Vector2Int.zero)
            _facing = DirectionUtils.GetDirection(lastInput);

        _character.MoveContinuous(lastInput, InputManager.Instance.IsRunning, OnMoveOver);
    }
    
    private void OnMoveOver()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_playerTransform.position, 0.2f, GameLayers.Instance.TriggerableLayers);

        foreach (Collider2D collider in colliders)
        {
            IPlayerTriggerable triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }
    }
}