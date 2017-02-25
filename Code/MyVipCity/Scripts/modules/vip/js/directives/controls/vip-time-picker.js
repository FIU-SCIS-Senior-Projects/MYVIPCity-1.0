define(['vip/js/vip', 'jquery', 'angular', 'moment', 'jtTimePicker'], function (vip, jQuery, angular, moment) {
	'use strict';

	vip.directive('vipTimePicker', ['vipControlRenderingService', function (vipControlRenderingService) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				// instantiate a control rendering service
				var controlRenderingService = vipControlRenderingService(scope, element);

				ngModelCtrl.$render = function () {
					scope._time = ngModelCtrl.$viewValue ? moment(ngModelCtrl.$viewValue).toDate() : null;
					element.find('.vip-time-picker-input').timepicker('setTime', scope._time);
				};

				controlRenderingService.setCreateReadModeElementFunction(function () {
					// tag to wrap the read only text
					var tag = attrs.wrapWith || 'span';
					// create the read mode element
					var readElement = angular.element('<' + tag + '>{{_time | date:"h:mm a"}}</' + tag + '>');
					// add css class
					if (attrs.readModeClass)
						readElement.addClass(attrs.readModeClass);
					// return element
					return readElement;
				});

				controlRenderingService.setCreateEditModeElementFunction(function () {
					// create edit mode element
					var editElement = angular.element('<input class="vip-time-picker-input" type="text" ' + (attrs.placeholder ? ' placeholder="' + attrs.placeholder + '"' : '') + '/>');
					jQuery(editElement)
						.timepicker()
						.on('change', function (e) {
							var time = jQuery(e.target).timepicker('getTime');
							scope._time = time;
							if (time) {
								var isoDate = moment(time).toISOString();
								// TODO: fix UTC
								ngModelCtrl.$setViewValue(isoDate);
							}
							else {
								ngModelCtrl.$setViewValue(null);
							}
						});

					if (ngModelCtrl.$viewValue) {
						var timeToSet = moment(ngModelCtrl.$viewValue).toDate();
						jQuery(editElement).timepicker('setTime', timeToSet);
					}

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