using SDL2;

public class Timer
{
    private uint startTime;

    public Timer() { startTime = SDL.SDL_GetTicks(); }

    public uint getTime() { return SDL.SDL_GetTicks() - startTime; }
    public void restart() { startTime = SDL.SDL_GetTicks(); }
}