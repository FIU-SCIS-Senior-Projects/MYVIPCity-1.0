define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.directive('vipMatch', [function () {
		return {
			require: 'ngModel',
			restrict: 'A',
			link: function (scope, element, attrs, ngModelCtrl) {
				ngModelCtrl.$validators.vipMatch = function (modelValue, viewValue) {
					return viewValue === scope.$eval(attrs.vipMatch);
				};
			}
		};
	}]);
});