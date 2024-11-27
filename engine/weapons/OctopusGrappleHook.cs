public class OctopusGrappleHook : Weapon
{
    public OctopusGrappleHook() : base()
    {
        sprite = new Sprite("assets/sprites/projectiles/octopus_grapple.png");
        sprite.offset = new Vector2d(12, 2);
        id = 5;

        name = "Octopus Hook";
        desc = "Throw an octopus to use\nlike a grapple hook!!\n\nWill pull you towards\nwhatever wall it hits.\nFire with ZL!\n\n0 love points\n240 use time";

        useTime = 0;
    }
}