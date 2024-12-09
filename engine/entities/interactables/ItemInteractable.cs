public class ItemInteractable : Interactable
{
    public UsableItem item;

    public ItemInteractable(Vector2d _position, UsableItem _item) : base(_position, new Tooltip(_position, _item.name, _item.desc)) { item = _item; item.sprite.position = _position; id = 1; }

    public override void interact(Player player)
    {
        base.interact(player);

        if(item.id != 5)
        {
            player.queuedWeapon = player.items.Count;
            player.items.Add(item);
        }
        else
            player.hasGrapple = true;
    }
}