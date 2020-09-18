using AutoMapper;
using Safe.Core.Domain;
using Safe.ViewModels;
using Safe.ViewModels.Domain;
using System;
using System.Linq;
using System.Text;

namespace Safe.Services
{
    public interface IMapper
    {
        void Map<TSource, TDestination>(TSource source, TDestination destination);
    }

    public sealed class Mapper : IMapper
    {
        private readonly AutoMapper.Mapper _mapper;
        private readonly INavigationService _navigationService;

        public Mapper(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Settings, Configuration>()
                            .ForMember(c => c.Salt, act => {
                                act.MapFrom(s => Encoding.ASCII.GetBytes(s.Salt));
                            });
                        cfg.CreateMap<IConfiguration, Settings>()
                            .ForMember(s => s.Salt, act => {
                                act.MapFrom(c => Encoding.ASCII.GetString(c.Salt));
                            });

                        cfg.CreateMap<Item, ItemViewModel>()
                            .ForMember(vm => vm.Tags, act => {
                                act.MapFrom(i => string.Join(", ", i.Tags));
                            })
                            .ForMember(vm => vm.Fields, act => {
                                act.MapFrom((i, vm) =>
                                {
                                    return i.Fields
                                        .Select(f => FieldViewModel.Create(f, vm, this, _navigationService))
                                        .ToArray();
                                });
                            });
                        cfg.CreateMap<ItemViewModel, Item>()
                            .ForMember(i => i.Tags, act => {
                                act.MapFrom((vm, i) =>
                                {
                                    if (string.IsNullOrWhiteSpace(vm.Tags)) return new string[0];

                                    return vm.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Where(t => !string.IsNullOrWhiteSpace(t))
                                        .Select(t => t.Trim())
                                        .ToArray();
                                });
                            })
                            .ForMember(i => i.Fields, act => {
                                act.MapFrom((vm, i) =>
                                {
                                    return vm.Fields.Select(f => f.Model).ToArray();
                                });
                            });

                        cfg.CreateMap<SingleLineTextField, SingleLineTextFieldViewModel>();
                        cfg.CreateMap<SingleLineTextFieldViewModel, SingleLineTextField>();
                        cfg.CreateMap<MultiLineTextField, MultiLineTextFieldViewModel>();
                        cfg.CreateMap<MultiLineTextFieldViewModel, MultiLineTextField>();
                        cfg.CreateMap<PasswordField, PasswordFieldViewModel>();
                        cfg.CreateMap<PasswordFieldViewModel, PasswordField>();
                    }
                );
            _mapper = new AutoMapper.Mapper(config);
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            _mapper.Map(source, destination);
        }
    }
}
