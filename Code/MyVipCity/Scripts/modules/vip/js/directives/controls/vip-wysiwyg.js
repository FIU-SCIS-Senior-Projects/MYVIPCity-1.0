define(['vip/js/vip', 'jquery', 'angular', 'trumbowyg'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipWysiwyg', ['vipControlRenderingService', '$timeout', function (vipControlRenderingService, $timeout) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				// instantiate a control rendering service
				var controlRenderingService = vipControlRenderingService(scope, element);

				//ngModelCtrl.$render = function () {
				//	scope._body = ngModelCtrl.$viewValue;
				//};

				controlRenderingService.setCreateReadModeElementFunction(function () {
					// create the read mode element
					var readElement = angular.element('<div ng-bind-html="' + attrs.ngModel + '"></div>');
					// add css class
					if (attrs.readModeClass)
						readElement.addClass(attrs.readModeClass);
					// return element
					return readElement;
				});

				controlRenderingService.setCreateEditModeElementFunction(function () {
					// create edit mode element
					var editElement = angular.element('<div class="vip-wysiwyg-container"><div' + (attrs.placeholder ? ' placeholder="' + attrs.placeholder + '"' : '') + ' class="vip-wysiwyg-wysiwyg"></div></div>');
					// add css class
					if (attrs.editModeClass)
						editElement.addClass(attrs.editModeClass);

					jQuery(editElement)
						.find('.vip-wysiwyg-wysiwyg')
						.trumbowyg({
							autogrow: true
						})
						.on('tbwchange', function (e) {
							var content = jQuery(e.currentTarget).trumbowyg('html');
							ngModelCtrl.$setViewValue(content);
						});

					jQuery(editElement)
						.find('.vip-wysiwyg-wysiwyg')
						.trumbowyg('html', ngModelCtrl.$viewValue);

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