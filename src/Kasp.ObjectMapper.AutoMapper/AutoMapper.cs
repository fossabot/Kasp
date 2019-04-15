using System.Linq;
using AutoMapper;

namespace Kasp.ObjectMapper.AutoMapper {
	public class AutoMapper : IObjectMapper<AutoMapper>, IObjectMapper {
		public AutoMapper(IMapper mapper) {
			Mapper = mapper;
		}

		private IMapper Mapper { get; }

		public TDestination MapTo<TDestination>(object source) => Mapper.Map<TDestination>(source);
		public TDestination MapTo<TDestination>(object source, TDestination destination) => Mapper.Map(source, destination);
		public IQueryable<TDestination> MapTo<TDestination>(IQueryable source) => Mapper.ProjectTo<TDestination>(source);
	}
}