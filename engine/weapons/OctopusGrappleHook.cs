public class OctopusGrappleHook : Weapon
{
    public Vector2d position;

    public OctopusGrappleHook(Vector2d _position) : base()
    {
        sprite = new Sprite("assets/sprites/projectiles/octopus_grapple.png");
        position = _position;
        sprite.offset = new Vector2d(12, 2);
        sprite.position = position;
        id = 5;

        useTime = 0;
    }
}