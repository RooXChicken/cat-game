using SDL2;
using static SDL2.SDL;

public class ParticleArray : Drawable
{
    public static readonly int PARTICLE_COUNT = 5;
    private Texture texture;
    public List<Particle> particles;

    public ParticleArray(nint renderer, string path)
    {
        texture = new Texture(renderer, path);
        particles = new List<Particle>();
    }

    public override void draw(nint surface, Vector2d cameraCenter)
    {
        base.draw(surface, cameraCenter);

        SDL_Vertex[] array = new SDL_Vertex[particles.Count*4];
        int[] indecies = new int[particles.Count*6];
        int index = 0;

        for(int i = 0; i < array.Length; i+=4)
        {
            array[i].position.x = (float)(particles[i/4].position.x - cameraCenter.x);
            array[i].position.y = (float)(particles[i/4].position.y - cameraCenter.y);
            array[i].tex_coord.x = (float)particles[i/4].u;
            array[i].tex_coord.y = (float)(particles[i/4].id * 1.0/PARTICLE_COUNT);

            array[i+1].position.x = (float)(particles[i/4].position.x - cameraCenter.x + particles[i/4].size.x);
            array[i+1].position.y = (float)(particles[i/4].position.y - cameraCenter.y);
            array[i+1].tex_coord.x = (float)particles[i/4].u + (float)particles[i/4].uS;
            array[i+1].tex_coord.y = (float)(particles[i/4].id * 1.0/PARTICLE_COUNT);

            array[i+2].position.x = (float)(particles[i/4].position.x - cameraCenter.x);
            array[i+2].position.y = (float)(particles[i/4].position.y - cameraCenter.y + particles[i/4].size.y);
            array[i+2].tex_coord.x = (float)particles[i/4].u;
            array[i+2].tex_coord.y = (float)(particles[i/4].id * 1.0/PARTICLE_COUNT + 1.0/PARTICLE_COUNT);

            array[i+3].position.x = (float)(particles[i/4].position.x - cameraCenter.x + particles[i/4].size.x);
            array[i+3].position.y = (float)(particles[i/4].position.y - cameraCenter.y + particles[i/4].size.y);
            array[i+3].tex_coord.x = (float)particles[i/4].u + (float)particles[i/4].uS;
            array[i+3].tex_coord.y = (float)(particles[i/4].id * 1.0/PARTICLE_COUNT + 1.0/PARTICLE_COUNT);

            for(int k = 0; k < 4; k++)
            {
                array[i+k].color.r = particles[i/4].color.r;
                array[i+k].color.g = particles[i/4].color.g;
                array[i+k].color.b = particles[i/4].color.b;
                array[i+k].color.a = particles[i/4].color.a;
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
            Console.WriteLine("Failed to render Particle array! " + SDL_GetError());
    }
}