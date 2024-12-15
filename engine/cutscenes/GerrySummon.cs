public class GerrySummon : Cutscene
{
    //used ONLY to set zoom on removal!
    private RenderWindow window = null;

    private double fadeOut = 1;

    public GerrySummon(Game game) : base()
    {
        duration = 120;

        haltGame = true;
        drawUI = false;

        Game.textBox.reset(new string[] { "%1These cats have been a mess today! Bean caused\nso much damage with her wheel,\nI can't believe it!", "%2Are we having some kind of bad cat day?", "%1It seems so. Is Gerry going to be a\ngood boy at least?", "%6(evil voice) meow", "%4...what", "%_1", "%1Ugh, not you too Gerry!\nWe have to calm him down too." } );

        List<UsableItem> player1Items = ((Player)Game.entities[1][0]).items;
        List<UsableItem> player2Items = ((Player)Game.entities[1][1]).items;

        game.loadEntities(2);
        game.t = 5400;
        Game.spawnManager.refill();

        ((Player)Game.entities[1][0]).items = player1Items;
        ((Player)Game.entities[1][1]).items = player2Items;
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