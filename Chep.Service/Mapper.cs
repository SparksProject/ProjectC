namespace Chep.Service
{
    public static class Mapper
    {
        public static T MapSingle<S, T>(object source)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<S, T>();
            });

            var mapper = config.CreateMapper();

            var result = mapper.Map<S, T>((S)source);

            return result;
        }
    }
}