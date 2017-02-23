define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipImagesControl', ['vipControlRenderingService', function (vipControlRenderingService) {
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
					// create the read mode element
					var readElement = angular.element('<div vip-images-gallery ng-model="' + attrs.ngModel + '"></div>');
					// add css class
					if (attrs.readModeClass)
						readElement.addClass(attrs.readModeClass);
					// return element
					return readElement;
				});

				controlRenderingService.setCreateEditModeElementFunction(function () {
					// create edit mode element
					var editElement = angular.element('<div vip-images-upload ng-model="' + attrs.ngModel + '"></div>');
					// add css class
					if (attrs.editModeClass)
						editElement.addClass(attrs.editModeClass);
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