public class GameEndFadeout : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private double fadeOut = 0;
    private Text text;

    public GameEndFadeout() : base()
    {
        duration = 9999999;

        haltGame = true;
        drawUI = false;
    }

    public override void onTick()
    {
        fadeOut += 0.0085;

        base.onTick();
    }

    public override void preDraw(RenderWindow _window)
    {
        base.preDraw(window);

        if(window == null)
        {
            window = _window;
            text = new Text(window.renderer, "Programmed by William\nDrawn by William\nIdeas by family\nMusic by Slopee9 (William's friend)\n\nI hope I made your speical day\na bit more special.\n\nLove you - William", RenderWindow.font, Color.WHITE);
            text.position = new Vector2d(200, 200);
            text.renderedText.size = new Vector2d(3, 3);
        }

        window.zoom = 1;
        window.fadeOut = fadeOut;

        window.drawUI(text);
    }

    public override void onRemove(Game game)
    {
        //reset zoom
        window.zoom = 1.0;

        //code to move to next cat
        base.onRemove(game);
    }
}