using EvoSim.Sim;

namespace EvoSim.ViewModels;

class MainViewModel : ReactiveObject
{
    public ObservableCollection<DocumentViewModel> Documents { get; } = new();

    DocumentViewModel? activeDocument;
    public DocumentViewModel? ActiveDocument { get => activeDocument; set => this.RaiseAndSetIfChanged(ref activeDocument, value); }

    public MainViewModel(IDialogService dialogService)
    {
        int lastDocumentIndex = 0;
        NewDocumentCommand = ReactiveCommand.Create(() =>
        {
            DocumentViewModel docVm = new($"sim{++lastDocumentIndex:00}", new()
            {
                GenomeLength = 4,
                InternalNeuronCount = 3,
                Size = 300,
                MutationChance = .2,
                PopulationSize = 500,
                Iterations = 4000,
                Goals =
                {
                    new RectangleGoal(0, 150, 40, 300),
                    new RectangleGoal(300, 150, 40, 300),
                }
            });
            //dialogService.ShowDialog(this, docVm);

            Documents.Add(docVm);
            ActiveDocument = Documents.Last();
        });

        NewDocumentCommand.Execute(default);
    }

    public ICommand NewDocumentCommand { get; }
}
