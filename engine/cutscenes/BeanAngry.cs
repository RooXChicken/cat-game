public class BeanAngry : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private Vector2d previousCameraPosition = null;
    private Vector2d cameraCenter = null;

    //camera center before we changed it
    private Vector2d oldCameraCenter = null;
    private double zoom = 1.0;

    private Vector2d target;

    public BeanAngry() : base()
    {
        duration = 520;

        haltGame = true;
        drawUI = false;
        target = Game.wheel.getRawPosition();
    }

    public override void onTick()
    {
        if(previousCameraPosition != null)
        {
            if(tick < 80)
            {
                cameraCenter = Vector2d.lerp(previousCameraPosition, Game.entities[2][0].getRawPosition() - new Vector2d(320, 180), 0.04);
                zoom = double.Lerp(zoom, 2.0, 0.04);
            }
            else if(tick < 480)
            {
                BeanCat cat = (BeanCat)Game.entities[2][0];
                cat.target = target;
                cat.state = 0;
                cat.dummy = true;
                cat.phaseDuration = 4;

                cat.tick();
                cat.shadow.tick();

                if(cat.getRawPosition().distance(Game.wheel.getRawPosition()) < 10)
                {
                    tick = 480;
                    cat.dummy = false;
                    cat.isWheel = true;
                }
            }
            else if(tick < 520)
            {
                cameraCenter = Vector2d.lerp(previousCameraPosition, oldCameraCenter, 0.06);
                zoom = double.Lerp(zoom, 1.0, 0.06);
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
        Game.wheel.remove = true;

        //code to move to next cat
        base.onRemove(game);
    }
}