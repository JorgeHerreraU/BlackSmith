using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BlackSmith.Presentation.Enums;

namespace BlackSmith.Presentation.Controls;

/// <summary>
///     Interaction logic for Modal.xaml
/// </summary>
public partial class Modal : UserControl
{
    public Modal(string message, ModalImage modalImage)
    {
        InitializeComponent();
        SetText(message);
        SetImage(modalImage);
    }

    private void SetText(string message)
    {
        MainContent.Text = message;
    }

    private void SetImage(ModalImage modalImage)
    {
        MainImage.Source = modalImage switch
        {
            ModalImage.Error => new BitmapImage(new Uri(@"/Assets/error.png", UriKind.Relative)),
            ModalImage.Warning => new BitmapImage(new Uri(@"/Assets/warning.png", UriKind.Relative)),
            _ => MainImage.Source
        };
    }
}