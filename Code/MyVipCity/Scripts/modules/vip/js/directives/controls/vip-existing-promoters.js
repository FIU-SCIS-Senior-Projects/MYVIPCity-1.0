define(['vip/js/vip', 'jquery', 'angular'], function (vip, jQuery, angular) {
	'use strict';

	vip.directive('vipExistingPromoters', ['$http', function ($http) {
		return {
			restrict: 'ACE',

			// new scope
			scope: true,

			template:
				'<a ng-repeat="promoter in promoters" class="list-group-item media" href="#/promoter-profile/{{promoter.Id}}">' +
                    '<div class="pull-left">' +
                        '<img alt="" class="list-group__img img-circle vip-existing-promoters__img" ng-src="api/Pictures/{{promoter.ProfilePicture.BinaryDataId}}" width="65" height="65">' +
                    '</div>' +
                    '<div class="media-body list-group__text">' +
						'<div vip-promoter-name ng-model="promoter" wrap-with="strong" vip-read-only></div>' +
                        '<div class="rmd-rate" data-rate-value="5" data-rate-readonly="true"></div>' +
                    '</div>' +
                '</a>',

			link: function (scope, element, attrs) {
				var listeners = [];

				listeners.push(attrs.$observe('vipBusinessId', function (idValue) {
					// get the attribute value as an int
					var id = parseInt(idValue);
					// make sure is a valid int
					if (!isNaN(id) && id) {
						// make a request to bring all the promoters associated to this business
						$http.get('/api/Business/' + id + '/Promoters').then(function (response) {
							scope.promoters = response.data;
						}, function (error) {
						});
					}
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