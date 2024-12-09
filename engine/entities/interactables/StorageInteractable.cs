public class StorageInteractable : Interactable
{
    public StorageCabinet cabinet;
    public Vector2d offset;

    public StorageInteractable(Vector2d _position, StorageCabinet _cabinet) : base(_position, new Tooltip(_position, "Storage Cabinet", "What's inside?", false, "assets/sprites/gui/cabinet_tooltip.png"))
    { cabinet = _cabinet; id = 2; }

    public override void interact(Player player)
    {
        Entity spawn = Game.spawnManager.getSpawn(player);
        cabinet.sprite = new Sprite("assets/sprites/decor/storage_open.png");
        if(cabinet.flip)
        {
            cabinet.sprite.size.x *= -1;
            cabinet.sprite.offset = new Vector2d(-4, 0);
        }

        if(spawn != null)
        {
            spawn.teleport(cabinet.getRawPosition() + (cabinet.flip ? new Vector2d(-24, 0) : new Vector2d(12, 0)));
            Game.spawnEntity(spawn);
            base.interact(player);
        }
        else
        {
            cabinet.interactable.tooltip.desc = "(It's empty!)";
            cabinet.interactable.tooltip.redrawTooltip();
        }
    }
}