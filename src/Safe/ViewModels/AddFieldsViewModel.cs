using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Services;
using Safe.ViewModels.Domain;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Safe.ViewModels
{
    public sealed class SelectableFieldViewModel : BindableBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        private FieldViewModel _field;
        public FieldViewModel Field
        {
            get { return _field; }
            set { SetProperty(ref _field, value); }
        }
    }

    public class AddFieldsViewModel : BindableBase, INavigationAware
    {
        private readonly IMapper _mapper;
        private readonly INavigationService _navigationService;

        private IRegionNavigationJournal _journal;
        private IContainer<FieldViewModel> _container;

        public ObservableCollection<SelectableFieldViewModel> Fields { get; } = new ObservableCollection<SelectableFieldViewModel>();

        public AddFieldsViewModel(IMapper mapper, INavigationService navigationService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            OkCommand = new DelegateCommand(AddFields);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void AddFields()
        {
            foreach (var selectedField in Fields.Where(f => f.IsSelected))
            {
                selectedField.Field.Add();
            }

            _journal.GoBack();
        }

        private void Cancel()
        {
            _journal.GoBack();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;

            // This is a back navigation.
            if (!navigationContext.Parameters.ContainsKey("Container")) return;

            _container = navigationContext.Parameters.GetValue<IContainer<FieldViewModel>>("Container");

            Fields.Clear();

            Fields.Add(
                new SelectableFieldViewModel
                {
                    Field = new SingleLineTextFieldViewModel(
                        new SingleLineTextField { Label = "URL:" },
                        _container,
                        _mapper,
                        _navigationService
                    )
                }
            );
            Fields.Add(
                new SelectableFieldViewModel
                {
                    Field = new SingleLineTextFieldViewModel(
                        new SingleLineTextField { Label = "Email:" },
                        _container,
                        _mapper,
                        _navigationService
                    )
                }
            );
            Fields.Add(
                new SelectableFieldViewModel
                {
                    Field = new SingleLineTextFieldViewModel(
                        new SingleLineTextField { Label = "Login:" },
                        _container,
                        _mapper,
                        _navigationService
                    )
                }
            );
            Fields.Add(
                new SelectableFieldViewModel
                {
                    Field = new PasswordFieldViewModel(
                        new PasswordField { Label = "Password:" },
                        _container,
                        _mapper,
                        _navigationService
                    )
                }
            );
            Fields.Add(
                new SelectableFieldViewModel
                {
                    Field = new MultiLineTextFieldViewModel(
                        new MultiLineTextField { Label = "" },
                        _container,
                        _mapper,
                        _navigationService
                    )
                }
            );
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public DelegateCommand OkCommand { get; }
        public DelegateCommand CancelCommand { get; }
    }
}
