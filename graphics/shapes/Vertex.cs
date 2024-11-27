using SDL2;
using static SDL2.SDL;

public class Vertex
{
    public Vector2d position;
    public Color color;
    public Vector2d texCoord;

    public Vertex(Vector2d _position, Vector2d _texCoord, Color _color)
    {
        position = _position;
        texCoord = _texCoord;
        color = _color;
    }

    public SDL_Vertex toSDL()
    {
        SDL_Vertex vertex = new SDL_Vertex();
        vertex.position.x = (float)position.x;
        vertex.position.y = (float)position.y;
        vertex.color.r = color.r;
        vertex.color.g = color.g;
        vertex.color.b = color.b;
        vertex.color.a = color.a;

        vertex.tex_coord.x = (float)texCoord.x;
        vertex.tex_coord.y = (float)texCoord.y;

        return vertex;
    }
}