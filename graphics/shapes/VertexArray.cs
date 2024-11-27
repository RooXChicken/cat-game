using SDL2;
using static SDL2.SDL;

public class VertexArray : Drawable
{
    private Texture texture;
    public List<Vertex> vertices;

    public VertexArray(nint renderer, string path)
    {
        texture = new Texture(renderer, path);
        vertices = new List<Vertex>();
    }

    public override void draw(nint surface, Vector2d cameraCenter)
    {
        base.draw(surface, cameraCenter);
        SDL_Vertex[] array = new SDL_Vertex[vertices.Count];
        for(int i = 0; i < array.Length; i++)
        {
            array[i] = vertices[i].toSDL();
            array[i].position.x -= (float)cameraCenter.x;
            array[i].position.y -= (float)cameraCenter.y;
        }

        if(SDL_RenderGeometry(surface, texture.pointer, array, array.Length, null, 0) < 0)
            Console.WriteLine("Failed to render Vertex array! " + SDL_GetError());
    }
}