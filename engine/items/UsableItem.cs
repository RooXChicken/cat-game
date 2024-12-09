using System.Data.Common;

public class UsableItem
{
    public Sprite sprite { get; protected set; }
    
    public int id { get; protected set; }
    public string name { get; protected set; }
    public string desc { get; protected set; }

    public Vector2d offset { get; protected set; }
    protected SoundEffect useSound;

    public bool destroy { get; protected set; }
    public bool autouse { get; protected set; }

    public UsableItem()
    {
        id = 0;
        name = "Item";
        desc = "You should not see this.";
        
        offset = new Vector2d(0, 0);
        destroy = false;
        autouse = false;
    }

    public virtual bool use(Player player, Vector2d direction, Vector2d position) { if(useSound != null) useSound.play(); return true; }

    public static UsableItem fromID(int id)
    {
        switch(id)
        {
            case 0: return new UsableItem();
            case 1: return new CatnipLauncher();
            case 2: return new TreatPistol();
            case 3: return new BottleCapRifle();
            case 4: return new CrinkleBlaster();
            case 5: return new OctopusGrappleHook();
            case 6: return new BobaTea();
            case 7: return new Coffee();
            case 8: return new LuckyBag();
            case 9: return new Chalk();
            case 10: return new BreadLoaf();
            case 11: return new ClothingTag();
            case 12: return new UsableItem();
            case 13: return new Pineapple();
            case 14: return new Cinnamon();
            case 15: return new Mochi();
            case 16: return new BananaBread();
            case 17: return new NewWorld();
            case 18: return new QTipRifle();
            case 19: return new PeanutButterApple();
            case 20: return new Polaroid();
        }

        return new UsableItem();
    }

    public virtual UsableItem clone() { return fromID(id); }
}