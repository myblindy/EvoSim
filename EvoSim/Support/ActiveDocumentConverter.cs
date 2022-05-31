using EvoSim.ViewModels;
using System.Globalization;

namespace EvoSim.Support;

class ActiveDocumentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is DocumentViewModel documentViewModel ? documentViewModel : Binding.DoNothing;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is DocumentViewModel documentViewModel ? documentViewModel : Binding.DoNothing;
}
