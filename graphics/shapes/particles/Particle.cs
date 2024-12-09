public class Particle
{
    public Vector2d position;
    public Vector2d velocity;
    public Vector2d size;
    public int id { get; protected set; }
    public Color color = new Color(255, 255, 255, 255);

    public double u = 0;
    public double uS = 1;

    public int lifetime = 0;
    public bool fadeout = false;
    protected bool gravity;

    public bool remove { get; protected set; }

    public Particle(Vector2d _position, Vector2d _size, double _u, double _uS, int _lifetime, bool _fadeout)
    {
        position = _position;
        velocity = new Vector2d(0, 0);
        size = _size;
        u = _u;
        uS = _uS;

        id = 0;
        gravity = true;

        lifetime = _lifetime;
        fadeout = _fadeout;
    }

    public virtual void tick()
    {
        if(--lifetime < 0)
            remove = true;

        if(fadeout && lifetime < 40)
        {
            color.a = (byte)((lifetime/40.0)*255);
            velocity = new Vector2d(0, 0);
            return;
        }

        if(gravity)
            velocity.y += 0.02;

        position += velocity;
        velocity *= 0.93;
    }
}