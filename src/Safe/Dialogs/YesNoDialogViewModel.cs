using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace Safe.Dialogs
{
    public class YesNoDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; private set; } = "Confirmation";

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if(parameters.ContainsKey("Title"))
            {
                Title = parameters.GetValue<string>("Title");
            }

            Message = parameters.GetValue<string>("Message");
        }

        public DelegateCommand YesCommand { get; }

        public DelegateCommand NoCommand { get; }

        public YesNoDialogViewModel()
        {
            YesCommand = new DelegateCommand(OnYes);
            NoCommand = new DelegateCommand(OnNo);
        }

        private void OnNo()
        {
            var button = ButtonResult.No;

            var result = new DialogResult(button);

            RequestClose?.Invoke(result);
        }

        private void OnYes()
        {
            var button = ButtonResult.Yes;

            var result = new DialogResult(button);

            RequestClose?.Invoke(result);
        }
    }
}
