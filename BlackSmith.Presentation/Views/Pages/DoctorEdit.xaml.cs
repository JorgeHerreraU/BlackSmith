using BlackSmith.Presentation.ViewModels;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages;

/// <summary>
///     Interaction logic for DoctorEdit.xaml
/// </summary>
public partial class DoctorEdit : Page
{
    public DoctorEdit(DoctorEditViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}