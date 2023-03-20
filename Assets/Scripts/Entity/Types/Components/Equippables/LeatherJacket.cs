sealed class LeatherJacket : Equippable
{
    public LeatherJacket()
    {
        EquipmentType = EquipmentType.Armor;
        DefenseBonus = 1;
    }

    private void OnValidate()
    {
        if (gameObject.transform.parent)
        {
            gameObject.transform.parent.GetComponent<Equipment>().Armor = this;
        }
    }
}
