using AutoMapper;
using Safe.Core.Domain;

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
                        cfg.CreateMap<Settings, Configuration>();
                        cfg.CreateMap<IConfiguration, Settings>();
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
