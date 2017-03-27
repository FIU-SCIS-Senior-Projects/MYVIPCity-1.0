using Ninject;

namespace MyVipCity.Common {

	public class NinjectResolver: IResolver {

		private readonly IKernel kernel;

		public NinjectResolver(IKernel kernel) {
			this.kernel = kernel;
		}

		public T Resolve<T>() {
			return kernel.Get<T>();
		}
	}
}
