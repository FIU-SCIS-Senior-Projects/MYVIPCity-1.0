﻿define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipTextbox', ['vipControlRenderingService', function (vipControlRenderingService) {
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
					scope._text = ngModelCtrl.$viewValue;
				};

				listeners.push(scope.$watch('_text', function (value) {
					ngModelCtrl.$setViewValue(value);
				}));

				controlRenderingService.setCreateReadModeElementFunction(function () {
					// tag to wrap the read only text
					var tag = attrs.wrapWith || 'label';
					// create the read mode element
					var element = angular.element('<' + tag + '>{{_text}}</' + tag + '>');
					// add css class
					if (attrs.readModeClass)
						element.addClass(attrs.readModeClass);
					// return element
					return element;
				});

				controlRenderingService.setCreateEditModeElementFunction(function () {
					// create edit mode element
					var element = angular.element('<input placeholder="' + (attrs.placeholder || '') + '" ng-model="_text"' + (!!attrs.autoGrow ? ' vip-auto-grow-input' : '') + '/>');
					// add css class
					if (attrs.editModeClass)
						element.addClass(attrs.editModeClass);
					// set input type
					var inputType = attrs.type || 'text';
					element.attr('type', inputType);
					// return element
					return element;
				});

				listeners.push(scope.$watch('$parent.renderingMode', function (value) {
					controlRenderingService.setRenderingMode(value);
				}));

				listeners.push(scope.$on('$destroy', function () {
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});