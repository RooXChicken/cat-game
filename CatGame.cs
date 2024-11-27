class Program
{
    public static void Main(string[] args)
    {
        RenderWindow window = new RenderWindow(640, 360, 1280, 720, "Cat Game");
        Game game = new Game(window);

        while(window.open)
        {
            window.pollEvents();
            game.update();

            window.clear(new Color(67, 67, 67));

            game.render(window);

            window.display();
        }

        window.destroy();
    }
}