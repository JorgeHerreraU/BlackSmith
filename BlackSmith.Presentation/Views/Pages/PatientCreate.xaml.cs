using BlackSmith.Presentation.ViewModels;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages;

/// <summary>
///     Interaction logic for PatientCreate.xaml
/// </summary>
public partial class PatientCreate : Page
{
    public PatientCreate(PatientCreateViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}