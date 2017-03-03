define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipTextbox', ['vipControlRenderingService', function (vipControlRenderingService) {
		return {
			restrict: 'ACE',

			require: 'ngModel',
			// new scope
			scope: true,

			link: function (scope, element, attrs) {
				var listeners = [];

				// instantiate a control rendering service
				var controlRenderingService = vipControlRenderingService(scope, element);

				controlRenderingService.setCreateReadModeElementFunction(function () {
					// tag to wrap the read only text
					var tag = attrs.wrapWith || 'span';
					// create the read mode element
					var readElement = angular.element('<' + tag + '>{{' + attrs.ngModel + '}}</' + tag + '>');
					// add css class
					if (attrs.readModeClass)
						readElement.addClass(attrs.readModeClass);
					// return element
					return readElement;
				});

				controlRenderingService.setCreateEditModeElementFunction(function () {
					// create edit mode element
					var editElement = angular.element('<input placeholder="' + (attrs.placeholder || '') + '" ng-model="' + attrs.ngModel + '"' + (!!attrs.autoGrow ? ' vip-auto-grow-input' : '') + (attrs.ngModelOptions ? ' ng-model-options="' + attrs.ngModelOptions + '"' : '') + '/>');
					// add css class
					if (attrs.editModeClass)
						editElement.addClass(attrs.editModeClass);
					// set input type
					var inputType = attrs.type || 'text';
					editElement.attr('type', inputType);
					// return element
					return editElement;
				});

				listeners.push(scope.$watch('renderingMode', function (value) {
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