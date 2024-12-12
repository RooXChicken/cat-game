
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;

public class Player : LivingEntity
{
    public int type { get; private set; }
    private int bg = -1;
    private Sprite sit;
    private Sprite down;
    private Sprite idle;
    private Sprite idleArms;
    private Sprite walk;
    private Sprite walkArms;

    private Sprite shootUArms;
    private Sprite shootU2Arms;
    private Sprite shootMArms;
    private Sprite shootM2Arms;
    private Sprite shootDArms;
    private Sprite shootD2Arms;

    private Sprite arms;
    private Sprite arms2;

    private Sprite healthBar;
    private Sprite crosshair;

    public List<UsableItem> items;
    private int selectedWeapon = 999;
    public int queuedWeapon = 0;

    private double moveSpeed = 0.12;

    private double anim = 0;
    private bool direction = true;

    private int damageCooldown = 0;
    private SoundEffect damageSound;
    public bool hasGrapple = false;

    private OctopusGrapple currentGrapple;
    public bool grappleOut = false;
    private int grappleCooldown = 0;
    private Vector2d lastCrosshairPosition;
    private int oldScroll = 0;

    private int weaponHeldTimer = 0;
    private bool justSwitched = false;

    public Player(int _bg, Vector2d _position, int _type) : base(_position, 1, null)
    {
        bg = _bg;
        type = _type;
        //if(bg == 0)
            //Input.registerController(bg);
        bg *= -1;
        
        hitbox = new Hitbox(new Vector2d(12,24));
        hitbox.offset = new Vector2d(6, 0);
        hitbox.setPosition(_position);
        
        crosshair = new Sprite("assets/sprites/crosshair.png");
        healthBar = new Sprite("assets/sprites/player/healthbar.png");
        healthBar.offset = new Vector2d(2, 25);
        healthBar.color = new Color(255, 255, 255, 192);
        crosshair.color = new Color(158, 163, 245);
        crosshair.offset = new Vector2d(-3.5, -3.5);

        string filename = type == 0 ? "bella" : "gavin";
        idle = new Sprite("assets/sprites/player/" + filename + "idle.png");
        sit = new Sprite("assets/sprites/player/" + filename + "sit.png");
        down = new Sprite("assets/sprites/player/" + filename + "down.png");
        idleArms = new Sprite("assets/sprites/player/" + filename + "idlearms.png");
        walk = new Sprite("assets/sprites/player/" + filename + "walk.png");
        walkArms = new Sprite("assets/sprites/player/" + filename + "walkarms.png");

        shootMArms = new Sprite("assets/sprites/player/" + filename + "shootm.png");
        shootM2Arms = new Sprite("assets/sprites/player/" + filename + "shootm2.png");
        shootUArms = new Sprite("assets/sprites/player/" + filename + "shootu.png");
        shootU2Arms = new Sprite("assets/sprites/player/" + filename + "shootu2.png");
        shootDArms = new Sprite("assets/sprites/player/" + filename + "shootd.png");
        shootD2Arms = new Sprite("assets/sprites/player/" + filename + "shootd2.png");

        // if(bg == 1)
        //     bg = -1;
        
        arms = idleArms;
        arms2 = new Sprite("assets/empty.png");
        damageSound = new SoundEffect("assets/sounds/player_damage.wav");
        items = new List<UsableItem>();

        idle.offset = new Vector2d(0, -8);
        sit.offset = new Vector2d(0, -8);
        down.offset = new Vector2d(0, -8);
        idleArms.offset = new Vector2d(0, -8);
        walk.offset = new Vector2d(0, -8);
        walkArms.offset = new Vector2d(0, -8);

        shootMArms.offset = new Vector2d(0, -8);
        shootM2Arms.offset = new Vector2d(0, -8);
        shootUArms.offset = new Vector2d(0, -8);
        shootU2Arms.offset = new Vector2d(0, -8);
        shootDArms.offset = new Vector2d(0, -8);
        shootD2Arms.offset = new Vector2d(0, -8);

        walk.textureBounds.w = 24;
        walkArms.textureBounds.w = 24;
        
        drawable = sit;

        maxHealth = 20;
        health = maxHealth;
        damageable = true;

        collision = 1;
        drawOrder = 3;
        solid = true;
        pushable = false;
        pushForce = 2;

        ignored.Add(1);
        ignored.Add(2);
        ignored.Add(3);
        ignored.Add(5);
        ignored.Add(8);
        lastCrosshairPosition = getCenter();

        PlayerShadow shadow = new PlayerShadow(this);
        shadow.render = true;
        Game.spawnEntity(shadow);
    }

