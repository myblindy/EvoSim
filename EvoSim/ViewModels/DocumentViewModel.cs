namespace EvoSim.ViewModels;

class DocumentViewModel : ReactiveObject, IModalDialogViewModel
{
    string title;
    public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title, value); }

    int width, height;
    public int Width { get => width; set => this.RaiseAndSetIfChanged(ref width, value); }
    public int Height { get => height; set => this.RaiseAndSetIfChanged(ref height, value); }

    public bool? DialogResult { get; private set; }

    public DocumentViewModel(string title)
    {
        this.title = title;
    }
}
