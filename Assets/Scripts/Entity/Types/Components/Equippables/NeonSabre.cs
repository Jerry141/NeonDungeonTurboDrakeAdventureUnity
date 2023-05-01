public class NeonSabre : Equippable
{
    public NeonSabre()
    {
        EquipmentType = EquipmentType.Weapon;
        PowerBonus = 4;
    }

    private void OnValidate()
    {
        if (gameObject.transform.parent)
        {
            gameObject.transform.parent.GetComponent<Equipment>().Weapon = this;
        }
    }
}
