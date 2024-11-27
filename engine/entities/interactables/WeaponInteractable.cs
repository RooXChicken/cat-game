public class WeaponInteractable : Interactable
{
    public Weapon weapon;

    public WeaponInteractable(Vector2d _position, Weapon _weapon) : base(_position, new Tooltip(_position, _weapon.name, _weapon.desc)) { weapon = _weapon; weapon.sprite.position = _position; id = 1; }

    public override void interact(Player player)
    {
        base.interact(player);

        if(weapon.id != 5)
        {
            player.queuedWeapon = player.weapons.Count;
            player.weapons.Add(weapon);
        }
        else
            player.hasGrapple = true;
    }
}