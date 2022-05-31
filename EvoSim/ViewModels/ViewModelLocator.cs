using Lamar;

namespace EvoSim.ViewModels;

static class ViewModelLocator
{
    static readonly Lamar.Container container = new(x =>
    {
        x.For<MainViewModel>().Add<MainViewModel>().Singleton();
        x.For<IDialogService>().Use<DialogService>().Singleton();
    });

    public static MainViewModel MainViewModel { get; } = container.GetInstance<MainViewModel>();
}
