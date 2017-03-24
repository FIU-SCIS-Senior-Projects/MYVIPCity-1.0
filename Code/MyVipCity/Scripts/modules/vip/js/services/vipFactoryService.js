define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.provider('vipFactoryService', [function () {
		var self = this;

		var map = {};

		self.registerFactory = function(factoryName, callback) {
			if (angular.isArray(factoryName)) {
				angular.forEach(factoryName, function(name) {
					self.registerFactory(name, callback);
				});
			}
			else {
				map[factoryName] = callback;
			}
		};

		self.$get = ['$injector', '$log', function($injector, $log) {
			return function(factoryName) {
				var instance = {};
				if (!map[factoryName]) {
					$log.warn('vipFactoryService - factory with name [' + factoryName + '] not found');
					return null;
				}
				instance = $injector.instantiate(map[factoryName], { instance: instance });
				return instance;
			};
		}];
	}]);
});