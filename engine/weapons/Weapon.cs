using System.Data.Common;

public class Weapon : UsableItem
{
    public int cooldown = 0;
    public int useTime { get; protected set; }
    public double damage { get; protected set; }

    public Weapon() : base ()
    {
        useTime = 0;
        damage = 0;
    }

    public override bool use(Player player, Vector2d direction, Vector2d position) { if(cooldown > 0) return false; cooldown = useTime; return base.use(player, direction, position); }
    public virtual void tick() { if(cooldown > 0) cooldown--; }
}