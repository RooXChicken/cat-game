using SDL2;
using static SDL2.SDL;

public class SpriteArray : Drawable
{
    private Texture texture;
    public List<SpriteVertex> sprites;

    public SpriteArray(nint renderer, string path)
    {
        texture = new Texture(renderer, path);
        sprites = new List<SpriteVertex>();
    }

    public override void draw(nint surface, Vector2d cameraCenter)
    {
        base.draw(surface, cameraCenter);

        SDL_Vertex[] array = new SDL_Vertex[sprites.Count*4];
        int[] indecies = new int[sprites.Count*6];
        int index = 0;

        for(int i = 0; i < array.Length; i+=4)
        {
            array[i].position.x = (float)(sprites[i/4].position.x - cameraCenter.x);
            array[i].position.y = (float)(sprites[i/4].position.y - cameraCenter.y);
            array[i].tex_coord.x = 0;
            array[i].tex_coord.y = 0;

            array[i+1].position.x = (float)(sprites[i/4].position.x - cameraCenter.x + sprites[i/4].size.x);
            array[i+1].position.y = (float)(sprites[i/4].position.y - cameraCenter.y);
            array[i+1].tex_coord.x = 1;
            array[i+1].tex_coord.y = 0;

            array[i+2].position.x = (float)(sprites[i/4].position.x - cameraCenter.x);
            array[i+2].position.y = (float)(sprites[i/4].position.y - cameraCenter.y + sprites[i/4].size.y);
            array[i+2].tex_coord.x = 0;
            array[i+2].tex_coord.y = 1;

            array[i+3].position.x = (float)(sprites[i/4].position.x - cameraCenter.x + sprites[i/4].size.x);
            array[i+3].position.y = (float)(sprites[i/4].position.y - cameraCenter.y + sprites[i/4].size.y);
            array[i+3].tex_coord.x = 1;
            array[i+3].tex_coord.y = 1;

            for(int k = 0; k < 4; k++)
            {
                array[i+k].color.r = 255;
                array[i+k].color.g = 255;
                array[i+k].color.b = 255;
                array[i+k].color.a = 255;
            }

            indecies[index] = i;
            indecies[index+1] = i+1;
            indecies[index+2] = i+2;
            indecies[index+3] = i+2;
            indecies[index+4] = i+1;
            indecies[index+5] = i+3;
            index += 6;
        }

        if(SDL_RenderGeometry(surface, texture.pointer, array, array.Length, indecies, indecies.Length) < 0)
            Console.WriteLine("Failed to render Sprite array! " + SDL_GetError());
    }
}