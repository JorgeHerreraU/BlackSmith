using BlackSmith.Presentation.ViewModels.Patients;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages.Patients;

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
