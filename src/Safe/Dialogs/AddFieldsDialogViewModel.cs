using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Safe.Core.Domain;
using System;

namespace Safe.Dialogs
{
    public class AddFieldsDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "Add fields";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        private int _type = 0;
        public int Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        public DelegateCommand  AddFieldCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public AddFieldsDialogViewModel()
        {
            AddFieldCommand = new DelegateCommand(AddField);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            var buttonResult = ButtonResult.Cancel;

            var parameters = new DialogParameters();

            RequestClose?.Invoke(new DialogResult(buttonResult, parameters));
        }

        private void AddField()
        {
            var buttonResult = ButtonResult.OK;

            Field field = null;
            switch(Type)
            {
                case 0:
                    {
                        field = new SingleLineTextField();
                        break;
                    }
                case 1:
                    {
                        field = new PasswordField();
                        break;
                    }
                case 2:
                    {
                        field = new MultiLineTextField();
                        break;
                    }
            }

            field.Label = Label;

            var parameters = new DialogParameters();
            parameters.Add("Field", field);

            RequestClose?.Invoke(new DialogResult(buttonResult, parameters));
        }
    }
}
