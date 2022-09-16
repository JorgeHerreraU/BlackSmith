using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackSmith.Presentation.Interfaces;

public interface IModalService
{
    void ShowErrorMessage(string message);
    void ShowErrorMessage(IEnumerable<string> messages);
    Task<bool> ShowConfirmDialog(string message, object? obj = null!);
}