    public void moveFromCouch() { teleport(getRawPosition() + new Vector2d(14, 0)); }

    public override void tick()
    {
        base.tick();

        if(health <= 0)
        {
            health = 0;
            down.size.x = ((Sprite)drawable).size.x;
            drawable = down;
            velocity = new Vector2d(0, 0);
            arms = null;
            teleport(getRawPosition());
            foreach(UsableItem item in items)
            {
                item.sprite.rotation = 0;
                item.sprite.size.x = Math.Abs(item.sprite.size.x);
                item.sprite.size.y = Math.Abs(item.sprite.size.y);

                ItemPickup pickup = new ItemPickup(item);
                pickup.teleport(getRawPosition());
                pickup.velocity = new Vector2d(Game.random.NextDouble()*6-3, Game.random.NextDouble()*6-3);

                Game.spawnEntity(pickup);
            }
            
            items.Clear();
            return;
        }

        if(bg == -1)
        {
            queuedWeapon -= oldScroll - (int)Input.getScroll();
            oldScroll = (int)Input.getScroll();

        }
        else
        {
            if(Input.isJoyJustPressed(bg, SDL2.SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSHOULDER))
                queuedWeapon--;
            if(Input.isJoyJustPressed(bg, SDL2.SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSHOULDER))
                queuedWeapon++;
        }

        
        bool weaponRemoved = (selectedWeapon < items.Count && items[selectedWeapon].destroy);
        if(weaponRemoved)
            items.Remove(items[selectedWeapon]);

        if(queuedWeapon >= items.Count)
            queuedWeapon = 0;
        
        if(queuedWeapon < 0)
            queuedWeapon = items.Count-1;

        if(selectedWeapon < items.Count && (!(items[selectedWeapon] is Weapon) || ((Weapon)items[selectedWeapon]).cooldown <= 0))
        {
            selectedWeapon = queuedWeapon;
            weaponHeldTimer = 0;
        }

        if(selectedWeapon >= items.Count)
            selectedWeapon = 0;

        if(selectedWeapon < items.Count && weaponRemoved)
        {
            justSwitched = true;
            weaponHeldTimer = 0;
        }

        foreach(UsableItem item in items)
            if(item is Weapon) ((Weapon)item).tick();

        // if(Input.isPressed(SDL2.SDL.SDL_Keycode.SDLK_LSHIFT))
        //     Game.GAME_SPEED = 0.1;
        // else
        //     Game.GAME_SPEED = 1.0;

        if(crosshair.position.distanceSquared(getCenter()) > 8)
            lastCrosshairPosition = crosshair.position;

        Vector2d crosshairDirection = lastCrosshairPosition.getDirectionBetweenPoints(getCenter());
        double shootAngle = Math.Abs(Math.Atan2(crosshairDirection.x, crosshairDirection.y)-3) * 60 - 90;

        // foreach(UsableItem item in items)
        //     if(item.autouse) item.use(this, crosshairDirection, getCenter()+crosshairDirection*3);

        if(selectedWeapon < items.Count)
        {
            items[selectedWeapon].sprite.rotation = shootAngle;

            if(((int)items[selectedWeapon].sprite.rotation+90)%360 < 180)
                items[selectedWeapon].sprite.size.y = 1;
            else
                items[selectedWeapon].sprite.size.y = -1;

            bool useWeapon = false;
            if(bg == -1)
            {
                if(Input.isMousePressed(1))
                    useWeapon = true;
                else
                    justSwitched = false;
            }
            else if(Input.getJoyAxis(bg, 3).x > 0.1)
                useWeapon = true;
            else
                justSwitched = false;

            if(useWeapon && (!justSwitched || weaponHeldTimer > 20))
                items[selectedWeapon].use(this, crosshairDirection, getCenter()+crosshairDirection*3);
        }

        if(hasGrapple)
        {
            bool useGrapple = false;
            if(bg == -1)
            {
                if(Input.isJustPressed(SDL2.SDL.SDL_Keycode.SDLK_LSHIFT))
                    useGrapple = true;
            }
            else if(Input.getJoyAxis(bg, 2).x > 0)
                useGrapple = true;

            if(useGrapple)
            {
                if(grappleCooldown <= 0 && (currentGrapple == null || currentGrapple.remove))
                {
                    currentGrapple = new OctopusGrapple(this, getCenter(), crosshairDirection);
                    Game.spawnEntity(currentGrapple);
                    grappleCooldown = 240;
                }
                else if(currentGrapple.grappled)
                    currentGrapple.kill();
            }

            if(grappleCooldown > 0)
                grappleCooldown--;
        }

        Vector2d moveVector = new Vector2d(0, 0);

        if(bg == -1)
        {
            if(Input.isPressed(SDL2.SDL.SDL_Keycode.SDLK_d))
                moveVector.x += moveSpeed;
            if(Input.isPressed(SDL2.SDL.SDL_Keycode.SDLK_a))
                moveVector.x -= moveSpeed;
            if(Input.isPressed(SDL2.SDL.SDL_Keycode.SDLK_s))
                moveVector.y += moveSpeed;
            if(Input.isPressed(SDL2.SDL.SDL_Keycode.SDLK_w))
                moveVector.y -= moveSpeed;

            if(moveVector.x != 0 && moveVector.y != 0)
                moveVector = moveVector.normalize() * moveSpeed;
        }
        else
            moveVector += Input.getJoyAxis(bg, 0) * moveSpeed;

        if(hasEffect(1))
            moveVector *= 1.3;

        if(hasEffect(2) && !grappleOut)
        {
            velocity = moveVector*10;
            moveVector = new Vector2d(0, 0);
        }

        if(hasEffect(7))
            moveVector *= 0.2;
        //moveVector = moveVector.normalize();

        velocity += moveVector;

        if(Math.Abs(moveVector.x) + Math.Abs(moveVector.y) < 0.05 && Math.Abs(velocity.x) + Math.Abs(velocity.y) < 0.05)
        {
            drawable = idle;
            arms = idleArms;
        }
        else
        {
            drawable = walk;
            arms = walkArms;
        }

        if(selectedWeapon < items.Count)
        {
            if((shootAngle > -91 && shootAngle < -20) || (shootAngle > 200 && shootAngle < 281))
            {
                arms = shootUArms;
                arms2 = shootU2Arms;
            }
            else if(shootAngle > 20 && shootAngle < 170)
            {
                arms = shootDArms;
                arms2 = shootD2Arms;
            }
            else
            {
                arms = shootMArms;
                arms2 = shootM2Arms;
            }
        }

        if(hasEffect(6))
            Game.spawnParticle(new CinnamonParticle(getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), true));

