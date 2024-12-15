public class BeanDefeat : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private Vector2d previousCameraPosition = null;
    private Vector2d cameraCenter = null;

    //camera center before we changed it
    private Vector2d oldCameraCenter = null;

    private Sprite beanWheel;
    private double zoom = 1.0;
    private double fadeOut = 0;

    private SoundEffect bobaMeowing;

    public BeanDefeat() : base()
    {
        duration = 400;

        haltGame = true;
        drawUI = false;

        beanWheel = new Sprite("assets/sprites/cats/bean/bean_standinwheel.png");
    }

    public override void onTick()
    {
        Game.entities[2][0].drawable = beanWheel;

        if(previousCameraPosition != null)
        {
            if(tick > 240)
                fadeOut += 0.0085;

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
        Game.playCutscene(new GerrySummon(game));
        base.onRemove(game);
    }
}