
public class LuckyBag : UsableItem
{
    public LuckyBag() : base()
    {
        sprite = new Sprite("assets/sprites/items/lucky_bag.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        useSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        id = 8;

        name = "Lucky Bag";
        desc = "I love gambling!\n\nGrants 3 random\npositive items";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        List<Item> toSpawn = Game.spawnManager.getPositive(3, id);

        foreach(Item item in toSpawn)
        {
            ItemPickup pickup = new ItemPickup(item.pickup);
            pickup.teleport(player.getRawPosition() + new Vector2d(Game.random.NextDouble()*32-16,Game.random.NextDouble()*32-16));
            Game.spawnEntity(pickup);
        }

        destroy = true;

        return true;
    }
}