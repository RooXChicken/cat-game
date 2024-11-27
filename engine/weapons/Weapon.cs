using System.Data.Common;

public class Weapon
{
    public Sprite sprite { get; protected set; }
    public int id { get; protected set; }
    public string name { get; protected set; }
    public string desc { get; protected set; }

    public int cooldown = 0;
    public int useTime { get; protected set; }
    public double damage { get; protected set; }
    public Vector2d offset { get; protected set; }
    protected SoundEffect fireSound;

    public bool destroy { get; protected set; }

    public Weapon()
    {
        useTime = 1;
        damage = 0;
        offset = new Vector2d(0, 0);
        id = 0;
        destroy = false;

        name = "Weapon";
        desc = "You should not see this.";
    }

    public virtual bool use(Player player, Vector2d direction, Vector2d position) { if(cooldown > 0) return false; cooldown = useTime; if(fireSound != null) fireSound.play(); return true; }
    public virtual void tick() { if(cooldown > 0) cooldown--; }

    public static Weapon fromID(int id)
    {
        switch(id)
        {
            case 0: return new Weapon();
            case 1: return new CatnipLauncher();
            case 2: return new TreatPistol();
            case 3: return new BottleCapRifle();
            case 4: return new CrinkleBlaster();
            case 5: return new OctopusGrappleHook();
            case 6: return new BobaTea();
        }

        return new Weapon();
    }
}