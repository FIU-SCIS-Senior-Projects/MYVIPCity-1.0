using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVipCity.Domain.Automapper.MappingContext {

	public class DtoToModelContext {

		private class ObjectReferenceComparer: IEqualityComparer<object> {

			/// <summary>
			/// Determines whether the specified objects are equal.
			/// </summary>
			/// <returns>
			/// true if the specified objects are equal; otherwise, false.
			/// </returns>
			/// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
			bool IEqualityComparer<object>.Equals(object x, object y) {
				return ReferenceEquals(x, y);
			}

			/// <summary>
			/// Returns a hash code for the specified object.
			/// </summary>
			/// <returns>
			/// A hash code for the specified object.
			/// </returns>
			/// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
			public int GetHashCode(object obj) {
				return obj.GetHashCode();
			}
		}

		private readonly IDictionary<object, object> mappedObjects;

		public DtoToModelContext() {
			mappedObjects = new Dictionary<object, object>(new ObjectReferenceComparer());
		}

		public void SetMappedObjectInContext<TDto, TModel>(TDto key, TModel value) {
			mappedObjects.Add(key, value);
		}

		public TModel TryGetMappedInstance<TDto, TModel>(TDto key) {
			object result;
			mappedObjects.TryGetValue(key, out result);
			return (TModel)result;
		}
	}
}
