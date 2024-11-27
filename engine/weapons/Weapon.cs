using System.Data.Common;

public class Weapon
{
    public Sprite sprite { get; protected set; }
    public int id { get; protected set; }
    public int cooldown = 0;
    public int useTime { get; protected set; }
    public double damage { get; protected set; }
    public Vector2d offset { get; protected set; }
    protected SoundEffect fireSound;

    protected Random rand;

    public Weapon()
    {
        useTime = 1;
        damage = 0;
        offset = new Vector2d(0, 0);
        id = 0;

        rand = new Random();
    }

    public virtual bool use(Vector2d direction, Vector2d position) { if(cooldown > 0) return false; cooldown = useTime; if(fireSound != null) fireSound.play(); return true; }
    public virtual void tick() { if(cooldown > 0) cooldown--; }
}