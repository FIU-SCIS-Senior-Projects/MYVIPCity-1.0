using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MyVipCity.DataTransferObjects.Contracts;
using MyVipCity.Domain.Automapper.MappingContext;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain.Automapper.CustomResolvers {

	public class CollectionValueResolver<TDto, TItemDto, TModel, TItemModel>: ValueResolverBase, IValueResolver<TDto, TModel, ICollection<TItemModel>>
		where TItemDto : class, IIdentifiableDto
		where TItemModel : class, IIdentifiable
		where TDto : class
		where TModel : class {

		private Func<TDto, ICollection<TItemDto>> getDtoCollectionProperty;
		private Func<TModel, ICollection<TItemModel>> getModelCollectionProperty;

		public CollectionValueResolver(Func<TDto, ICollection<TItemDto>> getDtoCollectionProperty, Func<TModel, ICollection<TItemModel>> getModelCollectionProperty) {
			this.getDtoCollectionProperty = getDtoCollectionProperty;
			this.getModelCollectionProperty = getModelCollectionProperty;
		}

		/// <summary>
		/// Implementors use source object to provide a destination object.
		/// </summary>
		/// <param name="source">Source object</param><param name="destination">Destination object, if exists</param><param name="destMember">Destination member</param><param name="context">The context of the mapping</param>
		/// <returns>
		/// Result, typically build from the source resolution result
		/// </returns>
		public ICollection<TItemModel> Resolve(TDto source, TModel destination, ICollection<TItemModel> destMember, ResolutionContext context) {
			DtoToModelContext dtoToModelContext = GetDtoToModelContextFromResolutionContext(context);
			var dbContext = GetDbContextResolutionContext(context);
			IMapper mapper = context.Mapper;

			// get the collection from the dto
			ICollection<TItemDto> dtoCollection = getDtoCollectionProperty(source);
			// if the collection is null, then return null
			if (dtoCollection == null)
				return null;
			// get the collection from the model
			ICollection<TItemModel> modelCollection = getModelCollectionProperty(destination);
			// if the collection in the model is null, then create a new one
			if (modelCollection == null)
				modelCollection = new List<TItemModel>();

			// store all the items in the model collection in a dictionary
			var modelCollectionDictionary = modelCollection.Where(item => item.Id > 0).ToDictionary(item => item.Id);
			// clear the model collection
			modelCollection.Clear();

			// loop dto collection
			foreach (var itemDto in dtoCollection) {
				var modelItem = MapDtoToModel(itemDto, dto => modelCollectionDictionary[dto.Id], dtoToModelContext, dbContext, mapper);
				modelCollection.Add(modelItem);
			}

			return modelCollection;
		}
	}
}
