using System.Windows.Controls;
using BlackSmith.Presentation.ViewModels;

namespace BlackSmith.Presentation.Views.Pages;

/// <summary>
///     Interaction logic for PatientEdit.xaml
/// </summary>
public partial class PatientEdit : Page
{
    public PatientEdit(PatientEditViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}