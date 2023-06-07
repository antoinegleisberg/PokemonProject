using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISaveable
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Character character;
    
    [SerializeField] private PokemonPartyManager playerPartyManager;
    
    private Direction facing;

    public void CheckForInteraction()
    {
        Vector2 interactPos = new Vector2(playerTransform.position.x, playerTransform.position.y) + DirectionUtils.ToVec2(facing);

        Collider2D collider = Physics2D.OverlapCircle(interactPos, 0.2f, GameLayers.Instance.InteractableLayer);
        if (collider != null)
        {
            IInteractable interactable = collider.GetComponentInParent<Transform>().GetComponentInChildren<IInteractable>();
            interactable.Interact(playerTransform);
        }
    }

    private void Start()
    {
        facing = Direction.Down;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2Int lastInput = InputManager.Instance.MovementInput;

        if (lastInput != Vector2Int.zero)
            facing = DirectionUtils.GetDirection(lastInput);

        character.MoveContinuous(lastInput, InputManager.Instance.IsRunning, OnMoveOver);
    }
    
    private void OnMoveOver()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerTransform.position, 0.2f, GameLayers.Instance.TriggerableLayers);

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

    public void SaveData(ref GameData data)
    {
        data.PlayerData.PlayerPosition = playerTransform.position;

        data.PlayerData.PokemonsSaveData = new List<PokemonSaveData>();
        foreach (Pokemon pokemon in playerPartyManager.PokemonParty.Pokemons)
        {
            data.PlayerData.PokemonsSaveData.Add(pokemon.GetSaveData());
        }
    }

    public void LoadData(GameData data)
    {
        playerTransform.position = data.PlayerData.PlayerPosition;

        playerPartyManager.PokemonParty.Pokemons.Clear();
        foreach (PokemonSaveData pokemonSaveData in data.PlayerData.PokemonsSaveData)
        {
            Pokemon pokemon = new Pokemon(pokemonSaveData);
            playerPartyManager.PokemonParty.Pokemons.Add(pokemon);
        }
    }
}