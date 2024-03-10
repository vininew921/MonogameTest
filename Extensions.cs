namespace StreamGame;

public static class Extensions
{
    public static float ToAngle(this Vector2 vector) => MathF.Atan2(vector.Y, vector.X);

    public static float NextFloat(this Random rand, float min, float max) => (float)rand.NextDouble() * (max - min) + min;

    public static Vector2 ScaleTo(this Vector2 vector, float length) => vector * (length / vector.Length());
}
