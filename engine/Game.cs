using System.Net.Http.Headers;
using System.Security;
using Microsoft.Win32.SafeHandles;
using SDL2;

public class Game
{
    public static Dictionary<int, List<Entity>> entities = new Dictionary<int, List<Entity>>();
    public static double catHealth = 1.0;
    private static List<Entity> toSpawn = new List<Entity>();
    private static List<Cutscene> activeCutscenes = new List<Cutscene>();
    private static List<Cutscene> toPlay = new List<Cutscene>();
    public static bool debug = false;
    public static Random random = new Random();
    public static SpawnManager spawnManager = new SpawnManager();

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

    private Dictionary<Vector2d, bool> possibleSpawns;

    private int t = 0;

    public Game(RenderWindow window)
    {
        possibleSpawns = new Dictionary<Vector2d, bool>();

        possibleSpawns.Add(new Vector2d(32, 100), false);
        possibleSpawns.Add(new Vector2d(32, 120), false);
        possibleSpawns.Add(new Vector2d(32, 140), false);
        possibleSpawns.Add(new Vector2d(32, 160), false);
        possibleSpawns.Add(new Vector2d(32, 180), false);
        possibleSpawns.Add(new Vector2d(32, 280), false);
        possibleSpawns.Add(new Vector2d(32, 300), false);

        possibleSpawns.Add(new Vector2d(256, 300), true);
        possibleSpawns.Add(new Vector2d(256, 322), true);
        possibleSpawns.Add(new Vector2d(256, 344), true);

        possibleSpawns.Add(new Vector2d(256, 512), true);
        possibleSpawns.Add(new Vector2d(256, 534), true);
        possibleSpawns.Add(new Vector2d(256, 556), true);
        possibleSpawns.Add(new Vector2d(256, 578), true);

        particles = new ParticleArray(window.renderer, "assets/sprites/particles.png");

        tilemap = new Sprite("assets/sprites/room.png");
        bossbarOutline = new Sprite("assets/sprites/gui/bossbarOutline.png");
        bossbarOutline.position = new Vector2d(140, 318);

        bossbarFill = new Sprite("assets/sprites/gui/bossbarFill.png");
        bossbarFill.position = new Vector2d(140, 318);

        timer = new Timer();

        for(int i = 0; i < 32; i++)
            entities.Add(i, new List<Entity>());

        phaseText = new Text(window.renderer, "Scavenge Phase", RenderWindow.font, Color.WHITE);
        spawnEntity(new ParticleRenderEntity(particles));

        spawnEntity(new SolidWall(new Vector2d(0, 16), new Vector2d(30, 1024)));
        spawnEntity(new SolidWall(new Vector2d(274, 16), new Vector2d(30, 372)));
        spawnEntity(new SolidWall(new Vector2d(274, 428), new Vector2d(30, 372)));

        spawnEntity(new SolidWall(new Vector2d(612, 16), new Vector2d(30, 1024)));
        spawnEntity(new SolidWall(new Vector2d(274, 508), new Vector2d(512, 30)));

        spawnEntity(new SolidWall(new Vector2d(0, 0), new Vector2d(1024, 88)));
        spawnEntity(new SolidWall(new Vector2d(0, 620), new Vector2d(1024, 16)));
        
        spawnEntity(new SolidWall(new Vector2d(146, 398), new Vector2d(14, 116), 11));
        spawnEntity(new SolidWall(new Vector2d(30, 398), new Vector2d(14, 256), 11));
        spawnEntity(new SolidWall(new Vector2d(0, 398), new Vector2d(160, 16), 11));

        spawnEntity(new Player(0, new Vector2d(35, 218), 0));
        spawnEntity(new Player(1, new Vector2d(35, 242), 1));

        spawnEntity(new BeanCat(new Vector2d(100, 200)));
        spawnEntity(new CollidableDecor(new Sprite("assets/sprites/decor/wheel.png", new Vector2d(-19, -64)), new Vector2d(106, 86), new Hitbox(new Vector2d(30, 9))));
        spawnEntity(new CollidableDecor("assets/sprites/decor/table.png", new Vector2d(72, 220)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/couch.png", new Vector2d(32, 212)));
        
        spawnEntity(new CollidableDecor("assets/sprites/decor/dining_chair.png", new Vector2d(36, 338)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/dining_table.png", new Vector2d(62, 338)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/dining_chair.png", new Vector2d(102, 338), null, true));

        spawnEntity(new CollidableDecor("assets/sprites/decor/bella_desk.png", new Vector2d(205, 100)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/gavin_desk.png", new Vector2d(248, 153)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/gavin_chair.png", new Vector2d(224, 153)));
        spawnEntity(new CollidableDecor("assets/sprites/decor/tv_set.png", new Vector2d(251, 202)));

        Hitbox bookshelfHitbox = new Hitbox(new Vector2d(89, 30));
        bookshelfHitbox.offset = new Vector2d(0, 28);
        //bookshelfHitbox.setPosition(new Vector2d(0, 0));
        spawnEntity(new CollidableDecor("assets/sprites/decor/bella_bookshelf.png", new Vector2d(308, 68), bookshelfHitbox));
        spawnEntity(new CollidableDecor("assets/sprites/decor/rug.png", new Vector2d(318, 124), new Hitbox(new Vector2d(0, 0)), false, -1));

        ItemPickup _treatPistol = new ItemPickup(Weapon.fromID(2));
        _treatPistol.teleport(new Vector2d(130, 164));
        _treatPistol.onSpawn();
        _treatPistol.shadow.render = true;
        spawnEntity(_treatPistol);

        foreach(KeyValuePair<Vector2d, bool> location in possibleSpawns)
            spawnEntity(new StorageCabinet(location.Key, location.Value));

        tick();
        phaseText.position = new Vector2d(260, 4);
        t = 0;
        
        textBox = new TextBox(new string[] { "%1What a relaxing day. Just me chilling with the cats.\nNothing beats this!", "%2I'm here too you know.", "%3You know what I meant, silly...", "%1...", "%1Boba, what are you doing...?", "%4BOBA!!!", "%_1", "%1We have to do something!", "%2She's just chilling", "%3...", "%1I feel like she's getting into tons of trouble.\nLet's try to find some things to\nhelp calm her down!" }, window.renderer, RenderWindow.font);
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
        bool halt = false;

        foreach(Cutscene _toPlay in toPlay)
            activeCutscenes.Add(_toPlay);

        toPlay.Clear();

        List<Cutscene> toStop = new List<Cutscene>();
        foreach(Cutscene cutscene in activeCutscenes)
        {
            cutscene.onTick();
            if(cutscene.remove) { toStop.Add(cutscene); continue; }
            if(cutscene.haltGame) halt = true;
        }

        foreach(Cutscene _toStop in toStop) { _toStop.onRemove(); activeCutscenes.Remove(_toStop); }

        if(halt)
        {
            Input.update();
            return;
        }

        if(textBox != null && !textBox.remove)
        {
            textBox.tick();
            Input.update();
            //textBox.kill();

            if(textBox.remove)
            {
                ((Player)entities[1][0]).moveFromCouch();
                ((Player)entities[1][1]).moveFromCouch();
            }
            return;
        }

        if(++t < 7200)
            phaseText.text = "Scavenge Phase\n     " + (60-t/120) + "s";
        else if(t < 7440)
            phaseText.text = "Attack Phase!";
        else
            phaseText.text = "";
        
        if(t == 7200)
            ((Cat)Game.entities[2][0]).phaseDuration = 0;

        //disable activeness of all tooltips
        foreach(ItemPickup _interactable in entities[9])
            _interactable.interactable.tooltip.active = false;
        foreach(StorageCabinet _interactable in entities[10])
            _interactable.interactable.tooltip.active = false;

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

        List<Entity> _toSpawn = [.. toSpawn];
        toSpawn.Clear();

        foreach(Entity spawn in _toSpawn)
        {
            spawn.onSpawn();
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

        if(Input.isJustPressed(SDL.SDL_Keycode.SDLK_F3))
            debug = !debug;

        Input.update();
    }

    public void render(RenderWindow window)
    {
        float alpha = (float)(accumulator/physicsTickRate);

        Vector2d centerPoint = new Vector2d(0, 0);
        int alivePlayers = 0;
        if(((LivingEntity)entities[1][0]).health > 0)
        {
            centerPoint += entities[1][0].getBlendPosition(alpha);
            alivePlayers++;
        }
        if(((LivingEntity)entities[1][1]).health > 0)
        {
            centerPoint += entities[1][1].getBlendPosition(alpha);
            alivePlayers++;
        }
        
        if(alivePlayers > 0)
            window.cameraCenter = centerPoint/alivePlayers - new Vector2d(window.gWidth/2.0, window.gHeight/2.0) + entities[1][0].hitbox.size/2;

        bool drawUI = true;
        bool noSmoothing = false;

        foreach(Cutscene cutscene in activeCutscenes)
        {
            cutscene.preDraw(window);
            if(!cutscene.drawUI) drawUI = false;
            if(cutscene.noSmoothing) noSmoothing = true;
        }

        if(noSmoothing)
            alpha = 0;

        window.draw(tilemap);

        List<Entity> drawOrder = new List<Entity>(entities[0]);
        drawOrder.Sort();
        
        foreach(Entity obj in drawOrder)
            obj.draw(window, alpha);

        if(debug)
            foreach(Entity obj in entities[0])
                if(obj.hitbox != null) window.draw(obj.hitbox);

        if(drawUI)
        {
            if(textBox != null && !textBox.remove)
                textBox.draw(window, alpha);
            else
            {
                window.drawNC(bossbarFill);
                window.drawNC(new Rectangle(new Vector2d(146, 324), new Vector2d(348 * (1-catHealth), 28), new Color(61, 140, 64)));
                window.drawNC(bossbarOutline);

                if(t > 0)
                    window.drawNC(phaseText);
            }
        }

        foreach(Cutscene cutscene in activeCutscenes)
            cutscene.postDraw(window);
    }

    public static void spawnEntity(Entity entity) { toSpawn.Add(entity); }
    public static void playCutscene(Cutscene cutscene) { toPlay.Add(cutscene); }
    public static void spawnParticle(Particle particle) { particles.particles.Add(particle); }
}