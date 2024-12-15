public class GameEnd : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private double fadeOut = 1;

    public GameEnd(Game game) : base()
    {
        duration = 120;

        haltGame = true;
        drawUI = false;

        Game.textBox.reset(new string[] { "%1Have you all had your fun yet?", "%0(collective purring)", "%_2", "%0" } );
        game.loadEntities(3);
    }

    public override void onTick()
    {
        fadeOut -= 0.0085;

        base.onTick();
    }

    public override void preDraw(RenderWindow _window)
    {
        base.preDraw(window);

        if(window == null)
            window = _window;

        window.zoom = 1;
        window.fadeOut = fadeOut;
    }

    public override void onRemove(Game game)
    {
        //reset zoom
        window.zoom = 1.0;

        //code to move to next cat
        base.onRemove(game);
    }
}