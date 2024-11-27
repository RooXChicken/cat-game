public class StorageInteractable : Interactable
{
    public StorageCabinet cabinet;
    public Vector2d offset;

    public StorageInteractable(Vector2d _position, Vector2d _offset, StorageCabinet _cabinet) : base(_position, new Tooltip(_position, "Storage Cabinet", "What's inside?", false, "assets/sprites/gui/cabinet_tooltip.png"))
    { cabinet = _cabinet; offset = _offset; id = 2; }

    public override void interact(Player player)
    {
        base.interact(player);
        cabinet.toSpawn.shadow.render = true;
        cabinet.toSpawn.teleport(cabinet.getRawPosition() + offset);
        cabinet.sprite = new Sprite("assets/sprites/decor/storage_open.png");

        Game.spawnEntity(cabinet.toSpawn);
    }
}