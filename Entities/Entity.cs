namespace StreamGame.Entities;

public abstract class Entity
{
    protected Texture2D image;
    protected Color color = Color.White;

    public Vector2 Position;
    public Vector2 Velocity;
    public float Orientation;
    public float Radius;
    public bool IsExpired;

    public Vector2 GetSize() => image is null ? Vector2.Zero : new Vector2(image.Width, image.Height);

    public abstract void Update();

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(image, Position, null, color, Orientation, GetSize() / 2f, 1f, 0, 0);
    }
}
