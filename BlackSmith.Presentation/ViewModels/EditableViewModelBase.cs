using BlackSmith.Core.ExtensionMethods;
using BlackSmith.Presentation.Interfaces;
using FluentValidation;
using Prism.Commands;
using System;
using System.ComponentModel;

namespace BlackSmith.Presentation.ViewModels;

public abstract class EditableViewModelBase : ValidatableBase, IDisposable
{
    private readonly IModalService _modalService;
    private bool _isTouched;

    protected EditableViewModelBase(IModalService modalService)
    {
        _modalService = modalService;
        SaveCommand = new DelegateCommand(OnSave, CanSave);
        GoBack = new DelegateCommand(OnGoBack);
        ClearCommand = new DelegateCommand(OnClear);
    }

    public DelegateCommand SaveCommand { get; }
    public DelegateCommand GoBack { get; }
    public DelegateCommand ClearCommand { get; }

    public bool IsTouched
    {
        get => _isTouched;
        set
        {
            _isTouched = value;
            RaisePropertyChanged();
        }
    }
    public abstract void Dispose();

    protected abstract void OnGoBack();
    protected abstract bool CanSave();
    protected abstract void OnSave();
    public abstract void Initialize();
    public abstract void SubscribeChanges();

    private void OnClear()
    {
        Initialize();
    }

    protected void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender != null)
            IsTouched = !sender.IsAnyStringNullOrEmpty();
        SaveCommand.RaiseCanExecuteChanged();
    }

    protected void RaiseCanChange(object? sender, EventArgs e)
    {
        SaveCommand.RaiseCanExecuteChanged();
    }

    protected void OnSaveErrorHandler(Exception ex)
    {
        switch (ex)
        {
            case ValidationException e:
                _modalService.ShowErrorMessage(e.Errors.ToErrorList());
                break;
            case ArgumentException:
                _modalService.ShowErrorMessage(ex.Message);
                break;
        }
    }
}
