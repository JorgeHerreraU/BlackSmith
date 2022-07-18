using System.Windows.Controls;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Views.Pages;

/// <summary>
///     Interaction logic for PatientList.xaml
/// </summary>
public partial class PatientList : Page
{
    public PatientList(PatientListViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}