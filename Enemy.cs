namespace StreamGame;

public class Enemy : Entity
{
    private static Random _random = new();

    private int _timeUntilStart = 60;
    private List<IEnumerator<int>> behaviours = new();

    public bool IsActive => _timeUntilStart <= 0;

    public Enemy(Texture2D enemyImage, Vector2 position)
    {
        image = enemyImage;
        Position = position;
        Radius = image.Width / 2f;
        color = Color.Transparent;
    }

    public override void Update()
    {
        if (_timeUntilStart <= 0)
        {
            ApplyBehaviours();
        }
        else
        {
            _timeUntilStart--;
            color = Color.White * (1 - _timeUntilStart / 60f);
        }

        Position += Velocity;
        Position = Vector2.Clamp(Position, GetSize() / 2, StreamGame.ScreenSize - GetSize() / 2);

        Velocity *= 0.8f;
    }

    public void TakeDamage() => IsExpired = true;

    public static Enemy CreateSeeker(Vector2 position)
    {
        Enemy enemy = new(Art.Seeker, position);
        enemy.AddBehaviour(enemy.FollowPlayer());

        return enemy;
    }

    public static Enemy CreateWanderer(Vector2 position)
    {
        Enemy enemy = new(Art.Wanderer, position);
        enemy.AddBehaviour(enemy.MoveRandomly());

        return enemy;
    }

    //Behaviours
    private IEnumerable<int> FollowPlayer(float acceleration = 1f)
    {
        while (true)
        {
            Velocity += (PlayerShip.Instance.Position - Position).ScaleTo(acceleration);
            if (Velocity != Vector2.Zero)
            {
                Orientation = Velocity.ToAngle();
            }

            yield return 0;
        }
    }

    private IEnumerable<int> MoveRandomly()
    {
        float direction = _random.NextFloat(0, MathHelper.TwoPi);

        while (true)
        {
            direction += _random.NextFloat(-0.1f, 0.1f);
            direction = MathHelper.WrapAngle(direction);

            for (int i = 0; i < 6; i++)
            {
                Velocity += MathUtil.FromPolar(direction, 0.4f);
                Orientation -= 0.05f;

                Rectangle bounds = StreamGame.Viewport.Bounds;
                bounds.Inflate(-image.Width, -image.Height);

                //If enemy is outside of the bounds, move away from the edge
                if (!bounds.Contains(Position.ToPoint()))
                {
                    direction = (StreamGame.ScreenSize / 2 - Position).ToAngle() + _random.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);
                }

                yield return 0;
            }
        }
    }

    private void AddBehaviour(IEnumerable<int> behaviour) => behaviours.Add(behaviour.GetEnumerator());

    private void ApplyBehaviours()
    {
        for (int i = 0; i < behaviours.Count; i++)
        {
            if (!behaviours[i].MoveNext())
            {
                behaviours.RemoveAt(i--);
            }
        }
    }
}
