namespace StreamGame.Entities;

public class PlayerShip : Entity
{
    private static PlayerShip _instance;
    private static Random _random = new();

    private float _speed = 8;
    private int _bulletCooldown = 6;
    private int _bulletCooldownRemaining = 0;

    public static PlayerShip Instance => _instance ?? (_instance = new());

    private PlayerShip()
    {
        image = Art.Player;
        Position = StreamGame.ScreenSize / 2;
        Radius = 10;
    }

    public override void Update()
    {
        Velocity = _speed * Input.GetMovementDirection();
        Position += Velocity;
        Position = Vector2.Clamp(Position, GetSize() / 2, StreamGame.ScreenSize - GetSize() / 2);

        //Set orientation based on movement direction
        if (Velocity.LengthSquared() > 0)
        {
            Orientation = Velocity.ToAngle();
        }

        //Shoot
        Vector2 aim = Input.GetAimDirection();
        if (aim.LengthSquared() > 0 && _bulletCooldownRemaining == 0)
        {
            Shoot(aim);
        }

        _bulletCooldownRemaining = _bulletCooldownRemaining > 0 ? _bulletCooldownRemaining - 1 : 0;
    }

    private void Shoot(Vector2 aim)
    {
        _bulletCooldownRemaining = _bulletCooldown;

        float aimAngle = aim.ToAngle();
        Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

        float spread = _random.NextFloat(-0.04f, 0.04f) + _random.NextFloat(-0.04f, 0.04f);
        Vector2 velocity = MathUtil.FromPolar(aimAngle + spread, 11f);

        Vector2 offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
        EntityManager.Add(new Bullet(Position + offset, velocity));

        offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
        EntityManager.Add(new Bullet(Position + offset, velocity));
    }
}
