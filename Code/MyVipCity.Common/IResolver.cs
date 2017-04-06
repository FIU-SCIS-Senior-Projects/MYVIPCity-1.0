﻿namespace MyVipCity.Common {

	public interface IResolver {

		T Resolve<T>() where T: class;
	}
}