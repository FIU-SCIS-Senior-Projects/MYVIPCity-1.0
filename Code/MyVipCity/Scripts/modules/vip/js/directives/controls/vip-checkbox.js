define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipCheckbox', ['vipControlRenderingService', function (vipControlRenderingService) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// isolated scope
			scope: {},

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				// instantiate a control rendering service
				var controlRenderingService = vipControlRenderingService(scope, element);

				ngModelCtrl.$render = function () {
					scope._value = ngModelCtrl.$viewValue;
				};

				listeners.push(scope.$watch('_value', function (value) {
					ngModelCtrl.$setViewValue(value);
				}));

				controlRenderingService.setCreateReadModeElementFunction(function () {
					// tag to wrap the read only text
					var tag = attrs.wrapWith || 'span';
					// create the read mode element
					var readElement = angular.element('<' + tag + '>{{(!!_value ? "Yes" : "No")}}</' + tag + '>');
					// add css class
					if (attrs.readModeClass)
						readElement.addClass(attrs.readModeClass);
					// return element
					return readElement;
				});

				controlRenderingService.setCreateEditModeElementFunction(function () {
					// create edit mode element
					var editElement = angular.element('<div class="checkbox"><input type="checkbox" ng-model="_value" value="true"/><i class="input-helper inline-block"></i></div>');
					// add css class
					if (attrs.editModeClass)
						editElement.addClass(attrs.editModeClass);
					// return element
					return editElement;
				});

				listeners.push(scope.$watch('$parent.renderingMode', function (value) {
					controlRenderingService.setRenderingMode(value);
				}));

				listeners.push(scope.$on('$destroy', function () {
					controlRenderingService.destroy();
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});