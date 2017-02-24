define(['vip/js/vip', 'myVipCityConfig'], function (vip, myVipCityConfig) {
	'use strict';

	vip.constant('vipConfig', myVipCityConfig);

	var apiKeys = {
		googleGeocoding: 'AIzaSyA6gYX9mBMwY7RHZCylYpfOCwA5nCYUqww',
		googleMaps: 'AIzaSyCilfDIIXr4o4c3FSLgTwG5jynPpu_TJgY'
	};

	vip.constant('vipApiKeys', apiKeys);

	vip.config(['$compileProvider', function ($compileProvider) {
		// TODO Set debug info to false in production
		$compileProvider.debugInfoEnabled(true);
	}]);
});