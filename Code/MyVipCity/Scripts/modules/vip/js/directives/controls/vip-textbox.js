define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipTextbox', ['$compile', function ($compile) {
		return {
			restrict: 'ACE',
			
			require: 'ngModel',

			// isolated scope
			scope: {},

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [], editModeElement, readModeElement;

				ngModelCtrl.$render = function () {
					scope._text = ngModelCtrl.$viewValue;
				};

				listeners.push(scope.$watch('_text', function (value) {
					ngModelCtrl.$setViewValue(value);
				}));

				var createReadModeElement = function () {
					// tag to wrap the read only text
					var tag = attrs.wrapWith || 'label';
					// create the read mode element
					var element = angular.element('<' + tag + '>{{_text}}</' + tag + '>');
					// add css class
					if (attrs.readModeClass)
						element.addClass(attrs.readModeClass);
					// return element
					return element;
				};

				var createEditModeElement = function () {
					// create edit mode element
					var element = angular.element('<input type="text" placeholder="' + (attrs.placeholder || '') + '" ng-model="_text"' + (!!attrs.autoGrow ? ' vip-auto-grow-input' : '') + '/>');
					// add css class
					if (attrs.editModeClass)
						element.addClass(attrs.editModeClass);
					// return element
					return element;
				};

				var hideEditModeElement = function () {
					if (editModeElement)
						jQuery(editModeElement).hide();
				};

				var hideReadModeElement = function () {
					if (readModeElement)
						jQuery(readModeElement).hide();
				};

				var showEditModeElement = function () {
					if (editModeElement)
						jQuery(editModeElement).show();
				};

				var showReadModeElement = function () {
					if (readModeElement)
						jQuery(readModeElement).show();
				};

				var showReadMode = function () {
					hideEditModeElement();
					if (readModeElement) {
						showReadModeElement();
					}
					else {
						readModeElement = createReadModeElement();
						element.append(readModeElement);
						$compile(readModeElement)(scope);
					}
				};

				var showEditMode = function () {
					hideReadModeElement();
					if (editModeElement) {
						showEditModeElement();
					}
					else {
						editModeElement = createEditModeElement();
						element.append(editModeElement);
						$compile(editModeElement)(scope);
					}
				};

				var setRenderingMode = function (renderingMode) {
					if (renderingMode === vip.renderingModes.read)
						showReadMode();
					else if (renderingMode === vip.renderingModes.edit)
						showEditMode();
				}

				listeners.push(scope.$watch('$parent.renderingMode', function (value) {
					setRenderingMode(value);
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