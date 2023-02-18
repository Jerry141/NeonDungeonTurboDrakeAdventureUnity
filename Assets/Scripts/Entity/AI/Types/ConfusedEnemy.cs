using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Confused enemy will wonder around aimlessly for number of turns, then change AI back
/// Enemy will attack if a player stands on its way
/// </summary>

[RequireComponent(typeof(Actor))]
public class ConfusedEnemy : AI
{
    [SerializeField] private AI previousAI;
    [SerializeField] private int turnsRemaining;

    public AI PreviousAI { get => previousAI; set => previousAI = value; }
    public int TurnsRemaining { get => turnsRemaining; set => turnsRemaining = value; }

    public override void RunAI()
    {
        // revert back to original AI if turnsRamaining is 0 (duration of spell is over).
        if (turnsRemaining <= 0)
        {
            UIManager.instance.AddMessage($"The {gameObject.name} is no longer confused.", "#FF0000");
            GetComponent<Actor>().AI = previousAI;
            GetComponent<Actor>().AI.RunAI();
            Destroy(this);
        }
        else
        {
            // Move to random direction
            Vector2Int direction = Random.Range(0, 8) switch
            {
                0 => new Vector2Int(0, 1), // upper left
                1 => new Vector2Int(0, -1), // up
                2 => new Vector2Int(1, 0), // upper right
                3 => new Vector2Int(-1, 0), // left
                4 => new Vector2Int(1, 1), // right
                5 => new Vector2Int(1, -1), // bottom left
                6 => new Vector2Int(-1, 1), // bottom
                7 => new Vector2Int(-1, -1), // bottom right
                _ => new Vector2Int(0, 0)
            };

            // The enemy will wonder around trying to attack if tile is occupied by Player
            // bumping into the wall will waste the enemy's turn
            Action.BumpAction(GetComponent<Actor>(), direction);
            turnsRemaining--;
        }
    }

    public override AIState SaveState() => new ConfusedState(
        type: "ConfusedEnemy",
        previousAI: previousAI,
        turnsRemaining: turnsRemaining);

    public void LoadState(ConfusedState state)
    {
        if (state.PreviousAI == "HostileEnemy")
        {
            previousAI = GetComponent<HostileEnemy>();
        }
        turnsRemaining = state.TurnsRemaining;
    }
}


[System.Serializable]
public class ConfusedState : AIState
{
    [SerializeField] private string previousAI;
    [SerializeField] private int turnsRemaining;

    public string PreviousAI { get => previousAI; set => previousAI = value; }
    public int TurnsRemaining { get => turnsRemaining; set => turnsRemaining = value; }

    public ConfusedState(string type = "", AI previousAI = null, int turnsRemaining = 0) : base(type)
    {
        this.previousAI = previousAI.GetType().ToString();
        this.turnsRemaining = turnsRemaining;
    }
}
