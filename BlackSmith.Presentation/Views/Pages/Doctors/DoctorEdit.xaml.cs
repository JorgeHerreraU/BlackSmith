using BlackSmith.Presentation.ViewModels.Doctors;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages.Doctors;

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
