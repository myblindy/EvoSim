namespace EvoSim.Sim;

class CellsGrid : IEnumerable<Cell>
{
    readonly SimConfiguration config;
    Cell[] cells;

    public CellsGrid(SimConfiguration config)
    {
        this.config = config;
        cells = Enumerable.Range(0, config.PopulationSize).Select(_ => new Cell(config)).ToArray();
    }

    public void Step()
    {
        foreach (var cell in cells)
            cell.Step();
    }

    public IEnumerator<Cell> GetEnumerator() => ((IEnumerable<Cell>)cells).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => cells.GetEnumerator();
}
