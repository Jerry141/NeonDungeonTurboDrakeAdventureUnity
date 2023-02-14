using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Consumable;
using static UnityEditor.Progress;

public class Healing : Consumable
{
    [SerializeField] private int amount = 0;

    public int Amount { get => amount; }

    // Activating consumable
    // Actor - entity holding the item
    // Item - item to consume
    public override bool Activate(Actor consumer)
    {
        // getting the value of healed HP (current HP + item healing val - maxhp)
        int amountRecovered = consumer.GetComponent<Fighter>().Heal(amount);

        // checking if amount recovered value is more than 0 (not full HP)
        if (amountRecovered > 0)
        {
            UIManager.instance.AddMessage($"You consume the {name}, and replenish {amountRecovered} HP!", "#00FF00"); // info on healing to player
            Consume(consumer); // consuming the item - deleting it from inv and from the game
            return true;
        }
        // If player at full hp - do not heal
        else
        {
            UIManager.instance.AddMessage("Healing on a full health huh?", "808080");
            return false;
        }
    }
}
