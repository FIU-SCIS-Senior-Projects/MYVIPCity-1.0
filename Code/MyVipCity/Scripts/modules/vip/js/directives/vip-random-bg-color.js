define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.directive('vipRandomBgColor', ['vipColorsService',  function (vipColorsService) {
		return {
			restrict: 'A',
			link: function (scope, element) {
				element[0].style.backgroundColor = vipColorsService.hexToRGB(vipColorsService.getRandomLightColor(), 0.4);
			}
		};
	}]);
});