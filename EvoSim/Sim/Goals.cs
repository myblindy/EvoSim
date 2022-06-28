using System.Numerics;

namespace EvoSim.Sim;

interface IGoal
{
    bool Contains(Cell cell);
}

class CircleGoal : IGoal
{
    readonly float x;
    readonly float y;
    readonly float r;

    public CircleGoal(float x, float y, float r)
    {
        this.x = x;
        this.y = y;
        this.r = r;
    }

    public bool Contains(Cell cell) =>
        (new Vector2(x, y) - new Vector2(cell.Position.X, cell.Position.Y)).LengthSquared() <= r * r;
}

class RectangleGoal : IGoal
{
    readonly float x;
    readonly float y;
    readonly float w;
    readonly float h;

    public RectangleGoal(float x, float y, float w, float h)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
    }

    public bool Contains(Cell cell) =>
        x - w / 2 <= cell.Position.X && cell.Position.X <= x + w / 2 &&
        y - h / 2 <= cell.Position.Y && cell.Position.Y <= y + h / 2;
}