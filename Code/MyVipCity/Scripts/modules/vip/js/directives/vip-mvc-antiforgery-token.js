define(['vip/js/vip', 'jquery'], function (vip, jQuery) {
	'use strict';

	vip.directive('vipMvcAntiforgeryToken', [function () {
		return {
			require: 'ngModel',
			restrict: 'C',
			scope: {
				ngModel: '='
			},
			transclude: true,
			link: function (scope, element, attrs, ngModelCtrl, transclude) {
				var content = transclude();
				var value = jQuery(content[1]).val();
				ngModelCtrl.$setViewValue(value);
			}
		};
	}]);
});