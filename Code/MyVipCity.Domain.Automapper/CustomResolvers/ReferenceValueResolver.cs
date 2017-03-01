using System;
using System.Data.Entity;
using AutoMapper;
using MyVipCity.DataTransferObjects.Contracts;
using MyVipCity.Domain.Automapper.MappingContext;
using MyVipCity.Domain.Contracts;
using Ninject;

namespace MyVipCity.Domain.Automapper.CustomResolvers {

	public class ReferenceValueResolver<TDto, TModel, TDtoProperty, TModelProperty>: IValueResolver<TDto, TModel, TModelProperty>
		where TDtoProperty : class, IIdentifiableDto
		where TModelProperty: class, IIdentifiable
		where TDto: class
		where TModel: class {

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
			TModelProperty modelProperty = null;
			// see if the property has been mapped before
			var mappedInstance = dtoToModelContext.TryGetMappedInstance<TDtoProperty, TModelProperty>(dtoProperty);
			if (mappedInstance != null)
				return mappedInstance;
			// check if the reference property is new
			if (dtoProperty.Id == 0) {
				modelProperty = CreateModelPropertyInstance();
			}
			else {
				// the reference property is not null, so it must be retrieved from the database
				modelProperty = dbContext.Set<TModelProperty>().Find(dtoProperty.Id);
			}
			dtoToModelContext.SetMappedObjectInContext(dtoProperty, modelProperty);
			
			var result =  mapper.Map<TDtoProperty, TModelProperty>(dtoProperty, modelProperty, opts => {
				opts.Items.Add(typeof(DtoToModelContext).Name, dtoToModelContext);
				opts.Items.Add(typeof(DbContext).Name, dbContext);
			});
			return result;
		}

		private TModelProperty CreateModelPropertyInstance() {
			var instance = Activator.CreateInstance<TModelProperty>();
			return instance;
		}

		private DbContext GetDbContextResolutionContext(ResolutionContext context) {
			object dbContext;
			context.Options.Items.TryGetValue(typeof(DbContext).Name, out dbContext);
			return (DbContext)dbContext;
		}

		private DtoToModelContext GetDtoToModelContextFromResolutionContext(ResolutionContext context) {
			object dtoToModelContext;
			context.Options.Items.TryGetValue(typeof(DtoToModelContext).Name, out dtoToModelContext);
			if (dtoToModelContext != null)
				return (DtoToModelContext)dtoToModelContext;
			return new DtoToModelContext();
		}
	}
}
