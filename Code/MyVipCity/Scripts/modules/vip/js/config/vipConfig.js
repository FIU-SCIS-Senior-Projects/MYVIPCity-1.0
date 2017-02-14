define(['vip/js/vip', 'myVipCityConfig'], function (vip, myVipCityConfig) {
	'use strict';

	vip.constant('vipConfig', myVipCityConfig);

	vip.config(['$compileProvider', function ($compileProvider) {
		$compileProvider.debugInfoEnabled(false);
	}]);
});