(function (window, requirejs, myVipCityVersion, dependencyLibraries) {
	'use strict';

	var packages = ['vip'];
	// require js config
	var config = {
		context: 'MyVIPCity',
		paths: dependencyLibraries.paths,
		shim: dependencyLibraries.shim,
		baseUrl: '/Scripts/modules',
		packages: packages,
		urlArgs: 'v=' + myVipCityVersion,
		waitSeconds: 0
	};


	var r = requirejs.config(config);

	r(['jquery', 'angular', 'vip', 'moment', 'bootstrap', 'waves', 'app', 'trumbowyg', 'lightgallery'], function (jQuery, angular, vip, moment) {
		// add moment to global scope to keep compatibility with some libraries like mdPickers
		window.moment = moment;

		angular.bootstrap(document, [vip.name]);

		// TODO Encapsulate this
		function fade(element) {
			var op = 1;  // initial opacity
			var timer = setInterval(function () {
				if (op <= 0.1) {
					clearInterval(timer);
					element.style.display = 'none';
				}
				element.style.opacity = op;
				element.style.filter = 'alpha(opacity=' + op * 100 + ")";
				op -= op * 0.1;
			}, 10);
		}

		fade(document.getElementById('page-loader'));
	});
})(window, requirejs, window.myVipCityVersion, window.dependencyLibraries);