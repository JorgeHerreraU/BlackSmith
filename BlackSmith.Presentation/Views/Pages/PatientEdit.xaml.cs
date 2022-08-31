using BlackSmith.Presentation.ViewModels;
using System.Windows.Controls;

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