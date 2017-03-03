using System;
using System.Data.Entity;
using AutoMapper;
using MyVipCity.DataTransferObjects.Contracts;
using MyVipCity.Domain.Automapper.MappingContext;
using MyVipCity.Domain.Contracts;
using Ninject;

namespace MyVipCity.Domain.Automapper.CustomResolvers {

	public class ReferenceValueResolver<TDto, TModel, TDtoProperty, TModelProperty>: ValueResolverBase, IValueResolver<TDto, TModel, TModelProperty>
		where TDtoProperty : class, IIdentifiableDto
		where TModelProperty : class, IIdentifiable
		where TDto : class
		where TModel : class {

		private Func<TDto, TDtoProperty> getDtoProperty;

		public ReferenceValueResolver(Func<TDto, TDtoProperty> getDtoProperty) {
			this.getDtoProperty = getDtoProperty;
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
			var dbContext = GetDbContextResolutionContext(context);
			IMapper mapper = context.Mapper;

			// get the property from the dto
			TDtoProperty dtoProperty = getDtoProperty(source);
			// if the dto property is null then return null as well
			if (dtoProperty == null)
				return null;
			// to store the model property object
			TModelProperty modelProperty = null;
			// see if the property has been mapped before
			var mappedInstance = dtoToModelContext.TryGetMappedInstance<TDtoProperty, TModelProperty>(dtoProperty);
			if (mappedInstance != null)
				return mappedInstance;
			var result = MapDtoToModel(dtoProperty, dto => dbContext.Set<TModelProperty>().Find(dto.Id), dtoToModelContext, dbContext, mapper);
			return result;
		}
	}
}
