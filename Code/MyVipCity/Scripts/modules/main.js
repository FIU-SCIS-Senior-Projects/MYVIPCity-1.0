(function (window, requirejs, myVipCityVersion, dependencyLibraries) {
	'use strict';

	var packages = ['vip'];
	// require js config
	var config = {
		context: 'MyVIPCity',
		paths: dependencyLibraries.paths,
		shim: dependencyLibraries.shim,
		baseUrl: 'Scripts/modules',
		packages: packages,
		urlArgs: 'v=' + myVipCityVersion,
		waitSeconds: 0
	};


	var r = requirejs.config(config);

	r(['jquery', 'angular', 'vip', 'bootstrap', 'waves', 'app'], function (jQuery, angular, vip) {

	});
})(window, requirejs, window.myVipCityVersion, window.dependencyLibraries);