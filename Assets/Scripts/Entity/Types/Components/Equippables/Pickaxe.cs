sealed class Pickaxe : Equippable
{
    public Pickaxe()
    {
        EquipmentType = EquipmentType.Weapon;
        PowerBonus = 6;
    }

    private void OnValidate()
    {
        if (gameObject.transform.parent)
        {
            gameObject.transform.parent.GetComponent<Equipment>().Weapon = this;
        }
    }
}
