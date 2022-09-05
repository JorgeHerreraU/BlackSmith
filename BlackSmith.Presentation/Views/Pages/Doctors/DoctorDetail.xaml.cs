using BlackSmith.Presentation.ViewModels.Doctors;
using System.Windows.Controls;

namespace BlackSmith.Presentation.Views.Pages.Doctors;

/// <summary>
///     Interaction logic for DoctorDetail.xaml
/// </summary>
public partial class DoctorDetail : Page
{
    public DoctorDetail(DoctorDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
