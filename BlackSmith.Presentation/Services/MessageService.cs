using System.Threading.Tasks;
using System.Windows;
using BlackSmith.Presentation.Controls;
using BlackSmith.Presentation.Enums;
using BlackSmith.Presentation.Interfaces;
using MessageBox = WPFUI.Controls.MessageBox;

namespace BlackSmith.Presentation.Services;

public class MessageService : IMessageService
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
        var msg = new MessageBox
        {
            Content = message,
            ButtonLeftName = "OK",
            ButtonRightName = "Cancel"
        };
        msg.Show();
        msg.ButtonLeftClick += (_, _) =>
        {
            completionSource.SetResult(true);
            msg.Close();
        };
        msg.ButtonRightClick += (_, _) =>
        {
            completionSource.SetResult(false);
            msg.Close();
        };

        return await completionSource.Task;
    }
}