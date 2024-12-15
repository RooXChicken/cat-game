public class BobaDefeat : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private Vector2d previousCameraPosition = null;
    private Vector2d cameraCenter = null;

    //camera center before we changed it
    private Vector2d oldCameraCenter = null;

    private Sprite bobameow;
    private double zoom = 1.0;
    private double fadeOut = 0;

    private SoundEffect bobaMeowing;

    public BobaDefeat() : base()
    {
        duration = 600;

        haltGame = true;
        drawUI = false;

        bobaMeowing = new SoundEffect("assets/sounds/cat_meow0.wav");
        bobameow = new Sprite("assets/sprites/cats/boba/boba_meow.png");
        bobameow.offset = new Vector2d(-4, -1);
        bobameow.textureBounds.w = 32;
        bobameow.textureBounds.x = 0;
    }

    public override void onTick()
    {
        Game.entities[2][0].drawable = bobameow;

        if(previousCameraPosition != null)
        {
            if(tick < 120)
            {
                cameraCenter = Vector2d.lerp(previousCameraPosition, Game.entities[2][0].getRawPosition() - new Vector2d(320, 180), 0.04);
                zoom = double.Lerp(zoom, 2.0, 0.04);
            }
            else if(tick < 480)
            {
                if(tick < 300) bobameow.textureBounds.x = 0;
                else if(tick < 360) bobameow.textureBounds.x = 32;
                else bobameow.textureBounds.x = 64;
            }
            else
                fadeOut += 0.0085;

            if(tick == 310)
                bobaMeowing.play();

            previousCameraPosition = cameraCenter;
        }

        base.onTick();
    }

    public override void preDraw(RenderWindow _window)
    {
        base.preDraw(window);
        if(previousCameraPosition == null)
        {
            window = _window;

            previousCameraPosition = window.cameraCenter;
            cameraCenter = window.cameraCenter;
            oldCameraCenter = window.cameraCenter;

        }

        window.zoom = zoom;
        window.cameraCenter = cameraCenter;
    }

    public override void postDraw(RenderWindow window)
    {
        base.postDraw(window);
        window.fadeOut = fadeOut;
        // window.draw(new Rectangle(cameraCenter, new Vector2d(1280, 720), new Color(0, 0, 0, (byte)(Math.Min(1, fadeOut)*255))));
    }

    public override void onRemove(Game game)
    {
        //reset zoom
        window.zoom = 1.0;

        //code to move to next cat
        Game.playCutscene(new BeanSummon(game));
        base.onRemove(game);
    }
}