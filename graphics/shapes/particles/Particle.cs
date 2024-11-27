public class Particle
{
    public Vector2d position;
    public Vector2d velocity;
    public Vector2d size;
    public Color color = new Color(255, 255, 255, 255);
    public double u = 0;
    public double v = 0;

    public double uS = 1;
    public double vS = 1;

    public int lifetime = 0;
    public bool fadeout = false;

    public bool remove { get; protected set; }

    public Particle(Vector2d _position, Vector2d _size, double _u, double _v, double _uS, double _vS, int _lifetime, bool _fadeout)
    {
        position = _position;
        velocity = new Vector2d(0, 0);
        size = _size;
        u = _u;
        v = _v;

        uS = _uS;
        vS = _vS;

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

        velocity.y += 0.02;

        position += velocity;
        velocity *= 0.93;
    }
}