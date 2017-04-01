using System;
using System.Data.Entity;
using AutoMapper;
using MyVipCity.DataTransferObjects.Contracts;
using MyVipCity.Domain.Automapper.MappingContext;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain.Automapper.CustomResolvers {

	public abstract class ValueResolverBase {

		protected DbContext GetDbContextFromResolutionContext(ResolutionContext context) {
			object dbContext;
			context.Options.Items.TryGetValue(typeof(DbContext).Name, out dbContext);
			return (DbContext)dbContext;
		}

		protected DtoToModelContext GetDtoToModelContextFromResolutionContext(ResolutionContext context) {
			object dtoToModelContext;
			context.Options.Items.TryGetValue(typeof(DtoToModelContext).Name, out dtoToModelContext);
			if (dtoToModelContext != null)
				return (DtoToModelContext)dtoToModelContext;
			return new DtoToModelContext();
		}

		protected TModel MapDtoToModel<TDto, TModel>(TDto dto, Func<TDto, TModel> existingModelFinder, DtoToModelContext dtoToModelContext, DbContext dbContext, IMapper mapper)
			where TDto : class, IIdentifiableDto
			where TModel : class, IIdentifiable {
			TModel model;
			// check if the reference property is new
			model = dto.Id == 0 ? Activator.CreateInstance<TModel>() : existingModelFinder(dto);
			if (model == null) {
				throw new Exception($"{typeof(TModel).Name} with Id = {dto.Id} not found");
			}
			// store the mapped instance in the context
			dtoToModelContext.SetMappedObjectInContext(dto, model);
			// map from the dto property to the model property
			return MapDtoToModel(dto, model, dtoToModelContext, dbContext, mapper);
		}

		protected TModel MapDtoToModel<TDto, TModel>(TDto dto, TModel model, DtoToModelContext dtoToModelContext, DbContext dbContext, IMapper mapper) {
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (dto == null)
				throw new ArgumentNullException(nameof(dto));
			var result = mapper.Map(dto, model,
				opts => {
					opts.Items.Add(typeof(DtoToModelContext).Name, dtoToModelContext);
					opts.Items.Add(typeof(DbContext).Name, dbContext);
				});
			return result;
		}
	}
}
