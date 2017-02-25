define(['vip/js/vip', 'jquery', 'angular', 'trumbowyg'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipTags', ['vipControlRenderingService', '$mdConstant', 'vipColorsService', 'vipUtilsService', function (vipControlRenderingService, $mdConstant, vipColorsService, vipUtilsService) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			controller: ['$scope', function ($scope) {
				// get all the color names
				var colors = vipColorsService.getColorsName();
				// shuffle the array
				vipUtilsService.shuffleArray(colors);
				// compute css classes for tags
				$scope.cssClasses = [];
				for (var i = 0; i < colors.length; i++)
					$scope.cssClasses[i] = vipColorsService.getRandomBackgroundColorClass(colors[i]);
				// set the separator keys for edit mode
				$scope.separatorKeys = [$mdConstant.KEY_CODE.ENTER, $mdConstant.KEY_CODE.COMMA];
			}],
			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				// instantiate a control rendering service
				var controlRenderingService = vipControlRenderingService(scope, element);

				ngModelCtrl.$render = function () {
					scope._items = ngModelCtrl.$viewValue ? ngModelCtrl.$viewValue.split(',') : [];
				};

				listeners.push(scope.$watchCollection('_items', function (items) {
					if (items === null || angular.isUndefined(items) || !angular.isArray(items) || !items.length) {
						ngModelCtrl.$setViewValue(null);
					}
					else {
						ngModelCtrl.$setViewValue(items.join(','));
					}
				}));

				controlRenderingService.setCreateReadModeElementFunction(function () {
					// create the read mode element
					var readElement = angular.element(
						'<ul>' +
							'<li ng-repeat="item in _items track by $index" class="{{cssClasses[$index % cssClasses.length]}}">{{item}}</li>' +
						'</ul>'
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
						'<md-chips ng-model="_items"' + (attrs.placeholder ? ' placeholder="' + attrs.placeholder + '"' : '') + 'md-separator-keys="separatorKeys" md-enable-chip-edit="true">' +
						'</md-chips>'
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