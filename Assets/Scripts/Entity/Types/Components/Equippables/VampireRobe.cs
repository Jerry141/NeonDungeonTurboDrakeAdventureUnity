sealed class VampireRobe : Equippable
{
    public VampireRobe()
    {
        EquipmentType = EquipmentType.Armor;
        DefenseBonus = 5;
    }

    private void OnValidate()
    {
        if (gameObject.transform.parent)
        {
            gameObject.transform.parent.GetComponent<Equipment>().Armor = this;
        }
    }
}
