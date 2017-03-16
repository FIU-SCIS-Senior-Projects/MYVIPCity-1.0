define(['vip/js/vip', 'jquery', 'rateyo'], function (vip, jQuery) {
	'use strict';

	vip.directive('vipRating', ['$timeout', function ($timeout) {
		return {
			restrict: 'ACE',

			require: 'ngModel',

			// new scope
			scope: true,

			link: function (scope, element, attrs, ngModelCtrl) {
				var listeners = [];

				jQuery(element).rateYo({
					starWidth: attrs.starWidth || '18px',
					ratedFill: '#fcd461',
					normalFill: '#eee',
					halfStar: attrs.halfStar === 'true',
					readOnly: attrs.readOnly === 'true',
					onSet: function (rating) {
						$timeout(function () {
							ngModelCtrl.$setViewValue(rating);
						}, 0);
					}
				});

				ngModelCtrl.$render = function () {
					jQuery(element).rateYo('option', 'rating', ngModelCtrl.$viewValue);
				};

				listeners.push(scope.$on('$destroy', function () {
					// unregister listeners
					for (var i = 0; i < listeners.length; i++)
						listeners[i]();
				}));
			}
		};
	}]);
});