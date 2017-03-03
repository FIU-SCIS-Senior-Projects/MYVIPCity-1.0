define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipNewPromoters', ['$compile', function ($compile) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				var listElement = angular.element(
					'<div>' +
						'<ul class="vip-new-promoter__list">' +
							'<li ng-repeat="item in ' + attrs.ngModel + '">' +
								'<div vip-new-promoter ng-model="item"></div>' +
							'</li>' +
						'</ul>' +
					'</div>'
				);

				element.append(listElement);
				$compile(listElement)(scope);

				listeners.push(scope.$on('$destroy', function () {
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);

	vip.directive('vipNewPromoter', ['$compile', function ($compile) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				var itemElement = angular.element(
					'<form name="form_' + scope.$id + '">' +
						'<div class="vip-new-promoter__field-container">' +
							'<div vip-textbox edit-mode-class="form-control" ng-model="' + attrs.ngModel + '.Name" placeholder="Name" name="Name" required></div>' +
							'<div ng-messages="form_' + scope.$id + '.Name.$error" class="mdc-text-red-500 animated fadeIn">' +
								'<div ng-message="required">' +
									'<small class="pull-left">Name is required</small>' +
								'</div>' +
							'</div>' +
						'</div>' +
						'<div class="vip-new-promoter__field-container">' +
							'<div vip-textbox edit-mode-class="form-control" ng-model="' + attrs.ngModel + '.Email" placeholder="Email" type="email" name="Email" required></div>' +
							'<div ng-messages="form_' + scope.$id + '.Email.$error" class="mdc-text-red-500 animated fadeIn">' +
								'<div ng-message="required">' +
									'<small class="pull-left">Valid email is required</small>' +
								'</div>' +
							'</div>' +
						'</div>' +
					'</form>'
				);

				element.append(itemElement);
				$compile(itemElement)(scope);

				listeners.push(scope.$on('$destroy', function () {
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});