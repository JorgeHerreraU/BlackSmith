using System.Threading.Tasks;

namespace BlackSmith.Presentation.Interfaces;

public interface IMessageService
{
    void ShowErrorMessage(string message);
    Task<bool> ShowConfirmDialog(string message);
}