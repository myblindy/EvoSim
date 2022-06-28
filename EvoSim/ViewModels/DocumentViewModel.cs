using EvoSim.Sim;

namespace EvoSim.ViewModels;

class DocumentViewModel : ReactiveObject
{
    string title;
    public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title, value); }

    public SimConfiguration Configuration { get; }
    public CellsGrid CellsGrid { get; }

    public WriteableBitmap PreviewImageSource { get; private set; }
    uint[] PreviewImageSourceData;
    public ICommand StepCommand { get; }

    void UpdatePreviewImageSource()
    {
        Array.Clear(PreviewImageSourceData);

        foreach (var cell in CellsGrid)
        {
            var hash = cell.Genome.GetHashCode();
            PreviewImageSourceData[cell.Position.Y * Configuration.Size + cell.Position.X] = Extensions.BytesToUInt32(
                (byte)(hash.GetByte(0) ^ hash.GetByte(3)),
                (byte)(hash.GetByte(1) ^ hash.GetByte(3)),
                (byte)(hash.GetByte(2) ^ hash.GetByte(3)),
                byte.MaxValue);
        }

        try
        {
            PreviewImageSource.Lock();
            PreviewImageSource.WritePixels(new(0, 0, Configuration.Size, Configuration.Size),
                PreviewImageSourceData, Configuration.Size * 4, 0);
        }
        finally
        {
            PreviewImageSource.Unlock();
        }
    }

    public DocumentViewModel(string title, SimConfiguration configuration)
    {
        this.title = title;
        Configuration = configuration;

        CellsGrid = new CellsGrid(Configuration);
        foreach (var cell in CellsGrid)
        {
            cell.PositionRandomly();
            cell.Genome = new(Configuration);
        }

        PreviewImageSource = new(Configuration.Size, Configuration.Size, 96, 96, PixelFormats.Bgra32, null);
        PreviewImageSourceData = new uint[Configuration.Size * Configuration.Size];
        //UpdatePreviewImageSource();

        //StepCommand = ReactiveCommand.Create(() =>
        //{
        //    CellsGrid.Step();
        //    UpdatePreviewImageSource();
        //});

        var sw = Stopwatch.StartNew();
        for (int iteration = 0; iteration < configuration.Iterations; ++iteration)
            CellsGrid.Step();
        Debug.WriteLine($"{configuration.Iterations} iterations took {sw.Elapsed}.");
    }
}
