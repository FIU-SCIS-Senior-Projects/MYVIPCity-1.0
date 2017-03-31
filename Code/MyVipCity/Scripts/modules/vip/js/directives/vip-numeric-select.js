define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.directive('vipNumericSelect', ['$compile', function ($compile) {
		return {
			require: 'ngModel',
			restrict: 'A',
			link: function (scope, element, attrs, ngModelCtrl) {
				ngModelCtrl.$render = function () {
					if (angular.isDefined(ngModelCtrl.$modelValue))
						element.closest('.form-group--float').addClass('form-group--active');
					else
						element.closest('.form-group--float').removeClass('form-group--active');
				};

				var min = parseInt(attrs.min);
				var max = parseInt(attrs.max);
				scope.array = [];
				for (var i = min; i <= max; i++)
					scope.array.push(i);

				var selectElement = angular.element(
					'<select class="form-control" ng-model="' + attrs.ngModel + '" ng-options="x for x in array"></select>'
					);

				if (attrs.ngChange) {
					selectElement.attr('ng-change', attrs.ngChange);
					element.removeAttr('ng-change');
				}

				element.append(selectElement);
				$compile(selectElement)(scope);
			}
		};
	}]);
});