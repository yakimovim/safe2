using AutoMapper;
using Safe.Core.Domain;
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

        public Mapper()
        {
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
