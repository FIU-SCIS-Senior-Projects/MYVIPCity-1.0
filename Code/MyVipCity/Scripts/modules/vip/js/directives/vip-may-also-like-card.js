define(['vip/js/vip', 'lodash'], function (vip, _) {
	'use strict';

	vip.directive('vipMayAlsoLikeCard', ['vipBusinessService', function (vipBusinessService) {
		return {
			restrict: 'AC',

			replace: true,

			require: 'ngModel',

			template:
				'<div class="card hidden-xs hidden-sm hidden-print">' +
					'<div class="card__header">' +
						'<h2>You may also like...</h2>' +
						'<small>Check out these other great places</small>' +
					'</div>' +
					'<div class="list-group">' +
						'<a ng-repeat="business in suggestions" href="#/view-business/{{::business.FriendlyId}}" class="hvr-glow list-group-item media">' +
							'<div class="pull-left">' +
								'<img ng-src="{{::business.firstPictureUrl}}" alt="{{::business.Name}}" class="list-group__img" width="65">' +
							'</div>' +
							'<div class="media-body list-group__text">' +
								'<strong>{{::business.Name}}</strong>' +
								'<small>OPEN NOW</small>' +
							'</div>' +
						'</a>' +
					'</div>' +
				'</div>',

			link: function (scope, element, attrs, ngModelCtrl) {

				var showSuggestions = function (business) {
					var searchCriteria = {
						latitude: business.Address.Latitude,
						longitude: business.Address.Longitude,
						top: 6
					}

					vipBusinessService.search(searchCriteria).then(function(suggestedBussineses) {
						vipBusinessService.addFirstPictureUrl(suggestedBussineses);
						_.remove(suggestedBussineses, function(b) {
							return b.Id === business.Id;
						});
						scope.suggestions = suggestedBussineses;
					});
				};

				ngModelCtrl.$render = function () {
					var business = ngModelCtrl.$modelValue;
					if (business && business.Id && business.Address) {
						showSuggestions(business);
					}
				};
			}
		};
	}]);
});