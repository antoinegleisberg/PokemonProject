using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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

    private void OnDestroy()
    {
        
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
        GameManager.Instance.CheckForNPCs();
        GameManager.Instance.CheckForEncounters(playerTransform.position);
    }
}