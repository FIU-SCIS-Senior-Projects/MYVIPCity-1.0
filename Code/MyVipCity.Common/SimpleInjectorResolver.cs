using SimpleInjector;

namespace MyVipCity.Common {

	public class SimpleInjectorResolver: IResolver {

		private readonly Container container;

		public SimpleInjectorResolver(Container container) {
			this.container = container;
		}

		public T Resolve<T>() where T : class {
			return container.GetInstance<T>();
		}
	}
}
