using System.Windows.Controls;
using System.Windows.Input;

namespace BlackSmith.Presentation.Views.Pages;

/// <summary>
///     Interaction logic for PatientCreate.xaml
/// </summary>
public partial class PatientCreate : Page
{
    public PatientCreate()
    {
        InitializeComponent();
    }

    private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (PhoneTextBox.MaskedTextProvider.MaskCompleted) return;
        PhoneTextBox.Focus();
        PhoneTextBox.Select(0, 0);
        e.Handled = true;
    }
}