        if(hasEffect(7))
            Game.spawnParticle(new CinnamonParticle(getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), false));

        basicCollision();

        Interactable closest = null;
        double closestDistance = Double.MaxValue;
        foreach(ItemPickup item in Game.entities[9])
        {
            double _distance = item.getRawPosition().distanceSquared(getRawPosition());
            if(_distance < closestDistance)
            {
                closest = item.interactable;
                closestDistance = _distance;
            }
        }

        foreach(StorageCabinet cabinet in Game.entities[10])
        {
            if(cabinet.interactable.remove)
                continue;

            double _distance = cabinet.getRawPosition().distanceSquared(getRawPosition());
            if(_distance < closestDistance)
            {
                closest = cabinet.interactable;
                closestDistance = _distance;
            }
        }

        if(closest != null && closestDistance < 32)
        {
            closest.position = getRawPosition() + new Vector2d(28, 1);
            closest.tooltip.active = true;
            bool pickupClosest = false;

            if(bg == -1)
            {
                if(Input.isJustPressed(SDL2.SDL.SDL_Keycode.SDLK_e))
                    pickupClosest = true;
            }
            else
            {
                if(Input.isJoyJustPressed(bg, SDL2.SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A))
                    pickupClosest = true;
            }

            if(pickupClosest)
                closest.interact(this);
        }

        drawable.color = Color.WHITE;
        if(damageCooldown > 0)
        {
            damageCooldown--;
            if(damageCooldown/20 % 2 == 1) drawable.color = new Color(255, 128, 128);
            else drawable.color = new Color(255, 255, 255);
        }

        arms.color = drawable.color;

        addVelocity();

        double animIncrement = Math.Abs(velocity.x/16) + Math.Abs(velocity.y/16);

        if(animIncrement > 0.05 && hasEffect(2) && Game.random.NextDouble() < 0.1)
            Game.spawnParticle(new DustParticle(getCenter() + new Vector2d(0, 14)));

        if(velocity.x < 0 && direction == true)
            animIncrement *= -1;
        if(velocity.x > 0 && direction == false)
            animIncrement *= -1;
            
        anim += animIncrement;

        if(anim > 6)
            anim = 0;
        if(anim < 0)
            anim = 5.9;

        if(lastCrosshairPosition.x < getCenter().x)
            direction = false;
        if(lastCrosshairPosition.x > getCenter().x)
            direction = true;

        walk.textureBounds.x = (int)anim * 24;
        walkArms.textureBounds.x = (int)anim * 24;

        if(direction)
        {
            ((Sprite)drawable).size.x = 1;
            arms.size.x = 1;
            arms2.size.x = 1;
        }
        else
        {
            ((Sprite)drawable).size.x = -1;
            arms.size.x = -1;
            arms2.size.x = -1;
        }

        velocity.x *= 0.9;
        velocity.y *= 0.9;

        if(hasEffect(4))
            velocity *= Game.random.NextDouble()+0.4;

        if(hasEffect(7))
            velocity *= 1.09;
    }

    public override bool genericCollision(Entity entity)
    {
        if(entity.collision == 2 && damageCooldown <= 0)
        {
            damage(((LivingEntity)entity).dealtDamage);
            damageSound.play();
            damageCooldown = 80;

            return false;
        }

        return base.genericCollision(entity);
    }

    public override void damage(double damage)
    {
        base.damage(damage * (((Cat)Game.entities[2][0]).hasEffect(0) ? 1.35 : 1));
    }

    public Vector2d getCenter() { return getRawPosition() + new Vector2d(idle.textureBounds.w/2.0, idle.textureBounds.h/2.0 - 8); }

    public override void draw(RenderWindow window, float alpha)
    {
        drawable.position = getBlendPosition(alpha);
        if(hasEffect(1) && drawable.color.g == 255)
        {
            drawable.tint = new Color(96, 126, 150, 0);
            if(arms != null)
                arms.tint = new Color(96, 126, 150, 0);
            if(arms2 != null)
                arms2.tint = new Color(96, 126, 150, 0);
        }
        else
        {
            drawable.tint = new Color(0, 0, 0, 0);
            if(arms != null)
                arms.tint = new Color(0, 0, 0, 0);
            if(arms2 != null)
                arms2.tint = new Color(0, 0, 0, 0);
        }

        window.draw(drawable);

        if(selectedWeapon < items.Count)
        {
            if(arms2 != null)
            {
                arms2.position = getBlendPosition(alpha);
                window.draw(arms2);
            }

            items[selectedWeapon].sprite.position = getBlendPosition(alpha);
            window.draw(items[selectedWeapon].sprite);
        }

        if(arms != null)
        {
            arms.position = getBlendPosition(alpha);
            window.draw(arms);
        }

        Vector2d crosshairRawPosition;
        if(bg == -1)
        {
            crosshairRawPosition = (Vector2d)Input.getMousePosition()/window.relativeSize - new Vector2d(window.gWidth, window.gHeight - 7)/2 - new Vector2d(5, 0);
            crosshair.color.a = 255;
        }
        else
        {
            double rJoyValue = Math.Abs(Input.getJoyAxis(bg, 1).x) + Math.Abs(Input.getJoyAxis(bg, 1).y);
            double lJoyValue = Math.Abs(Input.getJoyAxis(bg, 0).x) + Math.Abs(Input.getJoyAxis(bg, 0).y);

            crosshairRawPosition = (Vector2d)(rJoyValue > 0.2 ? (Input.getJoyAxis(bg, 1)*63) : (Input.getJoyAxis(bg, 0)*32));
            crosshair.color.a = (byte)(Math.Min(254, crosshair.position.distanceSquared(getRawPosition())*4));
        }

        crosshair.position = getBlendPosition(alpha) + crosshairRawPosition + new Vector2d(idle.textureBounds.w/2.0, idle.textureBounds.h/2.0 - 8);

        if(crosshair.color.a > 64)
            window.draw(crosshair);

        if(health < maxHealth)
        {
            healthBar.position = getBlendPosition(alpha);
            window.draw(new Rectangle(healthBar.position + healthBar.offset + new Vector2d(0, 1), new Vector2d(20 * (health/maxHealth), 3), new Color(61, 140, 64)));
            window.draw(healthBar);
        }
    }

    public void heal(double _hp)
    {
        health = Math.Min(maxHealth, health + _hp);
    }
}