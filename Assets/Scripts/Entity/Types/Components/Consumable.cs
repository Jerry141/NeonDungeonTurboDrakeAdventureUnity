using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Consumable : MonoBehaviour
{
    // setting up Consumable Type
    public enum ConsumableType
    {
        Healing,
    }

    [SerializeField] private ConsumableType consumableType;
    [SerializeField] private int amount = 0;

    public ConsumableType _ConsumableType { get => consumableType; }
    public int Amount { get => amount; }


    // Activating consumable
    // Actor - entity holding the item
    // Item - item to consume
    public bool Activate(Actor actor, Item item)
    {
        // checking for item type - for each type return applicable func
        switch (consumableType)
        {
            case ConsumableType.Healing:
                return Healing(actor, item);
            default:
                return false;
        }
    }

    // Healing function - adding healing item value to current hp if not full
    private bool Healing(Actor actor, Item item)
    {
        // getting the value of healed HP (current HP + item healing val - maxhp)
        int amountRecovered = actor.GetComponent<Fighter>().Heal(amount);

        // checking if amount recovered value is more than 0 (not full HP)
        if (amountRecovered > 0)
        {
            UIManager.instance.AddMessage($"You consume the {name}, and replenish {amountRecovered} HP!", "#00FF00"); // info on healing to player
            Consume(actor, item); // consuming the item - deleting it from inv and from the game
            return true;
        }
        // If player at full hp - do not heal
        else
        {
            UIManager.instance.AddMessage("Healing on a full health huh?", "808080");
            return false;
        }
    }

    private void Consume(Actor actor, Item item)
    {
        actor.Inventory.Items.Remove(item);
        Destroy(item.gameObject);
    }
}
