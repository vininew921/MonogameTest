using System;

public static class Art
{
    public static Texture2D Player { get; private set; }
    public static Texture2D Seeker { get; private set; }
    public static Texture2D Wanderer { get; private set; }
    public static Texture2D Bullet { get; private set; }
    public static Texture2D Pointer { get; private set; }

    public static void Load(ContentManager contentManager)
    {
        Player = contentManager.Load<Texture2D>("Art/Player");
        Seeker = contentManager.Load<Texture2D>("Art/Seeker");
        Wanderer = contentManager.Load<Texture2D>("Art/Wanderer");
        Bullet = contentManager.Load<Texture2D>("Art/Bullet");
        Pointer = contentManager.Load<Texture2D>("Art/Pointer");
    }
}
