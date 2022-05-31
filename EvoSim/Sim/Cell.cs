namespace EvoSim.Sim;

enum Direction { SW, S, SE, W, CENTER, E, NW, N, NE }

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
                Direction.CENTER => Direction.CENTER,
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
    public Genome Genome { get; set; }

    public Cell(SimConfiguration simConfiguration) =>
        NeuralNet = new(simConfiguration);

    public void PositionRandomly(SimConfiguration simConfiguration) =>
        (Position, Direction) = (new(Random.Shared.Next(simConfiguration.Size), Random.Shared.Next(simConfiguration.Size)), (Direction)Random.Shared.Next(9));
}
