define(['vip/js/vip', 'myVipCityConfig'], function (vip, myVipCityConfig) {
	'use strict';

	vip.constant('vipConfig', myVipCityConfig);

	vip.config(['$compileProvider', function ($compileProvider) {
		// TODO Set debug info to false in production
		$compileProvider.debugInfoEnabled(true);
	}]);
});