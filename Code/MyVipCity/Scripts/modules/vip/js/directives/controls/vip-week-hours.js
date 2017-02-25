define(['vip/js/vip', 'jquery', 'angular', 'trumbowyg', 'moment'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipWeekHours', ['vipControlRenderingService', '$parse', function (vipControlRenderingService, $parse) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				// instantiate a control rendering service
				var controlRenderingService = vipControlRenderingService(scope, element);

				//ngModelCtrl.$render = function() {
				//	scope.model = ngModelCtrl.$viewValue;
				//};

				controlRenderingService.setCreateReadModeElementFunction(function () {
					// create the read mode element
					var readElement = angular.element(
						'<div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Monday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Tuesday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Wednesday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Thursday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Friday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Saturday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Sunday"></div>' +
						'</div>'
					);
					// add css class
					if (attrs.readModeClass)
						readElement.addClass(attrs.readModeClass);
					// return element
					return readElement;
				});

				controlRenderingService.setCreateEditModeElementFunction(function () {
					// create edit mode element
					var editElement = angular.element(
						'<div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Monday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Tuesday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Wednesday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Thursday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Friday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Saturday"></div>' +
							'<div vip-day-hours ng-model="' + attrs.ngModel + '.Sunday"></div>' +
						'</div>'
					);
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