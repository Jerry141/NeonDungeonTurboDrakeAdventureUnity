using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    [SerializeField] private bool moveKeyHeld;

    private void Awake() => controls = new Controls();

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDisble()
    {
        controls.Player.SetCallbacks(this);
        controls.Player.Disable();
    }

    // checks for player movement input
    void Controls.IPlayerActions.OnMovement(InputAction.CallbackContext context)
    {
        if (context.started)
            moveKeyHeld = true;
        else if (context.canceled)
            moveKeyHeld = false;
    }

    // Checks for escape
    void Controls.IPlayerActions.OnExit(InputAction.CallbackContext context)
    {
        if (context.performed)
            Action.EscapeAction();
    }

    public void OnView(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIManager.instance.ToggleMessageHistory();
        }
    }

    private void FixedUpdate()
    {
        if (!UIManager.instance.IsMessageHistoryOpen)
        {
            if (GameManager.instance.IsPlayerTurn && moveKeyHeld && GetComponent<Actor>().IsAlive)
                MovePlayer();
        }
    }

    // Player movement function
    private void MovePlayer()
    {
        // reading the player position and rounding it up
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x),
            Mathf.Round(direction.y));

        // Setting up future position
        Vector3 futurePosition = transform.position + (Vector3)roundedDirection;

        // if position is valid - move character to the future position
        // based on the rounded direction
        if (IsValidPosition(futurePosition))
            moveKeyHeld = Action.BumpAction(GetComponent<Actor>(), roundedDirection);
    }

    // Checking if the position is valid
    private bool IsValidPosition(Vector3 futurePosition)
    {
        // checking the future position from the map manager based on futurePosition
        Vector3Int gridPosition = MapManager.instance.FloorMap.WorldToCell(futurePosition);

        // Checking if the position that player wants to move is valid
        // check if it is in bounds
        // check if it is an obstacle
        // check if it is current transform position (if player didn't move)
        // If any of the above is true, do not move character
        if (!MapManager.instance.InBounds(gridPosition.x, gridPosition.y) ||
                MapManager.instance.ObstacleMap.HasTile(gridPosition) ||
                futurePosition == transform.position)
            return false;

        return true;
    }
}
