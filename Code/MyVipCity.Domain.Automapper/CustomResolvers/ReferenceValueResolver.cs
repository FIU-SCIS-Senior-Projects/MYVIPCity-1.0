using System;
using AutoMapper;
using MyVipCity.DataTransferObjects.Contracts;
using MyVipCity.Domain.Automapper.MappingContext;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain.Automapper.CustomResolvers {

	public class ReferenceValueResolver<TDto, TModel, TDtoProperty, TModelProperty>: ValueResolverBase, IValueResolver<TDto, TModel, TModelProperty>
		where TDtoProperty : class, IIdentifiableDto
		where TModelProperty : class, IIdentifiable
		where TDto : class
		where TModel : class {

		private readonly Func<TDto, TDtoProperty> getDtoProperty;
		private readonly bool loadOnly;

		public ReferenceValueResolver(Func<TDto, TDtoProperty> getDtoProperty) : this(getDtoProperty, false) {
		}

		/// <summary>
		/// Creates a new reference value resolver.
		/// </summary>
		/// <param name="getDtoProperty">Function that retrieves the instance of the dto property given the instance of the dto.</param>
		/// <param name="loadOnly">Indicates if model the model value will be loaded only instead of loaded and updated from the dto.</param>
		public ReferenceValueResolver(Func<TDto, TDtoProperty> getDtoProperty, bool loadOnly) {
			this.getDtoProperty = getDtoProperty;
			this.loadOnly = loadOnly;
		}

		/// <summary>
		/// Implementors use source object to provide a destination object.
		/// </summary>
		/// <param name="source">Source object</param><param name="destination">Destination object, if exists</param><param name="destMember">Destination member</param><param name="context">The context of the mapping</param>
		/// <returns>
		/// Result, typically build from the source resolution result
		/// </returns>
		public TModelProperty Resolve(TDto source, TModel destination, TModelProperty destMember, ResolutionContext context) {
			DtoToModelContext dtoToModelContext = GetDtoToModelContextFromResolutionContext(context);
			var dbContext = GetDbContextFromResolutionContext(context);
			IMapper mapper = context.Mapper;

			// get the property from the dto
			TDtoProperty dtoProperty = getDtoProperty(source);
			// if the dto property is null then return null
			if (dtoProperty == null)
				return null;
			// see if the property has been mapped before
			var mappedInstance = dtoToModelContext.TryGetMappedInstance<TDtoProperty, TModelProperty>(dtoProperty);
			if (mappedInstance != null)
				return mappedInstance;
			// check if the model property must be loaded only
			if (loadOnly && dtoProperty.Id > 0) {
				// find the model instance with the Id = dtopProperty.Id
				var existingModel = dbContext.Set<TModelProperty>().Find(dtoProperty.Id);
				// check if the model instance does not exist in the DB
				if (existingModel == null)
					throw new Exception($"{typeof(TModelProperty).Name} with Id = {dtoProperty.Id} not found");
				return existingModel;
			}

			var result = MapDtoToModel(dtoProperty, dto => dbContext.Set<TModelProperty>().Find(dto.Id), dtoToModelContext, dbContext, mapper);
			return result;
		}
	}
}
