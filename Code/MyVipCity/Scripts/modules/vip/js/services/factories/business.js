define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.config(['vipFactoryServiceProvider', function (vipFactoryServiceProvider) {
		vipFactoryServiceProvider.registerFactory('business', ['instance', '$q', function (instance, $q) {
			return $q.when(instance).then(function () {
				var newBusiness = {
					Id: 0,
					WeekHours: {
						Monday: {
							Day: 1
						},
						Tuesday: {
							Day: 2
						},
						Wednesday: {
							Day: 3
						},
						Thursday: {
							Day: 4
						},
						Friday: {
							Day: 5
						},
						Saturday: {
							Day: 6
						},
						Sunday: {
							Day: 0
						}
					},

					Images: [],

					Address: {}
				}
				angular.extend(instance, newBusiness);
				return instance;
			});
		}]);
	}]);
});