using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Consumable : MonoBehaviour
{
    public enum ConsumableType
    {
        Healing,
    }

    [SerializeField] private ConsumableType consumableType;
    [SerializeField] private int amount = 0;

    public ConsumableType _ConsumableType { get => consumableType; }
    public int Amount { get => amount; }


    public bool Activate(Actor actor, Item item)
    {
        switch (consumableType)
        {
            case ConsumableType.Healing:
                return Healing(actor, item);
            default:
                return false;
        }
    }

    private bool Healing(Actor actor, Item item)
    {
        int amountRecovered = actor.GetComponent<Fighter>().Heal(amount);

        if (amountRecovered > 0)
        {
            UIManager.instance.AddMessage($"You consume the {name}, and replenish {amountRecovered} HP!", "#00FF00");
            Consume(actor, item);
            return true;
        }
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
