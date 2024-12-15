public class BeanSummon : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private double fadeOut = 1;

    public BeanSummon(Game game) : base()
    {
        duration = 120;

        haltGame = true;
        drawUI = false;

        Game.textBox.reset(new string[] { "%1Phew, I'm glad she's calmed down! The mess she\nmade took a while to clean though.", "%1Aww, looks like someone else wants some love!", "%5meow", "%1Beanie?", "%_1", "%1Not again! Do you know what's wrong\nwith them today?", "%2I don't speak cat-ese", "%1Come on, let's calm her down too." } );

        List<UsableItem> player1Items = ((Player)Game.entities[1][0]).items;
        List<UsableItem> player2Items = ((Player)Game.entities[1][1]).items;

        game.loadEntities(1);
        game.t = 3600;

        ((Player)Game.entities[1][0]).items = player1Items;
        ((Player)Game.entities[1][1]).items = player2Items;

        Game.spawnManager.refill();
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