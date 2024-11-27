using System.Net.Http.Headers;
using System.Security;
using SDL2;

public class Game
{
    public static Dictionary<int, List<Entity>> entities = new Dictionary<int, List<Entity>>();
    public static double catHealth = 1.0;
    private static List<Entity> toSpawn = new List<Entity>();

    public static double GAME_SPEED = 1.0;
    private double physicsTickRate = 1/120.0;
    private double accumulator = 0.0;
    private Timer timer;

    private Sprite tilemap;
    private Sprite bossbarOutline;
    private Sprite bossbarFill;
    private static ParticleArray particles;

    private TextBox textBox;
    private Text phaseText;

    private int t = 0;

    public Game(RenderWindow window)
    {
        particles = new ParticleArray(window.renderer, "assets/sprites/particles.png");

        // crosshair = new Sprite("assets/sprites/crosshair.png");
        // crosshair.offset = new Vector2d(-crosshair.textureBounds.w/2.0, -crosshair.textureBounds.h/2.0);
        tilemap = new Sprite("assets/sprites/tiles/untitled.png");
        bossbarOutline = new Sprite("assets/sprites/gui/bossbarOutline.png");
        bossbarOutline.position = new Vector2d(140, 318);

        bossbarFill = new Sprite("assets/sprites/gui/bossbarFill.png");
        bossbarFill.position = new Vector2d(140, 318);

        timer = new Timer();

        for(int i = 0; i < 64; i++)
            entities.Add(i, new List<Entity>());

        phaseText = new Text(window.renderer, "Scavenge Phase", RenderWindow.font, Color.WHITE);
        spawnEntity(new ParticleRenderEntity(particles));

        spawnEntity(new Player(0, new Vector2d(35, 150)));
        //spawnEntity(new Player(1, new Vector2d(200, 400)));
        spawnEntity(new Cat(new Vector2d(100, 200)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/table.png", new Vector2d(96, 150)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/couch.png", new Vector2d(32, 150)));
        
        spawnEntity(new CollidableDecor("assets/sprites/decor/dining_chair.png", new Vector2d(36, 352)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/dining_table.png", new Vector2d(62, 352)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/dining_chair.png", new Vector2d(102, 352), true));

        Random rand = new Random();

        spawnEntity(new StorageCabinet(new Vector2d(32, 220), new WeaponPickup(new Vector2d(44, 220), Weapon.fromID(rand.Next(1, 6)))));
        spawnEntity(new StorageCabinet(new Vector2d(32, 240), new WeaponPickup(new Vector2d(44, 240), Weapon.fromID(rand.Next(1, 6)))));
        spawnEntity(new StorageCabinet(new Vector2d(32, 260), new WeaponPickup(new Vector2d(44, 260), Weapon.fromID(rand.Next(1, 6)))));
        spawnEntity(new StorageCabinet(new Vector2d(32, 280), new WeaponPickup(new Vector2d(44, 280), Weapon.fromID(rand.Next(1, 6)))));
        spawnEntity(new StorageCabinet(new Vector2d(32, 300), new WeaponPickup(new Vector2d(44, 300), Weapon.fromID(rand.Next(1, 6)))));
        // spawnEntity(new CollidableDecor("assets/sprites/decor/storage.png", new Vector2d(32, 214)));
        // spawnEntity(new CollidableDecor("assets/sprites/decor/storage.png", new Vector2d(32, 242)));
        //spawnEntity(new StorageCabinet("assets/sprites/decor/storage.png", new Vector2d(32, 214), ));

        spawnEntity(new SolidWall(new Vector2d(0, 16), new Vector2d(30, 1024)));
        spawnEntity(new SolidWall(new Vector2d(274, 16), new Vector2d(30, 1024)));

        spawnEntity(new SolidWall(new Vector2d(0, 0), new Vector2d(1024, 88)));
        spawnEntity(new SolidWall(new Vector2d(0, 624), new Vector2d(1024, 16)));

        spawnEntity(new Item(new Vector2d(100, 300), new CatnipLauncher(new Vector2d(100, 300)),    "Catnip Cannon",  "Blasts catnip into the air!\n\nIf the cat touches the\ncatnip, it becomes stronger\nand faster, but is quicker\nto tame.\n\n0 love points\n120 use time"));
        spawnEntity(new Item(new Vector2d(100, 330), new TreatPistol(new Vector2d(100, 330)),       "Treat Pistol",      "Shoots treats onto\nthe floor!\n\n2 love points\n40 use time"));
        spawnEntity(new Item(new Vector2d(100, 360), new BotteCapRifle(new Vector2d(100, 360)),     "Bottle Cap Rifle","Shoots bottle caps onto\nthe floor!\n\nBottle caps bounce off\nthemselves and walls.\n\n1 love point\n30 use time"));
        spawnEntity(new Item(new Vector2d(100, 390), new CrinkleBlaster(new Vector2d(100, 390)),    "Crinkle Blaster","Shoots tiny crinkle\nballs!\n\nHas a charge time\nand cooldown.\n\n0.5 love point\n9 use time"));
        spawnEntity(new Item(new Vector2d(100, 420), new OctopusGrappleHook(new Vector2d(100, 420)),"Octopus Hook",   "Throw an octopus to use\nlike a grapple hook!!\n\nWill pull you towards\nwhatever wall it hits.\nFire with ZL!\n\n0 love point\n240 use time"));

        tick();
        phaseText.position = new Vector2d(260, 4);
        
        textBox = new TextBox(new string[] { "%1What a relaxing day. Just me chilling with the cats.\nNothing beats this!", "%2I'm here too you know.", "%3You know what I meant, silly...", "%1...", "%1Boba, what are you doing...?", "%4BOBA!!!" }, window.renderer, RenderWindow.font);
    }

