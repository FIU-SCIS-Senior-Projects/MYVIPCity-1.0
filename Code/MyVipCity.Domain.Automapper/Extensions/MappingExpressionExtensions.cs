using System;
using System.Linq.Expressions;
using AutoMapper;
using MyVipCity.DataTransferObjects.Contracts;

namespace MyVipCity.Domain.Automapper.Extensions {
	public static class MappingExpressionExtensions {

		public static IMappingExpression<TSource, TDestination> CheckNonZeroIdForMember<TSource, TDestination, TDestinationMember, TSourceMember>(this IMappingExpression<TSource, TDestination> mappingExpression, Expression<Func<TDestination, TDestinationMember>> destinationMember, Expression<Func<TSource, TSourceMember>> sourceMember) where TSourceMember : IIdentifiableDto {
			mappingExpression.ForMember(destinationMember,
				opt => opt.Condition(src => {
					var member = sourceMember.Compile()(src);
					if (member != null && member.Id == 0)
						throw new Exception($"Id of {typeof(TSourceMember)} cannot be zero.");
					return true;
				}));

			return mappingExpression;
		}
	}
}
