using System.Data.Entity;
using AutoMapper;
using MyVipCity.Common;
using MyVipCity.DataTransferObjects.Contracts;
using MyVipCity.Domain.Automapper.MappingContext;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class AbstractEntityManager {

		public AbstractEntityManager(IResolver resolver, IMapper mapper, ILogger logger) {
			Resolver = resolver;
			Mapper = mapper;
			Logger = logger;
		}

		public IResolver Resolver
		{
			get;
			set;
		}
		
		public IMapper Mapper
		{
			get;
			set;
		}

		public ILogger Logger
		{
			get;
			set;
		}

		protected TDto ToDto<TDto, TModel>(TModel model)
			where TDto : class
			where TModel : class {
			if (model == null)
				return null;
			var dto = Mapper.Map<TDto>(model);
			return dto;
		}

		protected TModel ToModel<TModel, TDto>(TDto dto)
			where TDto : class, IIdentifiableDto
			where TModel : class, new() {
			var db = Resolver.Resolve<DbContext>();
			var model = dto.Id == 0 ? new TModel() : db.Set<TModel>().Find(dto.Id);
			return ToModel(dto, model);
		}

		protected TModel ToModel<TModel, TDto>(TDto dto, TModel model)
			where TDto : class, IIdentifiableDto
			where TModel : class {
			var db = Resolver.Resolve<DbContext>();
			var result = Mapper.Map<TDto, TModel>(dto, model,
				opts => {
					opts.Items.Add(typeof(DtoToModelContext).Name, new DtoToModelContext());
					opts.Items.Add(typeof(DbContext).Name, db);
				});
			return result;
		}
	}
}