    public void update()
    {
        accumulator += timer.getTime()/1000.0 * GAME_SPEED;
        timer.restart();

        while(accumulator >= physicsTickRate)
        {
            tick();
            accumulator -= physicsTickRate;
        }
    }

    public void tick()
    {
        if(textBox != null && !textBox.remove)
        {
            textBox.tick();
            Input.update();
            return;
        }

        if(++t < 3600)
            phaseText.text = "Scavenge Phase\n     " + (30-t/120) + "s";
        else if(t < 3840)
            phaseText.text = "Attack Phase!";
        else
            phaseText.text = "";
        
        if(t == 3600)
            ((Cat)Game.entities[2][0]).phaseDuration = 0;

        List<Entity> removedEntities = new List<Entity>();
        foreach(Entity entity in entities[0])
        {
            entity.tick();
            if(entity.remove)
                removedEntities.Add(entity);
        }

        foreach(Entity entity in removedEntities)
        {
            if(entity.collision != 0)
                entities[entity.collision].Remove(entity);

            entities[0].Remove(entity);
        }

        foreach(Entity spawn in toSpawn)
        {
            if(spawn.collision != 0)
            entities[spawn.collision].Add(spawn);

            entities[0].Add(spawn);
        }

        List<Particle> removedParticles = new List<Particle>();

        foreach(Particle particle in particles.particles)
        {
            particle.tick();
            if(particle.remove)
                removedParticles.Add(particle);
        }

        foreach(Particle particle in removedParticles)
            particles.particles.Remove(particle);

        toSpawn.Clear();
        Input.update();
    }

    public void render(RenderWindow window)
    {
        float alpha = (float)(accumulator/physicsTickRate);
        window.cameraCenter = (entities[1][0].getBlendPosition(alpha)*2)/2 - new Vector2d(window.gWidth/2.0, window.gHeight/2.0) + entities[1][0].hitbox.size/2;

        window.draw(tilemap);

        List<Entity> drawOrder = new List<Entity>(entities[0]);
        drawOrder.Sort();
        
        foreach(Entity obj in drawOrder)
            obj.draw(window, alpha);

        window.drawNC(bossbarFill);
        window.drawNC(new Rectangle(new Vector2d(146, 316), new Vector2d(348 * (1-catHealth), 28), new Color(61, 140, 64)));
        window.drawNC(bossbarOutline);

        phaseText.renderText(window.renderer);
        window.drawNC(phaseText);

        if(textBox != null && !textBox.remove)
            textBox.draw(window, alpha);
        
        // double rJoyValue = Math.Abs(Input.getJoyAxis(0, 1).x) + Math.Abs(Input.getJoyAxis(0, 1).y);
        // if(rJoyValue < 0.2)
        //     crosshair.color.a = 128;
        // else
        //     crosshair.color.a = 255;

        // crosshairWorldPos = (Vector2d)((rJoyValue != 0 ? (Input.getJoyAxis(0, 1)*128) : (Input.getJoyAxis(0, 0)*64)) + new Vector2d(window.rWidth, window.rHeight)/2) / (window.relativeSize) + window.cameraCenter;

        // crosshair.position = crosshairWorldPos;
        // window.draw(crosshair);
    }

    public static void spawnEntity(Entity entity) { toSpawn.Add(entity); }
    public static void spawnParticle(Particle particle) { particles.particles.Add(particle); }
}