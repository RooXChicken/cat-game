using SDL2;
using static SDL2.SDL;

public class Music
{
    private nint pointer;

    public Music(string path)
    {
        // if(!RenderWindow.audioEnabled)
        //     return;
        pointer = SDL_mixer.Mix_LoadMUS(path);
        if(pointer == IntPtr.Zero)
            Console.WriteLine("Failed to load music file with path: " + path + "! " + SDL_mixer.Mix_GetError());
        
        SDL_mixer.Mix_VolumeMusic(30);
    }

    public void playMusic()
    {
        SDL_mixer.Mix_PlayMusic(pointer, -1);
    }

    public void stopMusic()
    {
        SDL_mixer.Mix_PauseMusic();
    }

    public void destroy()
    {
        SDL_mixer.Mix_FreeMusic(pointer);
    }
}