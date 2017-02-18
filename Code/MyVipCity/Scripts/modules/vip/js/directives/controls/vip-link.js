define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipLink', ['vipControlRenderingService', function (vipControlRenderingService) {
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
					scope._link = ngModelCtrl.$viewValue;
				};
				
				listeners.push(scope.$watch('_link', function (value) {
					// if the link does not start with either http:// or https
					if (value && value.indexOf('https://') !== 0 && value.indexOf('http://') !== 0)
						value='http://' + value;
					ngModelCtrl.$setViewValue(value);
					scope._link = value;
				}));

				controlRenderingService.setCreateReadModeElementFunction(function () {
					// create the read mode element
					var readElement = angular.element('<a href="{{_link}}" target="_blank">{{_link}}</a>');
					// add css class
					if (attrs.readModeClass)
						readElement.addClass(attrs.readModeClass);
					// return element
					return readElement;
				});

				controlRenderingService.setCreateEditModeElementFunction(function () {
					// create edit mode element
					var editElement = angular.element('<input type="text" placeholder="' + (attrs.placeholder || '') + '" ng-model="_link" ng-model-options="{updateOn: \'blur\'}"' + (!!attrs.autoGrow ? ' vip-auto-grow-input' : '') + '/>');
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