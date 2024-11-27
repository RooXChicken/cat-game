public class TextBox : Entity
{
    private nint renderer;
    private int t = 0;
    private int character = 0;
    private Text textRender;
    public string[] text;
    private string currentText = "";
    private int index = 0;

    private Sprite textBox;
    private Sprite icon;
    private SoundEffect dialogueTick;

    public TextBox(string[] _text, nint _renderer, Font font) : base(new Vector2d(0, 300), 0)
    {
        renderer = _renderer;
        text = _text;
        
        textRender = new Text(renderer, "", font, Color.BLACK);
        textRender.position = new Vector2d(135, 255);

        textBox = new Sprite("assets/sprites/textbox.png");
        textBox.position = new Vector2d(20, 250);
        textBox.size = new Vector2d(4, 4);

        currentText = text[index].Substring(2);
        icon = new Sprite(new Texture(renderer, "assets/sprites/gui/icons/" + text[index].Substring(1, 1) + ".png"));
        icon.size = new Vector2d(4, 4);
        icon.position = new Vector2d(36, 266);
        dialogueTick = new SoundEffect("assets/sounds/dialogue_tick.wav");

        solid = false;
    }

    public override void tick()
    {
        if(Input.isMouseJustPressed(1) || Input.isJoyJustPressed(0, 1) || Input.isJoyJustPressed(1, 1))
        {
            if(character <= currentText.Length)
            {
                character = currentText.Length;
                textRender.text = currentText;
                textRender.renderText(renderer);
            }
            else if(character >= currentText.Length)
            {
                if(index < text.Length-1)
                {
                    character = 0;
                    index++;
                    currentText = text[index].Substring(2);
                    icon.setTexture(new Texture(renderer, "assets/sprites/gui/icons/" + text[index].Substring(1, 1) + ".png"));
                }
                else
                    kill();
            }
        }

        if(++t % 5 == 0)
        {
            if(++character <= currentText.Length)
            {
                textRender.text = currentText.Substring(0, character);
                textRender.renderText(renderer);
            }
        }

        if(character <= currentText.Length && t % 13 == 0)
            dialogueTick.play();
    }

    public override void draw(RenderWindow window, float alpha)
    {
        base.draw(window, alpha);
        window.drawNC(icon);
        window.drawNC(textBox);
        window.drawNC(textRender);
    }
}