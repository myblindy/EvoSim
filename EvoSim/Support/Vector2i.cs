namespace EvoSim.Support;

struct Vector2i
{
    public readonly int X, Y;

    public Vector2i(int x, int y) =>
        (X, Y) = (x, y);

    public static Vector2i operator +(Vector2i a, Vector2i b) =>
        new(a.X + b.X, a.Y + b.Y);
}
