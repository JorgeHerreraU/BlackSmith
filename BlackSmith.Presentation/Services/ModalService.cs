using System.Threading.Tasks;
using System.Windows;
using BlackSmith.Presentation.Controls;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Interfaces;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace BlackSmith.Presentation.Services;

public class ModalService : IModalService
{
    public void ShowErrorMessage(string message)
    {
        new MessageBox
        {
            Title = "Error",
            Content = new Modal(message, ModalImage.Error),
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowFooter = false
        }.Show();
    }

    public async Task<bool> ShowConfirmDialog(string message)
    {
        var completionSource = new TaskCompletionSource<bool>();
        var messageBox = new MessageBox { Content = message, ButtonLeftName = "OK", ButtonRightName = "Cancel" };

        messageBox.Show();

        messageBox.ButtonLeftClick += (_,
            _) =>
        {
            completionSource.SetResult(true);
            messageBox.Close();
        };

        messageBox.ButtonRightClick += (_,
            _) =>
        {
            completionSource.SetResult(false);
            messageBox.Close();
        };

        return await completionSource.Task;
    }
}