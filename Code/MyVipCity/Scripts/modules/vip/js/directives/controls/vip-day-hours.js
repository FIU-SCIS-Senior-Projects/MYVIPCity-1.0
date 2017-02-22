define(['vip/js/vip', 'jquery', 'angular', 'trumbowyg', 'moment'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipDayHours', ['vipControlRenderingService', '$locale', function (vipControlRenderingService, $locale) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			controller: ['$scope', function ($scope) {
				$scope.locale = $locale;
			}],
			link: function (scope, element, attrs) {
				var listeners = [];
				// instantiate a control rendering service
				var controlRenderingService = vipControlRenderingService(scope, element);

				controlRenderingService.setCreateReadModeElementFunction(function () {
					// create the read mode element
					var readElement = angular.element(
						'<div class="vip-day-hours__read-mode-container">' +
							'<span class="vip-day-hours__day">{{locale.DATETIME_FORMATS.SHORTDAY[' + attrs.ngModel + '.Day]}}</span>' +
							'<div vip-time-picker ng-model="' + attrs.ngModel + '.OpenTime" placeholder="Open Time" wrap-with="span" ng-hide="' + attrs.ngModel + '.Closed"></div>' +
							'<span ng-hide="' + attrs.ngModel + '.Closed"> - </span>' +
							'<div vip-time-picker ng-model="' + attrs.ngModel + '.CloseTime" placeholder="Close Time" wrap-with="span" ng-hide="' + attrs.ngModel + '.Closed"></div>' +
							'<span class="vip-day-hours__closed" ng-show="' + attrs.ngModel + '.Closed">Closed</span>' +
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
						'<div class="vip-day-hours__edit-mode-container">' +
							'<span class="vip-day-hours__day">{{locale.DATETIME_FORMATS.SHORTDAY[' + attrs.ngModel + '.Day]}}</span>' +
							'<div vip-time-picker ng-model="' + attrs.ngModel + '.OpenTime" placeholder="Open Time" wrap-with="span" ng-hide="' + attrs.ngModel + '.Closed"></div>' +
							'<span ng-hide="' + attrs.ngModel + '.Closed"> - </span>' +
							'<div vip-time-picker ng-model="' + attrs.ngModel + '.CloseTime" placeholder="Close Time" wrap-with="span" ng-hide="' + attrs.ngModel + '.Closed"></div>' +
							'<span class="vip-day-hours__closed">Closed</span><div vip-checkbox class="inline-block" ng-model="' + attrs.ngModel + '.Closed" wrap-with="span"></div>' +
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