public class BobaRun : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private Vector2d previousCameraPosition = null;
    private Vector2d cameraCenter = null;

    //camera center before we changed it
    private Vector2d lastTracked;
    private Vector2d oldCameraCenter = null;
    private double zoom = 1.0;

    public BobaRun() : base()
    {
        duration = 480;

        haltGame = true;
        drawUI = false;
    }

    public override void onTick()
    {
        Game.entities[2][0].tick();
        ((Cat)Game.entities[2][0]).shadow.tick();

        if(previousCameraPosition != null)
        {
            if(tick < 120)
                lastTracked = Game.entities[2][0].getRawPosition() - new Vector2d(320, 180);
                
            if(tick < 320)
            {
                cameraCenter = Vector2d.lerp(previousCameraPosition, lastTracked, 0.04);
                zoom = double.Lerp(zoom, 1.5, 0.04);
            }
            else
            {
                cameraCenter = Vector2d.lerp(previousCameraPosition, oldCameraCenter, 0.04);
                zoom = double.Lerp(zoom, 1.0, 0.04);
            }

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

    public override void onRemove(Game game)
    {
        //reset zoom
        window.zoom = 1.0;

        //code to move to next cat
        base.onRemove(game);
    }
}