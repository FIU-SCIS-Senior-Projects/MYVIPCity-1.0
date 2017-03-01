using System;
using System.Data.Entity;
using AutoMapper;
using MyVipCity.DataTransferObjects.Contracts;
using MyVipCity.Domain.Automapper.MappingContext;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain.Automapper.CustomResolvers {

	public abstract class ValueResolverBase {

		protected DbContext GetDbContextResolutionContext(ResolutionContext context) {
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
			if (dto.Id == 0) {
				// create a new instance of the model property
				model = Activator.CreateInstance<TModel>();
			}
			else {
				// the reference property is not null, so it must be retrieved 
				model = existingModelFinder(dto);
			}
			// store the mapped instance in the context
			dtoToModelContext.SetMappedObjectInContext(dto, model);
			// map from the dto property to the model property
			var result = mapper.Map<TDto, TModel>(dto, model,
				opts => {
					opts.Items.Add(typeof(DtoToModelContext).Name, dtoToModelContext);
					opts.Items.Add(typeof(DbContext).Name, dbContext);
				});
			return result;
		}
	}
}
