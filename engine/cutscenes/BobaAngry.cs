public class BobaAngry : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private Vector2d previousCameraPosition = null;
    private Vector2d cameraCenter = null;

    //camera center before we changed it
    private Vector2d oldCameraCenter = null;

    private Sprite bobaangry;
    private double zoom = 1.0;
    private SoundEffect bobaAngry;

    public BobaAngry() : base()
    {
        duration = 480;

        haltGame = true;
        drawUI = false;

        bobaAngry = new SoundEffect("assets/sounds/boba_becomeangry.wav");
        bobaangry = new Sprite("assets/sprites/cats/boba/boba_becomeangry.png");
        bobaangry.offset = new Vector2d(-4, -1);
        bobaangry.textureBounds.w = 32;
        bobaangry.textureBounds.x = 0;
    }

    public override void onTick()
    {
        Game.entities[2][0].drawable = bobaangry;

        if(previousCameraPosition != null)
        {
            if(tick < 120)
            {
                cameraCenter = Vector2d.lerp(previousCameraPosition, Game.entities[2][0].getRawPosition() - new Vector2d(320, 180), 0.04);
                zoom = double.Lerp(zoom, 2.0, 0.04);
            }
            else if(tick < 360)
            {
                bobaangry.textureBounds.x = (tick < 240) ? 0 : 32;
            }
            else
            {
                cameraCenter = Vector2d.lerp(previousCameraPosition, oldCameraCenter, 0.04);
                zoom = double.Lerp(zoom, 1.0, 0.04);
            }

            if(tick == 270)
                bobaAngry.play();

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