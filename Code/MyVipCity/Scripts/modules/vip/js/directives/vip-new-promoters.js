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
					'<form name="newPromotersForm">' +
						'<ul class="vip-new-promoter__list">' +
							'<li ng-repeat="item in ' + attrs.ngModel + '">' +
								'<div vip-new-promoter ng-model="item"></div>' +
								'<i class="zmdi zmdi-close vip-new-promoter__delete" ng-click="removeNewPromoter($event, item)"></i>' +
							'</li>' +
						'</ul>' +
						'<div class="vip-new-promoter__actions-container">' +
							'<button class="btn btn-primary vip-new-promoter__add-promoter" ng-disabled="newPromotersForm.$invalid" ng-click="addNewPromoter()">' +
								'<i class="zmdi zmdi-plus"></i>' +
								'Add Promoter' +
							'</button>' +
							'<button class="btn vip-new-promoter__send-invitation" ng-disabled="newPromotersForm.$invalid" ng-show="' + attrs.ngModel + '.length" ng-click="sendInvitation()">' +
								'<i class="zmdi zmdi-email"></i>' +
								'Send Invitation' +
							'</button>' +
						'</div>' +
					'</form>'
				);

				scope.addNewPromoter = function () {
					var promoterList = ngModelCtrl.$viewValue;
					if (promoterList)
						promoterList.push({ Id: 0 });
				};

				scope.removeNewPromoter = function($event, item) {
					var promoterList = ngModelCtrl.$viewValue;
					var index = promoterList.indexOf(item);
					// remove it
					if (index > -1) {
						promoterList.splice(index, 1);
					}
				};

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

			link: function (scope, element, attrs) {
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
									'<small class="pull-left">A valid email is required</small>' +
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