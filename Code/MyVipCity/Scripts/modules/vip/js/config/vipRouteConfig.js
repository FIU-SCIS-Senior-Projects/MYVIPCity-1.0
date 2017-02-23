define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['$routeProvider', 'vipConfig', '$locationProvider', function ($routeProvider, vipConfig, $locationProvider) {
		// $locationProvider.html5Mode(true);
		$locationProvider.hashPrefix('');

		// get the routes
		var routes = vipConfig.Routes || [];

		// loop the routes
		angular.forEach(routes, function (route) {
			// set the route
			$routeProvider.when(route.Path, {
				template: route.Template,
				templateUrl: route.TemplateUrl
			});
		});

		if (window.location.pathname === '/') {
			$routeProvider.otherwise({
				redirectTo: '/home'
			});
		}
	}]);
});