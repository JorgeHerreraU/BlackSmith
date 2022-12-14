using BlackSmith.Presentation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BlackSmith.Presentation.Controls.UserControls;

/// <summary>
///     Interaction logic for Modal.xaml
/// </summary>
public partial class Modal : UserControl
{
    public Modal(string message,
        ImageType image)
    {
        InitializeComponent();
        OnInitialize(message, image);
    }

    public Modal(IEnumerable<string> messages,
        ImageType image)
    {
        InitializeComponent();
        OnInitialize(messages, image);
    }

    private void OnInitialize(string message,
        ImageType image)
    {
        MessageList.ItemsSource = new List<ModalMessage>
            { new() { Title = message, Image = GetDisplayImage(image) } };
    }

    private void OnInitialize(IEnumerable<string> messages,
        ImageType image)
    {
        var displayImage = GetDisplayImage(image);

        var modalMessages = messages.Select(message => new ModalMessage
            { Title = message, Image = displayImage })
            .ToList();

        MessageList.ItemsSource = modalMessages;
    }

    private static BitmapImage GetDisplayImage(ImageType image)
    {
        return image switch
        {
            ImageType.Error => new BitmapImage(new Uri(@"/Assets/error.png", UriKind.Relative)),
            ImageType.Warning => new BitmapImage(new Uri(@"/Assets/warning.png", UriKind.Relative)),
            _ => throw new NotImplementedException()
        };
    }

    internal class ModalMessage
    {
        public string? Title { get; set; } = "";
        public BitmapImage? Image { get; set; }
    }
}
