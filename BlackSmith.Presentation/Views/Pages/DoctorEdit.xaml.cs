using System.Windows.Controls;
using BlackSmith.Presentation.ViewModels;

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