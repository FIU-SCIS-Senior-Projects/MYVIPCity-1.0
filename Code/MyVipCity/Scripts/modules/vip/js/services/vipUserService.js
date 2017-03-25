define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.factory('vipUserService', ['vipConfig', function (vipConfig) {

		return {
			isAdmin: function () {
				return vipConfig && angular.isDefined(vipConfig.Roles) && vipConfig.Roles.indexOf('Admin') > -1;
			},

			isAuthenticated: function() {
				return vipConfig.IsAuthenticated;
			}
		};
	}]);
});