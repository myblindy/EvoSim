using EvoSim.Sim;

namespace EvoSim.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            var config = new SimConfiguration()
            {
                GenomeLength = 4,
                InternalNeuronCount = 3,
                Size = 100,
                MutationChance = .2
            };
            var cells = Enumerable.Range(0, 50).Select(_ => new Cell(config)).ToArray();
            foreach (var cell in cells)
            {
                cell.PositionRandomly(config);
                cell.Genome = new(config);
            }

            var childGene = Genome.MakeChild(config, cells.Take(10).Select(c => c.Genome).ToList());
        }
    }
}
