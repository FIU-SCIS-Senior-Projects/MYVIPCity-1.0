define(['vip/js/vip', 'jquery', 'angular', 'autosize'], function (vip, jQuery, angular, autosize) {
	'use strict';

	vip.directive('vipTextarea', ['vipControlRenderingService', function (vipControlRenderingService) {
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
					var tag = attrs.wrapWith || 'p';
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
					var editElement = angular.element('<textarea placeholder="' + (attrs.placeholder || '') + '" ng-model="' + attrs.ngModel + '"' + (attrs.ngModelOptions ? ' ng-model-options="' + attrs.ngModelOptions + '"' : '') + (attrs.maxlength ? ' maxlength="' + attrs.maxlength + '"' : '') + '/>');
					autosize(editElement);
					// add css class
					if (attrs.editModeClass)
						editElement.addClass(attrs.editModeClass);
					// set input type
					var inputType = attrs.type || 'text';
					editElement.attr('type', inputType);
					// return element
					return editElement;
				});

				// check if the directive will be rendered in read mode only
				if (angular.isDefined(attrs.vipReadOnly)) {
					controlRenderingService.setRenderingMode(vip.renderingModes.read);
				} else if (angular.isDefined(attrs.vipEditOnly)) {
					// the directive will be rendered in edit mode only
					controlRenderingService.setRenderingMode(vip.renderingModes.edit);
				}
				else {
					// directive can change read/edit mode dinamically
					listeners.push(scope.$watch('renderingMode', function (value) {
						controlRenderingService.setRenderingMode(value);
					}));
				}

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