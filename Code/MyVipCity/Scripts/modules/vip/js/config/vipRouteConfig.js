define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
		// $locationProvider.html5Mode(true);
		$locationProvider.hashPrefix('');
		$routeProvider
			.when('/Testing', {
				template: '<div>Testing</div>'
			})
			.when('/', {
				template: '<div>Hello home</div>'
			})
			.otherwise({
				redirectTo: '/'
			});
	}]);
});