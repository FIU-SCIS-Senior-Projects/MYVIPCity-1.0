define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipPromoterName', ['vipControlRenderingService', function (vipControlRenderingService) {
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
					var readElement = angular.element(
						'<' + tag + ' class="vip-promoter-name__name-part vip-promoter-name__first-name" ng-cloak>{{' + attrs.ngModel + '.FirstName}}</' + tag + '>' +
						'<' + tag + ' class="vip-promoter-name__name-part vip-promoter-name__nick-name" ng-show="' + attrs.ngModel + '.NickName" ng-cloak>{{' + attrs.ngModel + '.NickName}}</' + tag + '>' +
						'<' + tag + ' class="vip-promoter-name__name-part vip-promoter-name__last-name" ng-show="' + attrs.ngModel + '.LastName" ng-cloak>{{' + attrs.ngModel + '.LastName}}</' + tag + '>'
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
						'<div vip-textbox class="vip-promoter-name__name-part" ng-model="' + attrs.ngModel + '.FirstName" placeholder="First Name" auto-grow="true"></div>' +
						'<div vip-textbox class="vip-promoter-name__name-part" ng-model="' + attrs.ngModel + '.NickName" placeholder="Nickname" auto-grow="true"></div>' +
						'<div vip-textbox class="vip-promoter-name__name-part" ng-model="' + attrs.ngModel + '.LastName" placeholder="Last Name" auto-grow="true"></div>'
					);
					// add css class
					if (attrs.editModeClass)
						editElement.addClass(attrs.editModeClass);
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
					listeners.push(scope.$watch('renderingMode', function(value) {
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