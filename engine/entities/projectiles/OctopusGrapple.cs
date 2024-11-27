
class OctopusGrapple : Projectile
{
    private Player player;
    public bool grappled = false;

    public OctopusGrapple(Player _player, Vector2d _position, Vector2d _direction) : base(_position, _direction, 0, new Sprite("assets/sprites/projectiles/octopus_grapple.png"), 0, 4, 8)
    {
        player = _player;
        player.grappleOut = true;
        bullet.rotation = Math.Abs(Math.Atan2(direction.x, direction.y)-3) * 60;
    }

    public override void tick()
    {
        base.tick();

        double distance = getCenter().distanceSquared(player.getCenter());
        if(distance > 196)
            kill();

        if(grappled)
        {
            player.velocity = getCenter().getDirectionBetweenPoints(player.getCenter()).normalize()*4;
            if(distance < 16)
                kill();
        }
    }

    public override bool onHit(Entity entity) { return false; }
    public override void onKill()
    {
        player.grappleOut = false;
    }
    
    private Vector2d getCenter() { return getRawPosition() + new Vector2d(3, 4); }

    public override bool genericCollision(Entity entity)
    {
        if(entity.collision == 4 || entity.collision == 6)
        {
            velocity = new Vector2d(0, 0);
            killOnStop = false;
            grappled = true;
            solid = false;
        }

        return false;
    }

    public override void draw(RenderWindow window, float alpha)
    {
        base.draw(window, alpha);
        window.draw(new Line(getCenter(), player.getCenter(), Color.WHITE));
    }
} 