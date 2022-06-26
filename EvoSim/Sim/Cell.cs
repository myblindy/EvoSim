namespace EvoSim.Sim;

enum Direction { SW, S, SE, W, Center, E, NW, N, NE }

static class DirectionExtensions
{
    public static Direction Rotate(this Direction dir, int cw)
    {
        while (cw-- > 0)
            dir = dir switch
            {
                Direction.N => Direction.NE,
                Direction.NE => Direction.E,
                Direction.E => Direction.SE,
                Direction.SE => Direction.S,
                Direction.S => Direction.SW,
                Direction.SW => Direction.W,
                Direction.W => Direction.NW,
                Direction.NW => Direction.N,
                Direction.Center => Direction.Center,
                _ => throw new InvalidOperationException()
            };

        return dir;
    }
}

class Cell
{
    public Vector2i Position { get; set; }
    public Direction Direction { get; set; }
    public NeuralNet NeuralNet { get; }
    public SimConfiguration SimConfiguration { get; }

    Genome genome;
    public Genome Genome { get => genome; set => NeuralNet.Load(genome = value); }

    public Cell(SimConfiguration simConfiguration)
    {
        SimConfiguration = simConfiguration;
        NeuralNet = new(this);
    }

    public void PositionRandomly() =>
        (Position, Direction) = (new(Random.Shared.Next(SimConfiguration.Size), Random.Shared.Next(SimConfiguration.Size)), (Direction)Random.Shared.Next(9));

    public void Step() =>
        NeuralNet.Step();

    public void Move(Vector2i delta)
    {
        if (Position.X + delta.X < 0 || Position.X + delta.X >= SimConfiguration.Size)
            delta = new(0, delta.Y);
        if (Position.Y + delta.Y < 0 || Position.Y + delta.Y >= SimConfiguration.Size)
            delta = new(delta.X, 0);

        Position += delta;
    }
}
