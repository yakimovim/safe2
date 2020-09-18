using Prism.Mvvm;
using Safe.Services;

namespace Safe.ViewModels
{
    public abstract class EntityViewModel<TModel, TView> : BindableBase 
        where TView : EntityViewModel<TModel, TView>
    {
        protected readonly IContainer<TView> ParentContainer;
        private readonly IMapper _mapper;

        public TModel Model { get; }

        private TView View => (TView)this;

        public EntityViewModel(
            TModel model,
            IContainer<TView> parentContainer,
            IMapper mapper
            )
        {
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));

            Model = model;
            ParentContainer = parentContainer;

            RefreshFromModel();
        }

        public virtual void RefreshFromModel()
        {
            _mapper.Map(Model, this);
        }

        public virtual void FillModel()
        {
            _mapper.Map(this, Model);
        }

        public void Add()
        {
            ParentContainer?.Add(View);
        }

        public void Delete()
        {
            ParentContainer?.Delete(View);
        }

        public bool CanMoveUp => ParentContainer?.CanMoveUp(View) ?? false;

        public bool CanMoveDown => ParentContainer?.CanMoveDown(View) ?? false;

        public void MoveUp()
        {
            if (CanMoveUp) ParentContainer?.MoveUp(View);
        }

        public void MoveDown()
        {
            if (CanMoveDown) ParentContainer?.MoveDown(View);
        }
    }
}
