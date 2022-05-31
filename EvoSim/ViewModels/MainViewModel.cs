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
            DocumentViewModel docVm = new($"sim{++lastDocumentIndex:00}");
            dialogService.ShowDialog(this, docVm);
            Documents.Add(docVm);
            ActiveDocument = Documents.Last();
        });
    }

    public ICommand NewDocumentCommand { get; }
}
