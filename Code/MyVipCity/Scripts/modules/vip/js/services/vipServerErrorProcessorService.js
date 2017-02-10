define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.factory('vipServerErrorProcessorService', ['$log', function($log) {
		return function(data) {
			if (angular.isString(data))
				return data;
			if (angular.isArray(data))
				return data.join('\n');
			$log.warn('Unrecognized data format in vipServerErrorProcessorService');
			return '';
		};
	}]);
});