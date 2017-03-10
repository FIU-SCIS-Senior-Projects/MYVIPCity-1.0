using System.Data.Entity;
using AutoMapper;
using MyVipCity.DataTransferObjects.Contracts;
using MyVipCity.Domain.Automapper.MappingContext;
using Ninject;

namespace MyVipCity.BusinessLogic {

	public class AbstractEntityManager {


		[Inject]
		public DbContext DbContext
		{
			get;
			set;
		}

		[Inject]
		public IMapper Mapper
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
			var model = dto.Id == 0 ? new TModel() : DbContext.Set<TModel>().Find(dto.Id);
			return ToModel(dto, model);
		}

		protected TModel ToModel<TModel, TDto>(TDto dto, TModel model)
			where TDto : class, IIdentifiableDto
			where TModel : class, new() {
			var result = Mapper.Map<TDto, TModel>(dto, model,
				opts => {
					opts.Items.Add(typeof(DtoToModelContext).Name, new DtoToModelContext());
					opts.Items.Add(typeof(DbContext).Name, DbContext);
				});
			return result;
		}
	}
}
