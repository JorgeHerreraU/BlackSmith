using BlackSmith.Presentation.ViewModels.Doctors;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages.Doctors;

/// <summary>
///     Interaction logic for DoctorCreate.xaml
/// </summary>
public partial class DoctorCreate : Page
{
    public DoctorCreate(DoctorCreateViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
