using BlackSmith.Presentation.Controls;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace BlackSmith.Presentation.Services;

public class ModalService : IModalService
{
    private const string ErrorTitle = "Error";
    private const string ConfirmTitle = "Confirm";
    public void ShowErrorMessage(string message)
    {
        new MessageBox
        {
            Title = ErrorTitle,
            Content = new Modal(message, ImageType.Error),
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowFooter = false,
            MicaEnabled = true,
        }.Show();
    }

    public void ShowErrorMessage(IEnumerable<string> messages)
    {
        new MessageBox
        {
            Title = ErrorTitle,
            Content = new Modal(messages, ImageType.Error),
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowFooter = false,
            MicaEnabled = true,
        }.Show();
    }

    public async Task<bool> ShowConfirmDialog(string message)
    {
        var completionSource = new TaskCompletionSource<bool>();
        var messageBox = new MessageBox
        {
            Title = ConfirmTitle,
            Content = message,
            ButtonLeftName = "OK",
            ButtonRightName = "Cancel"
        };

        messageBox.Show();

        messageBox.ButtonLeftClick += (_, _) =>
        {
            completionSource.SetResult(true);
            messageBox.Close();
        };

        messageBox.ButtonRightClick += (_, _) =>
        {
            completionSource.SetResult(false);
            messageBox.Close();
        };

        return await completionSource.Task;
    }
}
