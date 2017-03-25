define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.directive('vipNonEmptyArray', [function () {
		return {
			restrict: 'A',
			
			require: 'ngModel',

			link: function (scope, element, attrs, ngModelCtrl) {
				ngModelCtrl.$validators.vipNonEmptyArray = function(modelValue) {
					return modelValue && modelValue.length > 0;
				};
			}
		};
	}]);
});