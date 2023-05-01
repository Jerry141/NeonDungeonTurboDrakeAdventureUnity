using UnityEngine;

public class LifeSteal : Consumable
{
    [SerializeField] private int damage = 10;
    [SerializeField] private int maximumRange = 5;
    [SerializeField] private int healingPotential = 10;

    public int Damage { get => damage; }
    public int MaximumRange { get => maximumRange; }
    public int HealingPotential { get => healingPotential; }

    public override bool Activate(Actor consumer)
    {
        consumer.GetComponent<Inventory>().SelectedConsumable = this;
        consumer.GetComponent<Player>().ToggleTargetMode();

        UIManager.instance.AddMessage("Select a target to strike", "#63FFFF");

        return false;
    }

    public override bool Cast(Actor consumer, Actor target)
    {
        UIManager.instance.AddMessage(
            $"A stream of blood emerges from {target.name} damaging it for {damage} damage and healing Player for {healingPotential} HP!",
            "#FFFFFF");
        target.GetComponent<Fighter>().Hp -= damage;
        consumer.GetComponent<Fighter>().Hp += healingPotential;

        Consume(consumer);
        consumer.GetComponent<Player>().ToggleTargetMode();

        return true;
    }
}
