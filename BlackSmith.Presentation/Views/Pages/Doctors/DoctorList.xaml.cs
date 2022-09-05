using BlackSmith.Presentation.ViewModels.Doctors;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages.Doctors;

/// <summary>
///     Interaction logic for DoctorList.xaml
/// </summary>
public partial class DoctorList : Page
{
    public DoctorList(DoctorListViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
