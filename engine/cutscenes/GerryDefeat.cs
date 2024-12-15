public class GerryDefeat : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private Vector2d previousCameraPosition = null;
    private Vector2d cameraCenter = null;

    //camera center before we changed it
    private Vector2d oldCameraCenter = null;

    private double zoom = 1.0;
    private double fadeOut = 0;

    public GerryDefeat() : base()
    {
        duration = 300;

        haltGame = true;
        drawUI = false;
    }

    public override void onTick()
    {
        if(previousCameraPosition != null)
        {
            if(tick > 120)
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
        Game.playCutscene(new GameEnd(game));
        base.onRemove(game);
    }
}