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

    public BobaDefeat() : base()
    {
        duration = 600;

        haltGame = true;
        drawUI = false;

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

    public override void onRemove()
    {
        //reset zoom
        window.zoom = 1.0;

        //code to move to next cat
        base.onRemove();
    }
}