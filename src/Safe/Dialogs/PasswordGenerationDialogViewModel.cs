using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Safe.Core.Domain;
using Safe.Core.Services;
using System;
using System.Windows;

namespace Safe.Dialogs
{
    public class PasswordGenerationDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IPasswordGenerator _passwordGenerator;

        public string Title => "Generate password";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }

        public PasswordGenerationDialogViewModel(IPasswordGenerator passwordGenerator)
        {
            _passwordGenerator = passwordGenerator ?? throw new ArgumentNullException(nameof(passwordGenerator));

            CopyCommand = new DelegateCommand(CopyPassword, CanCopy)
                .ObservesProperty(() => Password);

            CloseCommand = new DelegateCommand(Close);

            GenerateCommand = new DelegateCommand(GeneratePassword, CanGeneratePassword)
                .ObservesProperty(() => UseLetters)
                .ObservesProperty(() => UseDigits)
                .ObservesProperty(() => UsePunctuation)
                .ObservesProperty(() => PasswordLength);
        }

        private bool CanCopy() => !string.IsNullOrWhiteSpace(Password);

        private void GeneratePassword()
        {
            Password = _passwordGenerator.Generate(
                PasswordLength,
                UseLetters,
                UseDigits,
                UsePunctuation
            );
        }

        private bool CanGeneratePassword()
        {
            if (!(UseLetters || UseDigits || UsePunctuation)) return false;
            if (PasswordLength <= 0) return false;
            return true;
        }

        private void Close()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        private void CopyPassword()
        {
            Clipboard.SetText(Password);
        }

        private bool _useLetters = true;
        public bool UseLetters
        {
            get { return _useLetters; }
            set { SetProperty(ref _useLetters, value); }
        }

        private bool _useDigits = true;
        public bool UseDigits
        {
            get { return _useDigits; }
            set { SetProperty(ref _useDigits, value); }
        }

        private bool _usePunctuation = true;
        public bool UsePunctuation
        {
            get { return _usePunctuation; }
            set { SetProperty(ref _usePunctuation, value); }
        }

        private uint _passwordLength = 16;
        public uint PasswordLength
        {
            get { return _passwordLength; }
            set { SetProperty(ref _passwordLength, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public DelegateCommand CopyCommand { get; }

        public DelegateCommand GenerateCommand { get; }

        public DelegateCommand CloseCommand { get; }
    }
}
