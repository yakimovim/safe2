using Safe.Core.Domain;
using Safe.Services;

namespace Safe.ViewModels.Domain
{
    public abstract class FieldViewModel : EntityViewModel<Field, FieldViewModel>
    {
        protected FieldViewModel(
            Field model, 
            IContainer<FieldViewModel> parentContainer, 
            IMapper mapper) 
            : base(model, parentContainer, mapper)
        {
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }
    }

    public sealed class SingleLineTextFieldViewModel : FieldViewModel
    {
        public SingleLineTextFieldViewModel(
            SingleLineTextField model, 
            IContainer<FieldViewModel> parentContainer, 
            IMapper mapper) 
            : base(model, parentContainer, mapper)
        { }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }

    public sealed class MultiLineTextFieldViewModel : FieldViewModel
    {
        public MultiLineTextFieldViewModel(
            MultiLineTextField model,
            IContainer<FieldViewModel> parentContainer,
            IMapper mapper)
            : base(model, parentContainer, mapper)
        { }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }

    public sealed class PasswordFieldViewModel : FieldViewModel
    {
        public PasswordFieldViewModel(
            PasswordField model,
            IContainer<FieldViewModel> parentContainer,
            IMapper mapper)
            : base(model, parentContainer, mapper)
        { }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }
}
